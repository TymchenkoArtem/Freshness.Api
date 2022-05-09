using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseModel> FindAsync(Expression<Func<Order, bool>> predicate);

        Task<List<OrderResponseModel>> GetAsync(Expression<Func<Order, bool>> predicate);

        Task<PaginationResponseModel<OrderResponseModel>> GetAsync(int offset, int limit);

        Task<OrderResponseModel> CreateAsync(OrderCreateRequestModel orderCreateRequestModel);

        Task<OrderResponseModel> UpdateAsync(OrderUpdateRequestModel orderUpdateRequestModel);

        Task<bool> DeleteAsync(int orderId);
    }
}
