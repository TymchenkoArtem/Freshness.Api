using System;
using System.Collections.Generic;

namespace Freshness.Models.ResponseModels
{
    public class CustomerResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public AddressResponseModel Address { get; set; }

        public List<NoteResponseModel> Notes { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
