using System;
using System.Collections.Generic;

namespace ChatBot.Service.Models
{
    public class StatisticsResponse
    {
        public string Department { get; set; }
        public IEnumerable<RatingSlice> Ratings { get; set; }

        public class RatingSlice
        {
            public DateTime Time { get; set; }
            public float Rating { get; set; }
        }
    }
}