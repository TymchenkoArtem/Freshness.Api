using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = ValidationMessages.InvalidPhone)]
        [RegularExpression(ValidationValues.Phone, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string Phone { get; set; }

        [Required(ErrorMessage = ValidationMessages.InvalidPassword)]
        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidPassword)]
        public string Password { get; set; }
    }
}
