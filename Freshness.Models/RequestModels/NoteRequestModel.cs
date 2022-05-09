using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class NoteRequestModel
    {
        [Required(ErrorMessage = ValidationMessages.IdRequired)]
        public int CustomerId { get; set; }

        [StringLength(ValidationValues.DescriptionMaxLength, MinimumLength = ValidationValues.DescriptionMinLength, ErrorMessage = ValidationMessages.InvalidNote)]
        public string Text { get; set; }
    }
}
