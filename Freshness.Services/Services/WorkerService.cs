using AutoMapper;
using Common.Settings;
using Freshness.Common.Constants;
using Freshness.Common.CustomExceptions;
using Freshness.Common.Extensions;
using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using Freshness.DAL.Interfaces;
using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using Freshness.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Freshness.Services.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly ITelegramBotCallService _telegramCallService;
        private readonly ITelegramBotOrderService _telegramOrderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly JwtBearerSettings _jwtBearerSettings;

        public WorkerService(ITelegramBotCallService telegramCallService,
            ITelegramBotOrderService telegramOrderService,
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IMapper mapper,
            IOptions<JwtBearerSettings> jwtBearerSettings)
        {
            _telegramCallService = telegramCallService;
            _telegramOrderService = telegramOrderService;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
            _jwtBearerSettings = jwtBearerSettings.Value;
        }

        public async Task<TokenResponseModel> LogInAsync(LoginRequestModel loginModel)
        {
            var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Phone == loginModel.Phone &&
                item.Password == loginModel.Password.GetCustomHash());

            if (worker == null)
            {
                throw new CustomException(ResponseMessage.WorkerDoesNotExist);
            }

            var accessToken = _jwtService.GenerateToken(worker);

            worker.AccessToken = accessToken.AccessToken;
            worker.AccessTokenExpireDate = DateTime.UtcNow.AddDays(_jwtBearerSettings.ExpireDate);

            _unitOfWork.Repository<Worker>().Update(worker);
            await _unitOfWork.SaveChangesAsync();

            return accessToken;
        }

        public async Task<TokenResponseModel> RefreshTokenAsync(RefreshTokenRequestModel refreshTokenRequestModel)
        {
            var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.AccessToken == refreshTokenRequestModel.AccessToken &&
                item.AccessTokenExpireDate > DateTime.UtcNow);

            if (worker == null)
            {
                throw new CustomException(ResponseMessage.WorkerDoesNotExist);
            }

            var accessToken = _jwtService.GenerateToken(worker);

            worker.AccessToken = accessToken.AccessToken;
            worker.AccessTokenExpireDate = DateTime.UtcNow.AddDays(_jwtBearerSettings.ExpireDate);

            _unitOfWork.Repository<Worker>().Update(worker);
            await _unitOfWork.SaveChangesAsync();

            return accessToken;
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestModel resetModel)
        {
            var telegramCallUser = await _unitOfWork.Repository<TelegramBotCallUser>().FindAsync(item => item.Phone == resetModel.Phone);
            var telegramOrderUser = await _unitOfWork.Repository<TelegramBotOrderUser>().FindAsync(item => item.Phone == resetModel.Phone);

            var adminsCall = await _unitOfWork.Repository<TelegramBotCallUser>().GetAsync(item => item.Role == Role.Admin && item.AuthorizationStage == AuthorizationStage.SignedIn);
            var adminsOrder = await _unitOfWork.Repository<TelegramBotOrderUser>().GetAsync(item => item.Role == Role.Admin && item.AuthorizationStage == AuthorizationStage.SignedIn);

            var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Phone == resetModel.Phone);

            if (worker == null)
            {
                throw new CustomException(ResponseMessage.WorkerDoesNotExist);
            }

            var newPassword = GeneratePassword(ValidationValues.PasswordMinLength);

            worker.Password = newPassword.GetCustomHash();

            _unitOfWork.Repository<Worker>().Update(worker);

            await _unitOfWork.SaveChangesAsync();

            adminsCall.ForEach(async admin =>
            {
                if (admin.ChatId != telegramCallUser?.ChatId)
                {
                    await _telegramCallService.SendMessage(admin.ChatId, $"Користувач {worker.Name} запросив відновлення пароля за номером телефону +38{worker.Phone}. Новий пароль : {newPassword}");
                }
            });

            adminsOrder.ForEach(async admin =>
            {
                if (admin.ChatId != telegramOrderUser?.ChatId)
                {
                    await _telegramOrderService.SendMessage(admin.ChatId, $"Користувач {worker.Name} запросив відновлення пароля за номером телефону +38{worker.Phone}. Новий пароль : {newPassword}");
                }
            });

            if (telegramCallUser != null)
            {
                await _telegramCallService.SendMessage(telegramCallUser.ChatId, $"Ваш новий пароль: {newPassword}");
            }

            if (telegramOrderUser != null)
            {
                await _telegramOrderService.SendMessage(telegramOrderUser.ChatId, $"Ваш новий пароль: {newPassword}");
            }
        }

        public async Task<WorkerResponseModel> FindAsync(Expression<Func<Worker, bool>> predicate)
        {
            var worker = await _unitOfWork.Repository<Worker>().FindAsync(predicate);

            if (worker == null)
            {
                throw new CustomException(ResponseMessage.WorkerDoesNotExist);
            }

            var workerResponseModel = _mapper.Map<Worker, WorkerResponseModel>(worker);

            return workerResponseModel;
        }

        public async Task<List<WorkerResponseModel>> GetAsync(Expression<Func<Worker, bool>> predicate)
        {
            var workers = await _unitOfWork.Repository<Worker>().GetAsync(predicate);

            var workerResponseModels = _mapper.Map<List<Worker>, List<WorkerResponseModel>>(workers);

            return workerResponseModels;
        }

        public async Task<PaginationResponseModel<WorkerResponseModel>> GetAsync(int offset, int limit)
        {
            var workers = await _unitOfWork.Repository<Worker>().GetAsync(offset, limit);

            var workerResponseModels = _mapper.Map<List<Worker>, List<WorkerResponseModel>>(workers.Item1);

            var paginationResponseModel = new PaginationResponseModel<WorkerResponseModel>
            {
                Entities = workerResponseModels,
                TotalCount = workers.Item2
            };

            return paginationResponseModel;
        }

        public async Task<WorkerResponseModel> CreateAsync(WorkerCreateRequestModel workerCreateRequestModel)
        {
            var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Phone == workerCreateRequestModel.Phone ||
                item.Email == workerCreateRequestModel.Email);

            if (worker != null)
            {
                throw new CustomException(ResponseMessage.WorkerAlreadyExists);
            }

            worker = _mapper.Map<WorkerCreateRequestModel, Worker>(workerCreateRequestModel);

            worker.Password = workerCreateRequestModel.Password.GetCustomHash();
            worker.AddedDate = DateTime.Now;

            // Check whether role exists. Otherwise user get smallest role
            if (!Role.Find(worker.Role))
            {
                worker.Role = Role.Worker;
            }

            var createdWorker = await _unitOfWork.Repository<Worker>().InsertAsync(worker);

            await _unitOfWork.SaveChangesAsync();

            var workerResponseModel = _mapper.Map<Worker, WorkerResponseModel>(createdWorker);

            return workerResponseModel;
        }

        public async Task<WorkerResponseModel> UpdateAsync(WorkerUpdateRequestModel workerUpdateRequestModel)
        {
            var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Id != workerUpdateRequestModel.Id &&
                (item.Phone == workerUpdateRequestModel.Phone ||
                item.Email == workerUpdateRequestModel.Email));

            if (worker != null)
            {
                throw new CustomException(ResponseMessage.WorkerAlreadyExists);
            }

            worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Id != workerUpdateRequestModel.Id);

            if (worker == null)
            {
                throw new CustomException(ResponseMessage.WorkerDoesNotExist);
            }

            //AddedDate is not changeable
            var addedDate = worker.AddedDate;

            worker = _mapper.Map<WorkerUpdateRequestModel, Worker>(workerUpdateRequestModel);

            worker.AddedDate = addedDate;
            worker.Password = workerUpdateRequestModel.Password.GetCustomHash();

            // Check whether role exists. Otherwise user get smallest role
            if (!Role.Find(worker.Role))
            {
                worker.Role = Role.Worker;
            }

            var updatedWorker = _unitOfWork.Repository<Worker>().Update(worker);

            await _unitOfWork.SaveChangesAsync();

            var workerResponseModel = _mapper.Map<Worker, WorkerResponseModel>(updatedWorker);

            return workerResponseModel;
        }

        public async Task<bool> DeleteAsync(int workerId)
        {
            var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Id != workerId);

            if (worker == null)
            {
                throw new CustomException(ResponseMessage.WorkerDoesNotExist);
            }

            var result = await _unitOfWork.Repository<Worker>().RemoveAsync(workerId);

            await _unitOfWork.SaveChangesAsync();

            return result;
        }


        private string GeneratePassword(int length)
        {
            //const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            const string valid = "1234567890";

            StringBuilder res = new StringBuilder();

            Random rnd = new Random();

            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }
    }
}
