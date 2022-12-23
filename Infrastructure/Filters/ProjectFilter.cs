using System;

namespace Infrastructure.Filters
{
    public class ProjectFilter
    {
        public string Name { get; set; }

        public byte? StatusId { get; set; }

        public int? Priority { get; set; }

        public RangeDate StartDateRange { get; set; }

        public RangeDate CompletionDateRange { get; set; }

        public bool OrderByStartDate { get; set; }

        public bool OrderByPriority { get; set; }
    }

    public class RangeDate
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

    }
}
