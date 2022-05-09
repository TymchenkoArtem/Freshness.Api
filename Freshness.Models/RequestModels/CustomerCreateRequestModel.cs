using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class CustomerCreateRequestModel
    {
        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidName)]
        public string Name { get; set; }

        [Required(ErrorMessage = ValidationMessages.InvalidPhone)]
        [RegularExpression(ValidationValues.Phone, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string Phone { get; set; }

        public AddressCreateRequestModel Address { get; set; }
    }
}
