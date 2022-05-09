using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressResponseModel> FindAsync(Expression<Func<Address, bool>> predicate);

        Task<List<AddressResponseModel>> GetAsync(Expression<Func<Address, bool>> predicate);

        Task<PaginationResponseModel<AddressResponseModel>> GetAsync(int offset, int limit);

        Task<Address> CreateAsync(AddressCreateRequestModel addressCreateRequestModel);

        Task<Address> UpdateAsync(AddressUpdateRequestModel addressUpdateRequestModel);

        Task<bool> DeleteAsync(int addressId);

        Task<Address> CreateAddressModelAsync(AddressCreateRequestModel addressCreateRequestModel);
    }
}
