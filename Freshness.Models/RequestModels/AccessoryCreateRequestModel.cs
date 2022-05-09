using Freshness.Common.Enums;
using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class AccessoryCreateRequestModel
    {
        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidName)]
        public string Name { get; set; }

        [Required(ErrorMessage = ValidationMessages.ImageRequired)]
        public IFormFile Image { get; set; }

        [StringLength(ValidationValues.DescriptionMaxLength, MinimumLength = ValidationValues.DescriptionMinLength, ErrorMessage = ValidationMessages.InvalidDescriptoin)]
        public string Description { get; set; }

        [Required(ErrorMessage = ValidationMessages.InvalidPrice)]
        [Range(ValidationValues.PriceMinValue, ValidationValues.PriceMaxValue, ErrorMessage = ValidationMessages.InvalidPrice)]
        public int Price { get; set; }

        [Required(ErrorMessage = ValidationMessages.InvalidLanguage)]
        [Range(ValidationValues.LanguageMinValue, ValidationValues.LanguageMaxValue, ErrorMessage = ValidationMessages.InvalidLanguage)]
        public Language Language { get; set; }
    }
}
