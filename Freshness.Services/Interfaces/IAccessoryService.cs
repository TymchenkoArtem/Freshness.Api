using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Interfaces
{
    public interface IAccessoryService
    {
        Task<AccessoryResponseModel> FindAsync(Expression<Func<Accessory, bool>> predicate);

        Task<List<AccessoryResponseModel>> GetAsync(Expression<Func<Accessory, bool>> predicate);

        Task<PaginationResponseModel<AccessoryResponseModel>> GetAsync(int offset, int limit);

        Task<AccessoryResponseModel> CreateAsync(AccessoryCreateRequestModel accessoryCreateRequestModel);

        Task<AccessoryResponseModel> UpdateAsync(AccessoryUpdateRequestModel accessoryUpdateRequestModel);

        Task<bool> DeleteAsync(int accessoryId);
    }
}
