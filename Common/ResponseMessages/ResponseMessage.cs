namespace Freshness.Common.ResponseMessages
{
    public static class ResponseMessage
    {
        public const string GeneralError = "Error occured";

        public const string CallDoesNotExist = "Call does not exist";
        public const string CallAlreadyExists = "Call already exists";
        public const string IncorrectCallData = "Incorrect call data";

        public const string CustomerDoesNotExist = "Customer does not exist";
        public const string CustomerAlreadyExists = "Customer already exists";

        public const string AddressDoesNotExist = "Address does not exist";
        public const string AddressAlreadyExists = "Address already exists";

        public const string AccessoryDoesNotExist = "Accessory does not exist";
        public const string AccessoryAlreadyExists = "Accessory already exists";
        public const string AccessoryWithNameAlreadyExists = "Accessory with this name and language already exists";

        public const string ServiceDoesNotExist = "Service does not exist";
        public const string ServiceWaterDoesNotExist = "Service 'water' does not exist";
        public const string ServiceAlreadyExists = "Service already exists";

        public const string UserDoesNotExist = "User does not exist";
        public const string UserAlreadyExists = "User already exists";

        public const string OrderDoesNotExist = "Order does not exist";
        public const string OrderAlreadyExists = "Order already exists";

        public const string NoteDoesNotExist = "Note does not exist";

        public const string TokenIsNotValid = "Token is not valid";
        public const string TokenDoesNotExist = "Token does not exist";
        public const string TokenAlreadyExists = "Token already exists";

        public const string WorkerDoesNotExist = "Worker does not exist";
        public const string WorkerAlreadyExists = "Worker already exists";
    }
}
