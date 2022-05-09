using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerResponseModel> FindAsync(Expression<Func<Customer, bool>> predicate);

        Task<List<CustomerResponseModel>> GetAsync(Expression<Func<Customer, bool>> predicate);

        Task<PaginationResponseModel<CustomerResponseModel>> GetAsync(int offset, int limit);

        Task<CustomerResponseModel> CreateAsync(CustomerCreateRequestModel customerCreateRequestModel);

        Task<CustomerResponseModel> UpdateAsync(CustomerUpdateRequestModel customerUpdateRequestModel);

        Task<bool> DeleteAsync(int customerId);
    }
}
