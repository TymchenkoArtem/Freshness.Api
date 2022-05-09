using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class PaginationRequestModel
    {
        [Required(ErrorMessage = ValidationMessages.InvalidLimit)]
        [Range(ValidationValues.LimitMinValue, ValidationValues.LimitMaxValue, ErrorMessage = ValidationMessages.InvalidLimit)]
        public int Limit { get; set; }

        [Range(ValidationValues.OffsetMinValue, ValidationValues.OffsetMaxValue, ErrorMessage = ValidationMessages.InvalidOffset)]
        public int Offset { get; set; }
    }
}
