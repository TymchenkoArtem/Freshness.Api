using System.Collections.Generic;

namespace Freshness.Common.Enums
{
    public static class Streets
    {
        static Streets()
        {
            _streets = new List<string>() {
                ""
            };
        }

        public static List<string> Get() { return _streets; }

        public static void Set(string street) { _streets.Add(street); }

        public static void Remove(string street) { _streets.Remove(street); }

        private static List<string> _streets;
    }
}
