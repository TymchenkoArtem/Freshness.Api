using AutoMapper;
using Freshness.Common.Constants;
using Freshness.Common.CustomExceptions;
using Freshness.Common.ResponseMessages;
using Freshness.DAL.Interfaces;
using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using Freshness.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly ITelegramBotOrderService _telegramOrderService;
        private readonly ICustomerService _customerService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(ITelegramBotOrderService telegramOrderService, ICustomerService customerService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _telegramOrderService = telegramOrderService;
            _customerService = customerService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderResponseModel> FindAsync(Expression<Func<Order, bool>> predicate)
        {
            var order = await _unitOfWork.Repository<Order>().FindAsync(predicate);

            var orderAccessories = await _unitOfWork.Repository<OrderAccessory>().GetAsync(item => item.OrderId == order.Id);

            var accessories = new List<Accessory>();

            foreach (var orderAccessory in orderAccessories)
            {
                var accessory = await _unitOfWork.Repository<Accessory>().FindAsync(item => item.Id == orderAccessory.AccessoryId);

                if (accessory != null)
                {
                    accessories.Add(accessory);
                }
            }

            //Getting accessories for order
            var accessoryResponseModels = _mapper.Map<List<Accessory>, List<AccessoryResponseModel>>(accessories);

            //Getting customer for order
            var customerResponseModel = await _customerService.FindAsync(item => item.Id == order.CustomerId);

            //Generating orderResponseModel
            var orderResponseModel = _mapper.Map<Order, OrderResponseModel>(order);

            orderResponseModel.Note = order.Note.Trim();
            orderResponseModel.Accessories = accessoryResponseModels;
            orderResponseModel.Customer = customerResponseModel;

            return orderResponseModel;
        }

        public async Task<List<OrderResponseModel>> GetAsync(Expression<Func<Order, bool>> predicate)
        {
            var orders = await _unitOfWork.Repository<Order>().GetAsync(predicate);

            var orderResponseModelList = await GenerateOrderResponseModelList(orders);

            return orderResponseModelList;
        }

        public async Task<PaginationResponseModel<OrderResponseModel>> GetAsync(int offset, int limit)
        {
            var orders = await _unitOfWork.Repository<Order>().GetAsync(offset, limit);

            var entities = await GenerateOrderResponseModelList(orders.Item1);

            var paginationResponseModel = new PaginationResponseModel<OrderResponseModel>
            {
                Entities = entities,
                TotalCount = orders.Item2
            };

            return paginationResponseModel;
        }

        // Only one order per day
        public async Task<OrderResponseModel> CreateAsync(OrderCreateRequestModel orderCreateRequestModel)
        {
            var order = await _unitOfWork.Repository<Order>().FindAsync(item => item.AddedDate.Date == DateTime.Now.Date &&
                item.Customer.Phone == orderCreateRequestModel.Customer.Phone &&
                item.Customer.Address.District.Name == orderCreateRequestModel.Customer.Address.District &&
                item.Customer.Address.Street.Name == orderCreateRequestModel.Customer.Address.Street &&
                item.Customer.Address.House == orderCreateRequestModel.Customer.Address.House);

            if (order != null)
            {
                throw new CustomException(ResponseMessage.OrderAlreadyExists);
            }

            //Retrieves customer or create if needed
            var customer = await _customerService.FindAsync(item => item.Name == orderCreateRequestModel.Customer.Name &&
                item.Phone == orderCreateRequestModel.Customer.Phone &&
                item.Address.District.Name == orderCreateRequestModel.Customer.Address.District &&
                item.Address.Street.Name == orderCreateRequestModel.Customer.Address.Street &&
                item.Address.House == orderCreateRequestModel.Customer.Address.House);

            if (customer == null)
            {
                customer = await _customerService.CreateAsync(orderCreateRequestModel.Customer);
            }

            //Create order
            order = new Order
            {
                DeliveryDate = orderCreateRequestModel.DeliveryDate,
                DeliveryTime = orderCreateRequestModel.DeliveryTime,
                AddedDate = DateTime.Now,
                IsDone = false,
                Note = orderCreateRequestModel.Note.Trim(),
                Container = orderCreateRequestModel.Container,
                CustomerId = customer.Id,
                Amount = orderCreateRequestModel.Amount,
                TotalCost = await CountTotalCostAsync(orderCreateRequestModel.Container, orderCreateRequestModel.Amount, orderCreateRequestModel.Accessories)
            };

            var createdOrder = await _unitOfWork.Repository<Order>().InsertAsync(order);

            await _unitOfWork.SaveChangesAsync();

            //Add relation between order and accessories

            var orderAccessories = new List<OrderAccessory>();

            foreach (var accessoryId in orderCreateRequestModel.Accessories)
            {
                var accessory = await _unitOfWork.Repository<Accessory>().FindAsync(item => item.Id == accessoryId);

                if (accessory != null)
                {
                    var orderAccessory = new OrderAccessory
                    {
                        OrderId = createdOrder.Id,
                        AccessoryId = accessoryId
                    };

                    await _unitOfWork.Repository<OrderAccessory>().InsertAsync(orderAccessory);

                    await _unitOfWork.SaveChangesAsync();
                }
            }

            //Retrieves created order
            var orderResponseModel = await FindAsync(item => item.Id == createdOrder.Id);

            //Sending notification to telegram
            await _telegramOrderService.BulkSent(orderResponseModel);

            return orderResponseModel;
        }

        public async Task<OrderResponseModel> UpdateAsync(OrderUpdateRequestModel orderUpdateRequestModel)
        {
            var order = await _unitOfWork.Repository<Order>().FindAsync(item => item.Id == orderUpdateRequestModel.Id);

            if (order == null)
            {
                throw new CustomException(ResponseMessage.OrderDoesNotExist);
            }

            //Updating relations between order and accessories
            var orderAccesories = await _unitOfWork.Repository<OrderAccessory>().GetAsync(item => item.OrderId == order.Id);

            foreach (var orderAccessory in orderAccesories)
            {
                if (!orderUpdateRequestModel.Accessories.Contains(orderAccessory.AccessoryId))
                {
                    await _unitOfWork.Repository<OrderAccessory>().RemoveAsync(orderAccessory.Id);

                    await _unitOfWork.SaveChangesAsync();
                }
            }

            foreach (var accessoryId in orderUpdateRequestModel.Accessories)
            {
                var orderAccessory = orderAccesories.Find(item => item.AccessoryId == accessoryId);

                if (orderAccessory == null)
                {
                    orderAccessory = new OrderAccessory
                    {
                        OrderId = order.Id,
                        AccessoryId = accessoryId
                    };

                    await _unitOfWork.Repository<OrderAccessory>().InsertAsync(orderAccessory);

                    await _unitOfWork.SaveChangesAsync();
                }
            }

            //Updating customer
            var customer = await _customerService.FindAsync(item => item.Name == orderUpdateRequestModel.Customer.Name &&
                item.Phone == orderUpdateRequestModel.Customer.Phone &&
                item.Address.District.Name == orderUpdateRequestModel.Customer.Address.District &&
                item.Address.Street.Name == orderUpdateRequestModel.Customer.Address.Street &&
                item.Address.House == orderUpdateRequestModel.Customer.Address.House);

            if (customer == null)
            {
                var customerCreateRequestModel = _mapper.Map<CustomerUpdateRequestModel, CustomerCreateRequestModel>(orderUpdateRequestModel.Customer);

                customer = await _customerService.CreateAsync(customerCreateRequestModel);
            }
            else
            {
                customer = await _customerService.UpdateAsync(orderUpdateRequestModel.Customer);
            }

            //Adding worker who done the order
            if (orderUpdateRequestModel.IsDone == true && order.IsDone == false)
            {
                var worker = await _unitOfWork.Repository<Worker>().FindAsync(item => item.Id == orderUpdateRequestModel.WorkerId);

                if (worker == null)
                {
                    throw new CustomException(ResponseMessage.WorkerDoesNotExist);
                }

                order.IsDone = orderUpdateRequestModel.IsDone;
                order.IsDoneDate = DateTime.Now;
                order.WorkerId = orderUpdateRequestModel.WorkerId;
            }

            //Update order
            order.DeliveryDate = orderUpdateRequestModel.DeliveryDate;
            order.DeliveryTime = orderUpdateRequestModel.DeliveryTime;
            order.IsDone = orderUpdateRequestModel.IsDone;
            order.Note = orderUpdateRequestModel.Note.Trim();
            order.Container = orderUpdateRequestModel.Container;
            order.CustomerId = customer.Id;
            order.Amount = orderUpdateRequestModel.Amount;
            order.TotalCost = await CountTotalCostAsync(orderUpdateRequestModel.Container, orderUpdateRequestModel.Amount, orderUpdateRequestModel.Accessories);

            var updatedOrder = _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.SaveChangesAsync();

            var orderResponseModel = await FindAsync(item => item.Id == updatedOrder.Id);

            //Sending notification to telegram
            if (orderUpdateRequestModel.IsDone == false && orderResponseModel.IsDone == true)
            {
                await _telegramOrderService.BulkSent(orderResponseModel);
            }

            return orderResponseModel;
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>().FindAsync(item => item.Id == orderId);

            if (order == null)
            {
                throw new CustomException(ResponseMessage.OrderDoesNotExist);
            }

            var result = await _unitOfWork.Repository<Order>().RemoveAsync(orderId);

            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        private async Task<List<OrderResponseModel>> GenerateOrderResponseModelList(List<Order> orders)
        {
            var orderResponseModelList = new List<OrderResponseModel>();

            foreach (var order in orders)
            {
                // Getting accessories for order
                var orderAccessories = await _unitOfWork.Repository<OrderAccessory>().GetAsync(item => item.OrderId == order.Id);

                var accessories = new List<Accessory>();

                foreach (var orderAccessory in orderAccessories)
                {
                    var accessory = await _unitOfWork.Repository<Accessory>().FindAsync(item => item.Id == orderAccessory.AccessoryId);

                    if (accessory != null)
                    {
                        accessories.Add(accessory);
                    }
                }

                var accessoryResponseModels = _mapper.Map<List<Accessory>, List<AccessoryResponseModel>>(accessories);

                //Gettingg customer for order
                var customerResponseModel = await _customerService.FindAsync(item => item.Id == order.CustomerId);

                //Generatin orderResponseModel with accessories and customer
                var orderResponseModel = _mapper.Map<Order, OrderResponseModel>(order);

                orderResponseModel.Note = order.Note.Trim();
                orderResponseModel.Accessories = accessoryResponseModels;
                orderResponseModel.Customer = customerResponseModel;

                orderResponseModelList.Add(orderResponseModel);
            }

            return orderResponseModelList;
        }

        private async Task<int> CountTotalCostAsync(string container, int bottlesAmount, List<int> accessories)
        {
            Service waterService;

            if(container == ConstantNames.RusNewBottle)
            {
                waterService = await _unitOfWork.Repository<Service>().FindAsync(item => item.Name.Trim().ToLower() == ServicesConstants.RusSetName);
            }
            else
            {
                waterService = await _unitOfWork.Repository<Service>().FindAsync(item => item.Name.Trim().ToLower() == ServicesConstants.RusWaterName);
            }

            if (waterService == null)
            {
                throw new CustomException(ResponseMessage.ServiceWaterDoesNotExist);
            }

            var totalCost = waterService.Price * bottlesAmount;

            foreach (var accessoryId in accessories)
            {
                var accessory = await _unitOfWork.Repository<Accessory>().FindAsync(item => item.Id == accessoryId);

                if (accessory != null)
                {
                    totalCost += accessory.Price;
                }
            }

            return totalCost;
        }
    }
}
