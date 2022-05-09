using Freshness.Domain.Entities;
using Freshness.Models.ResponseModels;

namespace Freshness.Services.Interfaces
{
    public interface IJwtService
    {
        /// <summary>
        /// Generates or updates (if tokens exist) access and refresh tokens for user
        /// </summary>
        /// <param name="user">Worker</param>
        /// <returns>TokenResponseModel</returns>
        TokenResponseModel GenerateToken(Worker user);
    }
}
