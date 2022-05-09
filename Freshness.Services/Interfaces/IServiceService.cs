using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Interfaces
{
    public interface IServiceService
    {
        Task<ServiceResponseModel> FindAsync(Expression<Func<Service, bool>> predicate);

        Task<List<ServiceResponseModel>> GetAsync(Expression<Func<Service, bool>> predicate);

        Task<PaginationResponseModel<ServiceResponseModel>> GetAsync(int offset, int limit);

        Task<ServiceResponseModel> CreateAsync(ServiceCreateRequestModel serviceCreateRequestModel);

        Task<ServiceResponseModel> UpdateAsync(ServiceUpdateRequestModel serviceUpdateRequestModel);

        Task<bool> DeleteAsync(int serviceId);
    }
}
