using Freshness.Common.ResponseMessages;
using Freshness.Common.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Freshness.Models.RequestModels
{
    public class WorkerUpdateRequestModel
    {
        [Required(ErrorMessage = ValidationMessages.IdRequired)]
        public int Id { get; set; }

        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidName)]
        public string Name { get; set; }

        [Required(ErrorMessage = ValidationMessages.InvalidPhone)]
        [RegularExpression(ValidationValues.Phone, ErrorMessage = ValidationMessages.InvalidPhone)]
        public string Phone { get; set; }

        [Required(ErrorMessage = ValidationMessages.InvalidEmail)]
        [RegularExpression(ValidationValues.Email, ErrorMessage = ValidationMessages.InvalidEmail)]
        public string Email { get; set; }

        [StringLength(ValidationValues.PasswordMaxLength, MinimumLength = ValidationValues.PasswordMinLength, ErrorMessage = ValidationMessages.InvalidPassword)]
        public string Password { get; set; }

        [StringLength(ValidationValues.StringMaxLength, MinimumLength = ValidationValues.StringMinLength, ErrorMessage = ValidationMessages.InvalidRole)]
        public string Role { get; set; }
    }
}
