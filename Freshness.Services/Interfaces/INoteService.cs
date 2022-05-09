using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Freshness.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteResponseModel> FindAsync(Expression<Func<Note, bool>> predicate);

        Task<List<NoteResponseModel>> GetAsync(Expression<Func<Note, bool>> predicate);

        Task<NoteResponseModel> CreateAsync(NoteRequestModel noteRequestModel);

        Task<bool> DeleteAsync(int noteId);
    }
}
