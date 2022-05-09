using AutoMapper;
using Freshness.Common.CustomExceptions;
using Freshness.Common.ResponseMessages;
using Freshness.DAL.Interfaces;
using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using Freshness.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Services
{
    public class CallService : ICallService
    {
        private readonly ITelegramBotCallService _telegramCallService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CallService(ITelegramBotCallService telegramCallService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _telegramCallService = telegramCallService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CallResponseModel> FindAsync(Expression<Func<Call, bool>> predicate)
        {
            var call = await _unitOfWork.Repository<Call>().FindAsync(predicate);

            if (call == null)
            {
                throw new CustomException(ResponseMessage.CallDoesNotExist);
            }

            var callResponseModels = _mapper.Map<Call, CallResponseModel>(call);

            return callResponseModels;
        }

        public async Task<List<CallResponseModel>> GetAsync(Expression<Func<Call, bool>> predicate)
        {
            var calls = await _unitOfWork.Repository<Call>().GetAsync(predicate);

            var callResponseModels = _mapper.Map<List<Call>, List<CallResponseModel>>(calls);

            return callResponseModels;
        }

        public async Task<PaginationResponseModel<CallResponseModel>> GetAsync(int offset, int limit)
        {
            var calls = await _unitOfWork.Repository<Call>().GetAsync(offset, limit);

            var entities = _mapper.Map<List<Call>, List<CallResponseModel>>(calls.Item1);

            var paginationResponseModel = new PaginationResponseModel<CallResponseModel>
            {
                Entities = entities,
                TotalCount = calls.Item2
            };

            return paginationResponseModel;
        }

        // Customer can only create one call per day
        public async Task<CallResponseModel> CreateAsync(CallCreateRequestModel callCreateRequestModel)
        {
            var call = await _unitOfWork.Repository<Call>().FindAsync(item => item.AddedDate.Date == DateTime.Now.Date &&
                item.Phone == callCreateRequestModel.Phone);

            if (call != null)
            {
                throw new CustomException(ResponseMessage.CallAlreadyExists);
            }

            call = new Call
            {
                Name = callCreateRequestModel.Name.Trim(),
                Phone = callCreateRequestModel.Phone,
                AddedDate = DateTime.Now,
                IsDone = false
            };

            

            var createdCall = await _unitOfWork.Repository<Call>().InsertAsync(call);

            await _unitOfWork.SaveChangesAsync();

            var callResponseModel = _mapper.Map<Call, CallResponseModel>(createdCall);

            //Sending notification to telegram
            await _telegramCallService.BulkSent(callResponseModel);

            return callResponseModel;
        }

        public async Task<CallResponseModel> UpdateAsync(CallUpdateRequestModel callUpdateRequestModel)
        {
            var call = await _unitOfWork.Repository<Call>().FindAsync(item => item.Id == callUpdateRequestModel.Id);

            if (call == null)
            {
                throw new CustomException(ResponseMessage.CallDoesNotExist);
            }

            call.Name = callUpdateRequestModel.Name.Trim();

            if (callUpdateRequestModel.IsDone == true && call.IsDone == false)
            {
                var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Id == callUpdateRequestModel.WorkerId);

                if (worker == null)
                {
                    throw new CustomException(ResponseMessage.WorkerDoesNotExist);
                }

                call.IsDone = callUpdateRequestModel.IsDone;
                call.IsDoneDate = DateTime.Now;
                call.WorkerId = callUpdateRequestModel.WorkerId;
            }

            var updatedCall = _unitOfWork.Repository<Call>().Update(call);

            await _unitOfWork.SaveChangesAsync();

            var callResponseModel = _mapper.Map<Call, CallResponseModel>(updatedCall);

            //Sending notification to telegram
            if (callUpdateRequestModel.IsDone == false && callResponseModel.IsDone == true)
            {
                await _telegramCallService.BulkSent(callResponseModel);
            }

            return callResponseModel;
        }

        public async Task<bool> DeleteAsync(int callId)
        {
            var call = await _unitOfWork.Repository<Call>().FindAsync(item => item.Id == callId);

            if (call == null)
            {
                throw new CustomException(ResponseMessage.CallDoesNotExist);
            }

            var result = await _unitOfWork.Repository<Call>().RemoveAsync(callId);

            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
