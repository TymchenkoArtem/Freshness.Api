namespace Freshness.Common.Validation
{
    public static class ValidationValues
    {
        public const string Email = "([a-zA-Z0-9]+([+=#-._][a-zA-Z0-9]+)*@([a-zA-Z0-9]+(-[a-zA-Z0-9]+)*)+(([.][a-zA-Z0-9]{2,4})+)+)";

        public const string Phone = @"^\d{10}$";

        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 20;

        public const int StringMinLength = 1;
        public const int StringMaxLength = 50;

        public const int FlatMinLength = 0;
        public const int FlatMaxLength = 50;

        public const int DescriptionMinLength = 5;
        public const int DescriptionMaxLength = 500;

        public const int PriceMinValue = 0;
        public const int PriceMaxValue = 10000;

        public const int AmountMinValue = 0;
        public const int AmountMaxValue = 1000;

        public const int LanguageMinValue = 0;
        public const int LanguageMaxValue = 2;

        public const int ImageMinLength = 1;
        public const int ImageMaxLength = 500;

        public const int LimitMinValue = 1;
        public const int LimitMaxValue = 1000;

        public const int OffsetMinValue = 0;
        public const int OffsetMaxValue = 1000;

        public const int EntranceMinValue = 0;
        public const int EntranceMaxValue = 20;
    }
}
