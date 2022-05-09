using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Interfaces
{
    public interface ICallService
    {
        Task<CallResponseModel> FindAsync(Expression<Func<Call, bool>> predicate);

        Task<List<CallResponseModel>> GetAsync(Expression<Func<Call, bool>> predicate);

        Task<PaginationResponseModel<CallResponseModel>> GetAsync(int offset, int limit);

        Task<CallResponseModel> CreateAsync(CallCreateRequestModel callCreateRequestModel);

        Task<CallResponseModel> UpdateAsync(CallUpdateRequestModel callUpdateRequestModel);

        Task<bool> DeleteAsync(int callId);
    }
}
