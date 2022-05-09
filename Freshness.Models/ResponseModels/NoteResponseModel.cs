using System;

namespace Freshness.Models.ResponseModels
{
    public class NoteResponseModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Text { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
