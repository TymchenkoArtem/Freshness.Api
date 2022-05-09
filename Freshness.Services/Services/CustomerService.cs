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
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAddressService _addressService;

        public CustomerService(IMapper mapper, IUnitOfWork unitOfWork, IAddressService addressService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _addressService = addressService;
        }

        public async Task<CustomerResponseModel> FindAsync(Expression<Func<Customer, bool>> predicate)
        {
            var customer = await _unitOfWork.Repository<Customer>().FindAsync(predicate);

            if(customer == null)
            {
                return null;
            }

            var notes = await GenerateNoteResponseModelsAsync(customer.Id);

            var address = await _addressService.FindAsync(item => item.Id == customer.AddressId);

            var customerResponseModel = _mapper.Map<Customer, CustomerResponseModel>(customer);

            customerResponseModel.Address = address;
            customerResponseModel.Notes = notes;

            return customerResponseModel;
        }

        public async Task<List<CustomerResponseModel>> GetAsync(Expression<Func<Customer, bool>> predicate)
        {
            var customers = await _unitOfWork.Repository<Customer>().GetAsync(predicate);

            var customerResponseModels = await GenerateCustomerResponseModels(customers);

            return customerResponseModels;
        }

        public async Task<PaginationResponseModel<CustomerResponseModel>> GetAsync(int offset, int limit)
        {
            var customers = await _unitOfWork.Repository<Customer>().GetAsync(offset, limit);

            var entities = await GenerateCustomerResponseModels(customers.Item1);

            var paginationResponseModel = new PaginationResponseModel<CustomerResponseModel>
            {
                Entities = entities,
                TotalCount = customers.Item2
            };

            return paginationResponseModel;
        }

        public async Task<CustomerResponseModel> CreateAsync(CustomerCreateRequestModel customerCreateRequestModel)
        {
            var customer = await _unitOfWork.Repository<Customer>().FindAsync(item => item.Phone == customerCreateRequestModel.Phone &&
                item.Address.District.Name == customerCreateRequestModel.Address.District &&
                item.Address.Street.Name == customerCreateRequestModel.Address.Street &&
                item.Address.House == customerCreateRequestModel.Address.House);

            if (customer != null)
            {
                throw new CustomException(ResponseMessage.CustomerAlreadyExists);
            }

            var address = await _unitOfWork.Repository<Address>().FindAsync(item => item.District.Name == customerCreateRequestModel.Address.District &&
                item.Street.Name == customerCreateRequestModel.Address.Street &&
                item.House == customerCreateRequestModel.Address.House);

            if (address == null)
            {
                address = await _addressService.CreateAsync(customerCreateRequestModel.Address);
            }

            customer = new Customer
            {
                Phone = customerCreateRequestModel.Phone,
                Name = customerCreateRequestModel.Name.Trim(),
                Address = address,
                AddedDate = DateTime.Now
            };

            var insertedCustomer = await _unitOfWork.Repository<Customer>().InsertAsync(customer);

            await _unitOfWork.SaveChangesAsync();

            var customerResponseModel = await FindAsync(item => item.Id == insertedCustomer.Id);

            return customerResponseModel;
        }

        public async Task<CustomerResponseModel> UpdateAsync(CustomerUpdateRequestModel customerUpdateRequestModel)
        {
            var customer = await _unitOfWork.Repository<Customer>().FindAsync(item => item.Id != customerUpdateRequestModel.Id &&
                item.Phone == customerUpdateRequestModel.Phone &&
                item.Address.District.Name == customerUpdateRequestModel.Address.District &&
                item.Address.Street.Name == customerUpdateRequestModel.Address.Street &&
                item.Address.House == customerUpdateRequestModel.Address.House);

            if (customer != null)
            {
                throw new CustomException(ResponseMessage.CustomerAlreadyExists);
            }

            var customerModel = await _unitOfWork.Repository<Customer>().FindAsync(item => item.Id == customerUpdateRequestModel.Id);

            if (customerModel == null)
            {
                throw new CustomException(ResponseMessage.CustomerDoesNotExist);
            }

            var address = await _unitOfWork.Repository<Address>().FindAsync(item => item.District.Name == customerUpdateRequestModel.Address.District &&
                item.Street.Name == customerUpdateRequestModel.Address.Street &&
                item.House == customerUpdateRequestModel.Address.House);

            if (address == null)
            {
                var addressCreateRequestModel = _mapper.Map<AddressUpdateRequestModel, AddressCreateRequestModel>(customerUpdateRequestModel.Address);

                address = await _addressService.CreateAsync(addressCreateRequestModel);
            }
            else
            {
                address = await _addressService.UpdateAsync(customerUpdateRequestModel.Address);
            }

            customerModel.Name = customerUpdateRequestModel.Name.Trim();
            customerModel.Phone = customerUpdateRequestModel.Phone;
            customerModel.Address = address;

            var updatedCustomer = _unitOfWork.Repository<Customer>().Update(customerModel);

            await _unitOfWork.SaveChangesAsync();

            var customerResponseModel = await FindAsync(item => item.Id == customerUpdateRequestModel.Id);

            return customerResponseModel;
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            var customer = await _unitOfWork.Repository<Customer>().FindAsync(item => item.Id == customerId);

            if (customer == null)
            {
                throw new CustomException(ResponseMessage.CustomerDoesNotExist);
            }

            var result = await _unitOfWork.Repository<Customer>().RemoveAsync(customer.Id);

            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        private async Task<List<CustomerResponseModel>> GenerateCustomerResponseModels(List<Customer> customers)
        {
            if (customers == null)
            {
                return null;
            }

            var customerResponseModels = new List<CustomerResponseModel>();

            foreach (var customer in customers)
            {
                var notes = await GenerateNoteResponseModelsAsync(customer.Id);

                var address = await _addressService.FindAsync(item => item.Id == customer.AddressId);

                var customerResponseModel = _mapper.Map<Customer, CustomerResponseModel>(customer);

                customerResponseModel.Address = address;
                customerResponseModel.Notes = notes;

                customerResponseModels.Add(customerResponseModel);
            }

            return customerResponseModels;
        }

        private async Task<List<NoteResponseModel>> GenerateNoteResponseModelsAsync(int customerId)
        {
            var notes = await _unitOfWork.Repository<Note>().GetAsync(item => item.CustomerId == customerId);

            var noteResponseModels = _mapper.Map<List<Note>, List<NoteResponseModel>>(notes);

            return noteResponseModels;
        }
    }
}
