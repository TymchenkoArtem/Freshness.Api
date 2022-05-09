using AutoMapper;
using Freshness.Common.CustomExceptions;
using Freshness.Common.ResponseMessages;
using Freshness.DAL;
using Freshness.DAL.Interfaces;
using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using Freshness.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Services
{
    public class AddressService : IAddressService
    {
        private readonly FreshnessContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressService(FreshnessContext context, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AddressResponseModel> FindAsync(Expression<Func<Address, bool>> predicate)
        {
            var address = await _context.Addresses.Include(address => address.District).Include(address => address.Street).Where(predicate).FirstOrDefaultAsync();

            var addressResponseModel = new AddressResponseModel()
            {
                Id = address.Id,
                District = address.District.Name,
                Street = address.Street.Name,
                House = address.House,
                Flat = address?.Flat,
                Entrance = address.Entrance
            };

            return addressResponseModel;
        }

        public async Task<List<AddressResponseModel>> GetAsync(Expression<Func<Address, bool>> predicate)
        {
            var addresses = await _context.Addresses.Include(address => address.District).Include(address => address.Street).Where(predicate).ToListAsync();

            if (addresses == null)
            {
                return null;
            }

            var addressResponseModels = new List<AddressResponseModel>();

            addresses.ForEach(async address =>
            {
                await Task.Run(() => addressResponseModels.Add(new AddressResponseModel
                {
                    Id = address.Id,
                    District = address.District.Name,
                    Street = address.Street.Name,
                    House = address.House,
                    Flat = address?.Flat,
                    Entrance = address.Entrance
                }));
            });

            return addressResponseModels;
        }

        public async Task<PaginationResponseModel<AddressResponseModel>> GetAsync(int offset, int limit)
        {
            var addresses = await _context.Addresses.Include(address => address.District).Include(address => address.Street).Skip(offset).Take(limit).ToListAsync();

            var totalCount = _context.Addresses.Count();

            var addressResponseModels = new List<AddressResponseModel>();

            addresses.ForEach(async address =>
            {
                await Task.Run(() => addressResponseModels.Add(new AddressResponseModel
                {
                    Id = address.Id,
                    District = address.District.Name,
                    Street = address.Street.Name,
                    House = address.House,
                    Flat = address?.Flat,
                    Entrance = address.Entrance
                }));
            });

            var paginationResponseModel = new PaginationResponseModel<AddressResponseModel>
            {
                Entities = addressResponseModels,
                TotalCount = totalCount
            };

            return paginationResponseModel;
        }

        public async Task<Address> CreateAsync(AddressCreateRequestModel addressCreateRequestModel)
        {
            var address = await _unitOfWork.Repository<Address>().FindAsync(item => item.District.Name == addressCreateRequestModel.District &&
                item.Street.Name == addressCreateRequestModel.Street &&
                item.House == addressCreateRequestModel.House);

            if (address != null)
            {
                throw new CustomException(ResponseMessage.AddressAlreadyExists);
            }

            address = await CreateAddressModelAsync(addressCreateRequestModel);

            var insertedAddress = await _unitOfWork.Repository<Address>().InsertAsync(address);

            await _unitOfWork.SaveChangesAsync();

            return insertedAddress;
        }

        public async Task<Address> UpdateAsync(AddressUpdateRequestModel addressUpdateRequestModel)
        {
            var address = await _unitOfWork.Repository<Address>().FindAsync(item => item.Id != addressUpdateRequestModel.Id &&
                item.District.Name == addressUpdateRequestModel.District &&
                item.Street.Name == addressUpdateRequestModel.Street &&
                item.House == addressUpdateRequestModel.House);

            if (address != null)
            {
                throw new CustomException(ResponseMessage.AddressAlreadyExists);
            }

            address = await _unitOfWork.Repository<Address>().FindAsync(item => item.Id == addressUpdateRequestModel.Id);

            if (address == null)
            {
                throw new CustomException(ResponseMessage.AddressDoesNotExist);
            }

            UpdateAddressModel(addressUpdateRequestModel, address);

            var updatedAddress = _unitOfWork.Repository<Address>().Update(address);

            await _unitOfWork.SaveChangesAsync();

            return updatedAddress;
        }

        public async Task<bool> DeleteAsync(int addressId)
        {
            var address = await _unitOfWork.Repository<Address>().FindAsync(item => item.Id == addressId);

            if (address == null)
            {
                throw new CustomException(ResponseMessage.AddressDoesNotExist);
            }

            var result = await _unitOfWork.Repository<Address>().RemoveAsync(address.Id);

            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<Address> CreateAddressModelAsync(AddressCreateRequestModel addressCreateRequestModel)
        {
            var district = await _unitOfWork.Repository<District>().FindAsync(item => item.Name == addressCreateRequestModel.District);

            if (district == null)
            {
                district = new District
                {
                    Name = addressCreateRequestModel.District
                };
            }

            var street = await _unitOfWork.Repository<Street>().FindAsync(item => item.Name == addressCreateRequestModel.Street);

            if (street == null)
            {
                street = new Street
                {
                    Name = addressCreateRequestModel.Street
                };
            }

            var address = new Address
            {
                District = district,
                Street = street,
                House = addressCreateRequestModel.House,
                Flat = addressCreateRequestModel?.Flat,
                Entrance = addressCreateRequestModel.Entrance
            };

            return address;
        }

        private void UpdateAddressModel(AddressUpdateRequestModel source, Address destination)
        {
            destination.District = new District { Name = source.District };
            destination.Street = new Street { Name = source.Street };
            destination.House = source.House;
            destination.Flat = source?.Flat;
            destination.Entrance = source.Entrance;
        }
    }
}
