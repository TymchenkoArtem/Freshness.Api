using System.Collections.Generic;

namespace Freshness.Common.Enums
{
    public class Districts
    {
        static Districts()
        {
            _districts = new List<string>() {
                ""
            };
        }

        public static List<string> Get() { return _districts; }

        public static void Set(string street) { _districts.Add(street); }

        public static void Remove(string street) { _districts.Remove(street); }

        private static List<string> _districts;
    }
}
