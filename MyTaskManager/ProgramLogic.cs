using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace Tasks_2
{
    public class ProgramLogic
    {
        private List<TaskModel> _taskList = new List<TaskModel>();

        public const string DefultPath = @"..\..\..\data.csv";

        public int ListCount
        {
            get { return _taskList.Count; }
        }

        public void AddTask(string taskDescription, DateTime startDate, DateTime? endDate, bool isImportant)
        {
            _taskList.Add(new TaskModel(taskDescription, startDate, endDate, isImportant));
        }

        public void RemoveTask(int taskIndex)
        {
            if (ListCount == 0 || _taskList == null)
            {
                ConsoleEx.WriteLine(ConsoleColor.Red, "Brak zadań na lisćie.");
            }
            else if (taskIndex < 1 || taskIndex > ListCount)
            {
                ConsoleEx.WriteLine(ConsoleColor.Red, $"Podany indeks zadania jest niepoprawny. Index powinien zawierać się w zakresie: od 1 do {ListCount}. ");
            }
            else
            {
                _taskList.RemoveAt(taskIndex - 1);
            }
 
        }

        public void ShowTask(Func<TaskModel, bool> whereFilter = null)
        {
            if (whereFilter == null)
            {
                whereFilter = task => true;
            }

            var filteredTaskList = _taskList;

            foreach (Func<TaskModel, bool> where in whereFilter.GetInvocationList())
            {
                filteredTaskList = filteredTaskList.Where(where).ToList();
            }

            //var filteredTaskList = _taskList.Where(whereFilter).ToList();
            
            if (filteredTaskList.Count == 0 || filteredTaskList == null)
            {
                ConsoleEx.WriteLine(ConsoleColor.Red, "Brak zadań do wyświetlenia.");
            }
            else
            {
                var importantTaskList = filteredTaskList.Where(x => x.IsImportant).OrderBy(x => x.StartDate);

                //ConsoleEx.WriteLine(ConsoleColor.Green, "Znaleziono nastepujące zadania: ");

                ShowTaskDisplayHeader(ConsoleColor.Gray);

                foreach (var task in importantTaskList)
                {
                    ShowTaskDisplayLines(task, ConsoleColor.DarkYellow);
                }

                //var sortedtTaskList = _taskList.Where(x => !x.IsImportant).OrderByDescending(x => x.StartDate);
                var sortedtTaskList = filteredTaskList.Except(importantTaskList).OrderBy(x => x.StartDate);

                foreach (var task in sortedtTaskList)
                {
                    ShowTaskDisplayLines(task, ConsoleColor.Gray);
                }
                Console.WriteLine();
            }

        }

        private void ShowTaskDisplayHeader(ConsoleColor color)
        {
            string h1 = string.Format("| {0} | {1} | {2} | {3} | {4} | {5} |",
                "ID".PadLeft(5),
                "Opis".PadLeft(20),
                "Data".PadLeft(11),
                "Data".PadLeft(11),
                "Cało-".PadLeft(6),
                "Ważne?".PadLeft(6)
            );
            string h2 = string.Format("| {0} | {1} | {2} | {3} | {4} | {5} |",
                "".PadLeft(5),
                "".PadLeft(20),
                "rozpoczęcia".PadLeft(11),
                "zakończenia".PadLeft(11),
                "dniowe".PadLeft(6),
                "".PadLeft(6)
            );

            ConsoleEx.WriteLine(color, "".PadLeft(h2.Length, '-'));
            ConsoleEx.WriteLine(color, h1);
            ConsoleEx.WriteLine(color, h2);
            ConsoleEx.WriteLine(color, "".PadLeft(h2.Length, '-'));
        }

        private void ShowTaskDisplayLines(TaskModel task, ConsoleColor color)
        {
            string l = string.Format("| {0} | {1} | {2} | {3} | {4} | {5} |",
                (_taskList.IndexOf(task) == -1 ? -1 : _taskList.IndexOf(task) + 1).ToString().PadLeft(5),
                task.Description.Length > 20 ? task.Description.Trim().Substring(0, 17) + "..." : task.Description.PadLeft(20),
                task.StartDate.ToString("yyyy-MM-dd").PadLeft(11),
                task.EndDate == null ? "".PadLeft(11) : task.EndDate.Value.ToString("yyyy-MM-dd").PadLeft(11),
                task.IsAllDayTask ? "T".PadLeft(6) : "N".PadLeft(6),
                task.IsImportant ? "T".PadLeft(6) : "N".PadLeft(6)
                );
            ConsoleEx.WriteLine(color, l);
            ConsoleEx.WriteLine(color, "".PadLeft(l.Length, '-'));
        }

        public void SaveTasks(string path = DefultPath)
        {
            if (ListCount == 0 || _taskList == null)
            {
                ConsoleEx.WriteLine(ConsoleColor.Red, "Brak zadań na lisćie.");
            }
            else
            {
                int count = 0;
                using (var writer = new StreamWriter(Path.GetFullPath(path), false))
                {
                    foreach (var task in _taskList)
                    {
                        writer.WriteLine("{0},{1},{2},{3},{4}",
                            task.Description,
                            task.StartDate.ToString("yyyy-MM-dd"),
                            task.EndDate.HasValue ? task.EndDate.Value.ToString("yyyy-MM-dd") : "",
                            task.IsAllDayTask.ToString(),
                            task.IsImportant.ToString()
                            );

                        count++;
                    }
                }

                Console.WriteLine($"Liczba zadań zapisanych w pliku {path}: {count}.");
            }

        }

        public void ShowTaskUpcomming(int upcommingDays)
        {
            Func<TaskModel, bool> filter;
            DateTime today = DateTime.Now;

            filter = task => (int) (task.StartDate - today).TotalDays <= upcommingDays && (int)(task.StartDate - today).TotalDays >= 0;

            ShowTask(filter);
        }

        public void LoadTasks(string path = DefultPath)
        {
            if (!File.Exists(Path.GetFullPath(path)))
            {
                ConsoleEx.WriteLine(ConsoleColor.Red, "Wskazany plik - {0} - nie istnieje", Path.GetFullPath(path));
            }
            else
            {
                int successCount = 0;
                int count = 0;
                using (var reader = new StreamReader(Path.GetFullPath(path)))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        count++;
                        string[] data = line.Split(',');

                        if (data.Length != 5)
                        {
                            ConsoleEx.WriteLine(ConsoleColor.Red, "Dane niepoprawne. Linia {0} zawiera za dużo lub za mało pozycji", count);
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(data[0]))
                            {
                                ConsoleEx.WriteLine(ConsoleColor.Red, "Brak opisu. Rekord {0} zostanie pominięty.", count);
                                continue;
                            }

                            if (!DateTime.TryParse(data[1], out DateTime startDate))
                            {
                                ConsoleEx.WriteLine(ConsoleColor.Red, "Data rozpoczęcia jest niepoprawna. Rekord {0} zostanie pominięty.", count);
                                continue;
                            }

                            DateTime? endDate = null;
                            if (string.IsNullOrWhiteSpace(data[2]))
                            {
                                endDate = null;
                            }
                            else if (!DateTime.TryParse(data[2], out DateTime endDate2))
                            {
                                ConsoleEx.WriteLine(ConsoleColor.Red, "Data zakończenia jest niepoprawna. Rekord {0} zostanie pominięty.", count);
                                continue;
                            }
                            else
                            {
                                endDate = endDate2;
                            }

                            if (!bool.TryParse(data[3], out bool isAllDay))
                            {
                                ConsoleEx.WriteLine(ConsoleColor.Red, "Niepoprawny znacznik zadania całodniowego. Rekord {0} zostanie pominięty.", count);
                                continue;
                            }

                            if (!bool.TryParse(data[4], out bool isImportant))
                            {
                                ConsoleEx.WriteLine(ConsoleColor.Red, "Niepoprawny znacznik ważności zadania. Rekord {0} zostanie pominięty.", count);
                                continue;
                            }

                            //AddTask(data[0], startDate, endDate, isImportant);
                            _taskList.Add(new TaskModel(data[0], startDate, endDate, isImportant));

                        }

                        successCount++;


                    }
                    
                }

                ConsoleEx.WriteLine(ConsoleColor.Green, "Liczba zadań zaimportowanych: {0}", successCount);
            }
        }

    }
}
