namespace Freshness.Common.Constants
{
    public static class Role
    {
        public const string Admin = "admin";

        public const string Dispatcher = "dispatcher";

        public const string Worker = "worker";

        public static bool Find(string role)
        {
            if (role == Admin || role == Dispatcher || role == Worker)
            {
                return true;
            }

            return false;
        }
    }
}
