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
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Services
{
    public class AccessoryService : IAccessoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageProcessor _imageProcessor;

        public AccessoryService(IUnitOfWork unitOfWork, IMapper mapper, IImageProcessor imageProcessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageProcessor = imageProcessor;
        }

        public async Task<AccessoryResponseModel> FindAsync(Expression<Func<Accessory, bool>> predicate)
        {
            var accessory = await _unitOfWork.Repository<Accessory>().FindAsync(predicate);

            if (accessory == null)
            {
                throw new CustomException(ResponseMessage.AccessoryDoesNotExist);
            }

            var accessoryResponseModel = _mapper.Map<Accessory, AccessoryResponseModel>(accessory);

            return accessoryResponseModel;
        }

        public async Task<List<AccessoryResponseModel>> GetAsync(Expression<Func<Accessory, bool>> predicate)
        {
            var accessories = await _unitOfWork.Repository<Accessory>().GetAsync(predicate);

            var accessoryResponseModels = _mapper.Map<List<Accessory>, List<AccessoryResponseModel>>(accessories);

            return accessoryResponseModels;
        }

        public async Task<PaginationResponseModel<AccessoryResponseModel>> GetAsync(int offset, int limit)
        {
            var accessories = await _unitOfWork.Repository<Accessory>().GetAsync(offset, limit);

            var accessoryResponseModels = _mapper.Map<List<Accessory>, List<AccessoryResponseModel>>(accessories.Item1);

            var paginationResponseModel = new PaginationResponseModel<AccessoryResponseModel>
            {
                Entities = accessoryResponseModels,
                TotalCount = accessories.Item2
            };

            return paginationResponseModel;
        }

        public async Task<AccessoryResponseModel> CreateAsync(AccessoryCreateRequestModel accessoryCreateRequestModel)
        {
            var accessory = await _unitOfWork.Repository<Accessory>().FindAsync(item => item.Name.Trim().ToLower() == accessoryCreateRequestModel.Name.Trim().ToLower() &&
                                                     item.Language == accessoryCreateRequestModel.Language);

            if (accessory != null)
            {
                throw new CustomException(ResponseMessage.AccessoryAlreadyExists);
            }

            var uploadedImage = _imageProcessor.Upload(accessoryCreateRequestModel.Image);

            accessory = _mapper.Map<AccessoryCreateRequestModel, Accessory>(accessoryCreateRequestModel);

            accessory.Name = accessory.Name.Trim();
            accessory.Description = accessory.Description.Trim();
            accessory.OriginalImage = uploadedImage.OriginalImage;
            accessory.CroppedImage = uploadedImage.OriginalImage;

            var createdAccessory = await _unitOfWork.Repository<Accessory>().InsertAsync(accessory);

            await _unitOfWork.SaveChangesAsync();

            var accessoryResponseModel = _mapper.Map<Accessory, AccessoryResponseModel>(createdAccessory);

            return accessoryResponseModel;
        }

        public async Task<AccessoryResponseModel> UpdateAsync(AccessoryUpdateRequestModel accessoryUpdateRequestModel)
        {
            var accessory = await _unitOfWork.Repository<Accessory>().FindAsync(item => item.Id != accessoryUpdateRequestModel.Id &&
                item.Name.Trim().ToLower() == accessoryUpdateRequestModel.Name.Trim().ToLower() &&
                item.Language == accessoryUpdateRequestModel.Language);

            if (accessory != null)
            {
                throw new CustomException(ResponseMessage.AccessoryAlreadyExists);
            }

            accessory = await _unitOfWork.Repository<Accessory>().FindAsync(item => item.Id == accessoryUpdateRequestModel.Id);

            if (accessory == null)
            {
                throw new CustomException(ResponseMessage.AccessoryDoesNotExist);
            }

            // Remove unused image from database
            if (File.Exists(accessory.OriginalImage))
            {
                File.Delete(accessory.OriginalImage);
            }

            if (File.Exists(accessory.CroppedImage))
            {
                File.Delete(accessory.CroppedImage);
            }

            var uploadedImage = _imageProcessor.Upload(accessoryUpdateRequestModel.Image);

            accessory.Name = accessoryUpdateRequestModel.Name.Trim();
            accessory.OriginalImage = uploadedImage.OriginalImage;
            accessory.CroppedImage = uploadedImage.OriginalImage;
            accessory.Description = accessoryUpdateRequestModel.Description.Trim();
            accessory.Price = accessoryUpdateRequestModel.Price;
            accessory.Language = accessoryUpdateRequestModel.Language;

            var updatedAccessory = _unitOfWork.Repository<Accessory>().Update(accessory);

            await _unitOfWork.SaveChangesAsync();

            var accessoryResponseModel = _mapper.Map<Accessory, AccessoryResponseModel>(updatedAccessory);

            return accessoryResponseModel;
        }

        public async Task<bool> DeleteAsync(int accessoryId)
        {
            var accessory = await _unitOfWork.Repository<Accessory>().FindAsync(item => item.Id == accessoryId);

            if (accessory == null)
            {
                throw new CustomException(ResponseMessage.AccessoryDoesNotExist);
            }

            var result = await _unitOfWork.Repository<Accessory>().RemoveAsync(accessoryId);

            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
