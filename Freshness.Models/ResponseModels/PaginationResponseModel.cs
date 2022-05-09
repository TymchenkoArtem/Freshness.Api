using System.Collections.Generic;

namespace Freshness.Models.ResponseModels
{
    public class PaginationResponseModel<T> where T : class
    {
        public List<T> Entities { get; set; }

        public int TotalCount { get; set; }
    }
}
