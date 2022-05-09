using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class CustomerUpdateRequestModel
    {
        [Required(ErrorMessage = ValidationMessages.IdRequired)]
        public int Id { get; set; }

        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidName)]
        public string Name { get; set; }

        [Required(ErrorMessage = ValidationMessages.InvalidPhone)]
        [RegularExpression(ValidationValues.Phone, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string Phone { get; set; }

        public AddressUpdateRequestModel Address { get; set; }
    }
}
