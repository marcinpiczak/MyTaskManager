using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks
{
    public class TaskModel
    {
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public bool IsAllDayTask { get; private set; }
        public bool IsImportant { get; private set; }

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

/*
    Opis - Wymagany.
    Datę Rozpoczęcia - Wymagana.
    Datę Zakończenia - Niewymagana, jeśli zadanie jest całodniowe.
    Flaga Zadanie Całodniowe - Niewymagana, domyślnie zadanie nie jest całodniowe.
    Flagę Zadanie Ważne - Niewymagana, domyślnie zadanie nie jest ważne.
 */
