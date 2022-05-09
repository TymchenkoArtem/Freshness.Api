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
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NoteResponseModel> FindAsync(Expression<Func<Note, bool>> predicate)
        {
            var note = await _unitOfWork.Repository<Note>().FindAsync(predicate);

            var noteResponseModel = _mapper.Map<Note, NoteResponseModel>(note);

            return noteResponseModel;
        }

        public async Task<List<NoteResponseModel>> GetAsync(Expression<Func<Note, bool>> predicate)
        {
            var notes = await _unitOfWork.Repository<Note>().GetAsync(predicate);

            var noteResponseModels = _mapper.Map<List<Note>, List<NoteResponseModel>>(notes);

            return noteResponseModels;
        }

        public async Task<NoteResponseModel> CreateAsync(NoteRequestModel noteRequestModel)
        {
            var customer = await _unitOfWork.Repository<Customer>().FindAsync(item => item.Id == noteRequestModel.CustomerId);

            if (customer == null)
            {
                throw new CustomException(ResponseMessage.CustomerDoesNotExist);
            }

            var note = new Note
            {
                Text = noteRequestModel.Text.Trim(),
                CustomerId = noteRequestModel.CustomerId,
                AddedDate = DateTime.Now
            };

            await _unitOfWork.Repository<Note>().InsertAsync(note);

            await _unitOfWork.SaveChangesAsync();

            var noteResponseModel = _mapper.Map<Note, NoteResponseModel>(note);

            return noteResponseModel;

        }

        public async Task<bool> DeleteAsync(int noteId)
        {
            var note = await _unitOfWork.Repository<Note>().FindAsync(item => item.Id == noteId);

            if (note == null)
            {
                throw new CustomException(ResponseMessage.NoteDoesNotExist);
            }

            var result = await _unitOfWork.Repository<Note>().RemoveAsync(noteId);

            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}
