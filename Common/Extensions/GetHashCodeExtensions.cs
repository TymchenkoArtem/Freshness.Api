using System.Text;

namespace Freshness.Common.Extensions
{
    public static class GetHashCodeExtensions
    {
        public static string GetCustomHash(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            byte[] data = Encoding.ASCII.GetBytes(inputString);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            var hash = Encoding.ASCII.GetString(data);
            return hash;
        }
    }
}
