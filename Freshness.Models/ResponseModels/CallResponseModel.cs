using System;

namespace Freshness.Models.ResponseModels
{
    public class CallResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public DateTime AddedDate { get; set; }

        public bool IsDone { get; set; }

        public DateTime IsDoneDate { get; set; }

        public WorkerResponseModel Worker { get; set; }
    }
}
