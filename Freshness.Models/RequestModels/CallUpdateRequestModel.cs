using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class CallUpdateRequestModel
    {
        [Required(ErrorMessage = ValidationMessages.IdRequired)]
        public int Id { get; set; }

        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidName)]
        public string Name { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsDoneRequired)]
        public bool IsDone { get; set; }

        public int? WorkerId { get; set; }
    }
}
