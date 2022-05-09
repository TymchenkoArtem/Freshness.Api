using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Freshness.Common.EcxeptionModel
{
    public class ExceptionResponseModel
    {
        public ExceptionResponseModel()
        {
        }

        public ExceptionResponseModel(ModelStateDictionary modelState)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;

            ExceptionDetails = new List<ExceptionDetail>();

            modelState.Select(error =>
            {
                error.Value.Errors.Select(errorDetail =>
                {
                    ExceptionDetails.Add(new ExceptionDetail { Key = error.Key, Message = errorDetail.ErrorMessage });

                    return errorDetail;

                }).ToList();

                return error;

            }).ToList();
        }

        public int StatusCode { get; set; }

        public string StackTrace { get; set; }

        public List<ExceptionDetail> ExceptionDetails { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
