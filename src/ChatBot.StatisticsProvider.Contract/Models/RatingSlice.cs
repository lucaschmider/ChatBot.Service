using System;

namespace ChatBot.StatisticsProvider.Contract.Models
{
    /// <summary>
    ///     Represents a key value pair of a point in time and the corresponding feedback
    /// </summary>
    public class RatingSlice
    {
        /// <summary>
        ///     The time at which the value was recent
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        ///     The mean of all ratings send in the corresponding time slice
        /// </summary>
        public float Value { get; set; }
    }
}