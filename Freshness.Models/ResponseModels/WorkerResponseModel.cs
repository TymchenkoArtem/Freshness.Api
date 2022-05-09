﻿using System;

namespace Freshness.Models.ResponseModels
{
    public class WorkerResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
