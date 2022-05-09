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
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponseModel> FindAsync(Expression<Func<Service, bool>> predicate)
        {
            var service = await _unitOfWork.Repository<Service>().FindAsync(predicate);

            if (service == null)
            {
                throw new CustomException(ResponseMessage.ServiceDoesNotExist);
            }

            var serviceResponseModel = _mapper.Map<ServiceResponseModel>(service);

            return serviceResponseModel;
        }

        public async Task<List<ServiceResponseModel>> GetAsync(Expression<Func<Service, bool>> predicate)
        {
            var services = await _unitOfWork.Repository<Service>().GetAsync(predicate);

            var serviceResponseModels = _mapper.Map<List<Service>, List<ServiceResponseModel>>(services);

            return serviceResponseModels;
        }

        public async Task<PaginationResponseModel<ServiceResponseModel>> GetAsync(int offset, int limit)
        {
            var services = await _unitOfWork.Repository<Service>().GetAsync(offset, limit);

            var serviceResponseModels = _mapper.Map<List<Service>, List<ServiceResponseModel>>(services.Item1);

            var paginationResponseModel = new PaginationResponseModel<ServiceResponseModel>
            {
                Entities = serviceResponseModels,
                TotalCount = services.Item2
            };

            return paginationResponseModel;
        }

        public async Task<ServiceResponseModel> CreateAsync(ServiceCreateRequestModel serviceCreateRequestModel)
        {
            var service = await _unitOfWork.Repository<Service>().FindAsync(item => item.Name.Trim().ToLower() == serviceCreateRequestModel.Name.Trim().ToLower() &&
                item.Language == serviceCreateRequestModel.Language);

            if (service != null)
            {
                throw new CustomException(ResponseMessage.ServiceAlreadyExists);
            }

            service = _mapper.Map<ServiceCreateRequestModel, Service>(serviceCreateRequestModel);

            service.Name = serviceCreateRequestModel.Name.Trim();

            var createdService = await _unitOfWork.Repository<Service>().InsertAsync(service);

            await _unitOfWork.SaveChangesAsync();

            var serviceResponseModel = _mapper.Map<Service, ServiceResponseModel>(createdService);

            return serviceResponseModel;
        }

        public async Task<ServiceResponseModel> UpdateAsync(ServiceUpdateRequestModel serviceUpdateRequestModel)
        {
            var service = await _unitOfWork.Repository<Service>().FindAsync(item => item.Id != serviceUpdateRequestModel.Id &&
                item.Name.Trim().ToLower() == serviceUpdateRequestModel.Name.Trim().ToLower() &&
                item.Language == serviceUpdateRequestModel.Language);

            if (service != null)
            {
                throw new CustomException(ResponseMessage.ServiceAlreadyExists);
            }

            service = await _unitOfWork.Repository<Service>().FindAsync(item => item.Id == serviceUpdateRequestModel.Id);

            if (service == null)
            {
                throw new CustomException(ResponseMessage.ServiceDoesNotExist);
            }

            service.Name = serviceUpdateRequestModel.Name.Trim();
            service.Language = serviceUpdateRequestModel.Language;
            service.Price = serviceUpdateRequestModel.Price;

            var updatedService = _unitOfWork.Repository<Service>().Update(service);

            await _unitOfWork.SaveChangesAsync();

            var serviceResponseModel = _mapper.Map<Service, ServiceResponseModel>(updatedService);

            return serviceResponseModel;
        }

        public async Task<bool> DeleteAsync(int serviceId)
        {
            var service = await _unitOfWork.Repository<Service>().FindAsync(item => item.Id == serviceId);

            if (service == null)
            {
                throw new CustomException(ResponseMessage.ServiceDoesNotExist);
            }

            var result = await _unitOfWork.Repository<Service>().RemoveAsync(serviceId);

            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
