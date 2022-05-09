using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class ResetPasswordRequestModel
    {
        [Required(ErrorMessage = ValidationMessages.InvalidPhone)]
        [RegularExpression(ValidationValues.Phone, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string Phone { get; set; }
    }
}
