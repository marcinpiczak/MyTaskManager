using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks_2
{
    public class TaskModel
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAllDayTask { get; set; }
        public bool IsImportant { get; set; }

        public TaskModel(string description, DateTime startDate, DateTime? endDate, bool isImportant = false)
        {
            Description = description;
            StartDate = startDate;
            if (endDate.HasValue)
            {
                if (endDate < startDate)
                {
                    EndDate = null;
                    IsAllDayTask = true;
                }
                else
                {
                    EndDate = endDate;
                    IsAllDayTask = false;
                }
            }
            else
            {
                EndDate = null;
                IsAllDayTask = true;
            }

            IsImportant = isImportant;
        }
    }
}
