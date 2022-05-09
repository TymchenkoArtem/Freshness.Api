using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Interfaces
{
    public interface IWorkerService
    {
        Task<TokenResponseModel> LogInAsync(LoginRequestModel loginModel);

        Task<TokenResponseModel> RefreshTokenAsync(RefreshTokenRequestModel refreshTokenRequestModel);

        Task ResetPasswordAsync(ResetPasswordRequestModel resetModel);

        Task<WorkerResponseModel> FindAsync(Expression<Func<Worker, bool>> predicate);

        Task<List<WorkerResponseModel>> GetAsync(Expression<Func<Worker, bool>> predicate);

        Task<PaginationResponseModel<WorkerResponseModel>> GetAsync(int offset, int limit);

        Task<WorkerResponseModel> CreateAsync(WorkerCreateRequestModel workerCreateRequestModel);

        Task<WorkerResponseModel> UpdateAsync(WorkerUpdateRequestModel workerUpdateRequestModel);

        Task<bool> DeleteAsync(int workerId);
    }
}
