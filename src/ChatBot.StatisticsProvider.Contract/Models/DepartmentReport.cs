using System.Collections.Generic;

namespace ChatBot.StatisticsProvider.Contract.Models
{
    /// <summary>
    ///     Represents all available feedback for a certain department
    /// </summary>
    public class DepartmentReport
    {
        /// <summary>
        ///     The department represented
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        ///     Feedback for the department grouped into slices of equal time slots
        /// </summary>
        public IEnumerable<RatingSlice> RatingSlices { get; set; }
    }
}