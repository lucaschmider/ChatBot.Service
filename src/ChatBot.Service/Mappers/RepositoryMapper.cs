﻿using System.Linq;
using ChatBot.Repository.Contracts.Models;
using ChatBot.Service.Models;
using ChatBot.StatisticsProvider.Contract.Models;

namespace ChatBot.Service.Mappers
{
    public static class RepositoryMapper
    {
        public static StatisticsResponse Map(this DepartmentReport departmentReport)
        {
            return new StatisticsResponse
            {
                Department = departmentReport.Department,
                Ratings = departmentReport.RatingSlices.Select(slice => slice.Map())
            };
        }

        public static StatisticsResponse.RatingSlice Map(this RatingSlice slice)
        {
            return new StatisticsResponse.RatingSlice
            {
                Rating = slice.Value,
                Time = slice.Timestamp
            };
        }

        public static DepartmentResponse Map(this Department department)
        {
            return new DepartmentResponse
            {
                DepartmentName = department.DepartmentName,
                Id = department.DepartmentId
            };
        }
    }
}