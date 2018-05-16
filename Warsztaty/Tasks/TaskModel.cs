﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks
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
                EndDate = endDate;
                IsAllDayTask = true;
            }

            IsImportant = isImportant;
        }
    }
}

/*
    Opis - Wymagany.
    Datę Rozpoczęcia - Wymagana.
    Datę Zakończenia - Niewymagana, jeśli zadanie jest całodniowe.
    Flaga Zadanie Całodniowe - Niewymagana, domyślnie zadanie nie jest całodniowe.
    Flagę Zadanie Ważne - Niewymagana, domyślnie zadanie nie jest ważne.
 */
