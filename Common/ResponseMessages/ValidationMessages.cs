namespace Freshness.Common.ResponseMessages
{
    public static class ValidationMessages
    {
        public const string IdRequired = "The id field is required";

        public const string IsDoneRequired = "The 'is done' field is required";

        public const string WorkerIdRequired = "The 'worker id' field is required";

        public const string IsDoneDateRequired = "The 'is done date' field is required";

        public const string DeliveryDateRequired = "The 'delivery date' field is required";

        public const string ImageRequired = "Image field is required";

        public const string EmailRequired = "Email field is required";
        public const string InvalidEmail = "Invalid email";
        public const string EmailNotFound = "Such email was not previously registered in the app";

        public const string PasswordRequired = "The password field is required";
        public const string InvalidPassword = "Password length must be in range from 6 to 20";

        public const string NewPasswordRequired = "New password field is required";
        public const string InvalidNewPassword = "New password must be from 6 to 20 symbols";

        public const string ConfirmPasswordRequired = "Confirm new password field is required";
        public const string ConfirmPasswordDoesNotMatch = "Confirm new password field doesn’t match password";

        public const string InvalidLimit = "Limit is required and must be in range from 1 to 1000";
        public const string InvalidOffset = "Offset must be in range fro 0 to 1000";

        public const string InvalidImageWeight = "Image is too large. It must be less than 5 MB.";
        public const string InvalidImageSize = "Image size error. It must be bigger than 400 x 500 and less than 5000 x 5000.";
        public const string InvalidImageExtension = "Image extension error. There are only 3 extensions allowed: .jpg, .png. .jpeg.";

        public const string InvalidName = "Name must be from 1 to 50 symbols";

        public const string InvalidPhone = "Phone number is required and must be 10 digits only";

        public const string InvalidRole = "Role is required and must from 1 to 50 symbols";

        public const string InvalidImage = "Image must be from 5 to 500 symbols";

        public const string InvalidDescriptoin = "Description must be from 5 to 500 symbols";

        public const string InvalidPrice = "Price must be from 0 to 10000";

        public const string InvalidAmount = "Amount must be from 0 to 1000";

        public const string InvalidLanguage = "Language must be from 0 to 2";

        public const string InvalidDistrict = "District field must be from 1 to 50 symbols";

        public const string InvalidStreet = "Street field must be from 1 to 50 symbols";

        public const string InvalidHouse = "House field must be from 1 to 50 symbols";

        public const string InvalidFlat = "Flat field must be from 0 to 50 symbols";

        public const string InvalidEntrance = "Entrance field must be from 0 to 20";

        public const string InvalidDeliveryTime = "Delivery time field must be from 1 to 50 symbols";

        public const string InvalidNote = "Note field must be from 5 to 500 symbols";
    }
}
