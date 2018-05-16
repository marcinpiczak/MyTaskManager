using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tasks
{
    class Program
    {

        public static List<TaskModel> taskList = new List<TaskModel>();

        static void Main(string[] args)
        {
            string DefaultFilePath = "data.csv";

            if (File.Exists(DefaultFilePath))
            {
                LoadTask(DefaultFilePath);
            }
            Console.Clear();
            ConsoleEx.WriteLine($"Automatycznie zostały załadowane zadania z pliku {DefaultFilePath} o ile istniał", ConsoleColor.White);

            string command = "";

            do
            {
                ConsoleEx.WriteLine("Podaj polecenie: ", ConsoleColor.Green);
                command = Console.ReadLine().ToLower();

                if (command == "add")
                {
                    ConsoleEx.WriteLine("Podaj dane w formacie: ", ConsoleColor.Blue);
                    ConsoleEx.Write("opis;data_rozpoczęcia;[ważność-opcjonalne]", ConsoleColor.Yellow);
                    Console.WriteLine(" - dla zadania całodniowego");
                    Console.WriteLine("lub");
                    ConsoleEx.Write("opis;data_rozpoczęcia;data_zakończenia;[ważność-opcjonalne]", ConsoleColor.Yellow);
                    Console.WriteLine(" - dla zadania z konkretnymi datami");
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input) || (input.Trim(' ', ';') == ""))
                    {
                        ConsoleEx.WriteLine("Podane dane są w niewłaściwym formacie", ConsoleColor.Red);
                    }
                    else
                    {
                        AddTask(input);
                    }
                }

                if (command == "show")
                {
                    ShowTaska();
                }

                if (command == "remove")
                {
                    ConsoleEx.WriteLine("Podaj indeks zadania do usunięcia lub wpisz show aby najpierw wyświetlić listę zadań", ConsoleColor.Green);
                    string remCommand = Console.ReadLine().ToLower();

                    if (remCommand == "show")
                    {
                        ShowTaska();
                        ConsoleEx.WriteLine("Podaj indeks zadania do usunięcia", ConsoleColor.Green);
                        remCommand = Console.ReadLine().ToLower();
                    }

                    if (int.TryParse(remCommand, out int index))
                    {
                        RemoveTask(index);
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Wprowadzona wartość jest niepoprawna", ConsoleColor.Red);
                    }


                }

                if (command == "save")
                {
                    SaveTask("tasks.csv");
                }

                if (command == "load")
                {
                    ConsoleEx.WriteLine($"Czy załadować zadnia z innego pliku niż standardowy {DefaultFilePath}. Jeżeli TAK podaj nazwę pliku w przeciwnym wyapdku naciśnij Enter?", ConsoleColor.White);
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        LoadTask(DefaultFilePath);
                        }
                    else
                    {
                        LoadTask(input);
                    }
                }

                if (command == "close")
                {
                    int numDays = 2;
                    ConsoleEx.WriteLine($"Zbliżające się zadania na {numDays} dni", ConsoleColor.DarkYellow);
                    ShowTaska(numDays);
                }

                if (command == "filter")
                {
                    ConsoleEx.WriteLine("Zbliżające się zadania", ConsoleColor.DarkYellow);
                    ShowTaska(2);
                }

            } while (command != "exit");

        }

        public static void AddTask(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                string[] items = input.Split(';');

                if (items.Length < 2)
                {
                    ConsoleEx.WriteLine("Podano za moało parametrów", ConsoleColor.Red);
                }
                else if (items.Length == 2)
                {
                    if (items[0].Trim() != "" && DateTime.TryParse(items[1], out DateTime sDate))
                    {
                        taskList.Add(new TaskModel(items[0], sDate, null));
                        ConsoleEx.WriteLine("Dodano", ConsoleColor.Green);
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Brak opisu zadania lub data rozpoczęcia jest niepoprawna", ConsoleColor.Red);
                    }
                }
                else if (items.Length == 3)
                {
                    if (items[0].Trim() != "" && DateTime.TryParse(items[1], out DateTime sDate))
                    {
                        if (bool.TryParse(items[2], out bool b))
                        {
                            taskList.Add(new TaskModel(items[0], sDate, null, b));
                            ConsoleEx.WriteLine("Dodano", ConsoleColor.Green);
                        }
                        else if (DateTime.TryParse(items[2], out DateTime eDate))
                        {
                            taskList.Add(new TaskModel(items[0], sDate, eDate));
                            ConsoleEx.WriteLine("Dodano", ConsoleColor.Green);
                        }
                        else
                        {
                            ConsoleEx.WriteLine("Data zakończenia lub znacznik ważności są niepoprawne.", ConsoleColor.Red);
                        }
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Brak opisu zadania lub data rozpoczęcia jest niepoprawna", ConsoleColor.Red);
                    }
                }
                else if (items.Length == 4)
                {
                    if (items[0].Trim() != "" && DateTime.TryParse(items[1], out DateTime sDate) && DateTime.TryParse(items[2], out DateTime eDate) && bool.TryParse(items[3], out bool b))
                    {
                        taskList.Add(new TaskModel(items[0], sDate, eDate, b));
                        ConsoleEx.WriteLine("Dodano", ConsoleColor.Green);
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Brak opisu zadania lub data rozpoczęcia jest niepoprawna", ConsoleColor.Red);
                    }
                }
                else
                {
                    ConsoleEx.WriteLine("Podano za dużo danych", ConsoleColor.Red);
                }
            }
        }
        /*
        public static void AddTask1(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                string[] items = input.Split(';');

                bool step1 = false;
                bool step2 = false;
                bool step3 = false;

                string descr;
                DateTime startDate;
                DateTime? endDate;
                bool isImportant;


                if (items.Length < 2)
                {
                    ConsoleEx.WriteLine("Podano za moało parametrów", ConsoleColor.Red);
                }

                if (items.Length >= 2)
                {
                    if (items[0].Trim() != "" && DateTime.TryParse(items[1], out DateTime sDate))
                    {
                        descr = items[1];
                        startDate = sDate;
                        step1 = true;
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Brak opisu zadania lub data rozpoczęcia jest niepoprawna", ConsoleColor.Red);
                    }
                }

                if (step1 && items.Length >= 3)
                {
                    if (bool.TryParse(items[2], out bool b))
                    {
                        isImportant = b;
                        step2 = true;
                    }
                    else if (DateTime.TryParse(items[2], out DateTime eDate))
                    {
                        endDate = eDate;
                        step2 = true;
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Data zakończenia lub znacznik ważności są niepoprawne.", ConsoleColor.Red);
                        step1 = false;
                    }
                }

                if (step2 && items.Length == 4)
                {
                    if (bool.TryParse(items[3], out bool b))
                    {
                        isImportant = b;
                        step3 = true;
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Brak opisu zadania lub data rozpoczęcia jest niepoprawna", ConsoleColor.Red);
                        step1 = false;
                        step2 = false;
                    }
                }

                if (items.Length > 4)
                {
                    ConsoleEx.WriteLine("Podano za dużo danych", ConsoleColor.Red);
                }

                if (step1 || step2 || step3)
                {
                    taskList.Add(new TaskModel(items[0], startDate, null));
                    ConsoleEx.WriteLine("Dodano", ConsoleColor.Green);
                }

            }
        }
        */

        public static void ShowTaska(int? close = null)
        {
            if (taskList.Count > 0)
            {
                string h = "| " + "Id".PadLeft(5) + " | " + "Opis".PadLeft(20) + " | " +
                           "Data".PadLeft(15) + " | " + "Data".PadLeft(15) + " | " +
                           "Zadanie".PadLeft(15) + " | " + "Ważne?".PadLeft(10) + " |";
                string h2 = "| " + "".PadLeft(5) + " | " + "".PadLeft(20) + " | " +
                            "ropzoczęcia".PadLeft(15) + " | " + "zakończenia".PadLeft(15) + " | " +
                            "całodniowe".PadLeft(15) + " | " + "".PadLeft(10) + " |";
                Console.WriteLine("".PadLeft(h.Length, '-'));
                Console.WriteLine(h);
                Console.WriteLine(h2);
                Console.WriteLine("".PadLeft(h.Length, '-'));

                taskList.Sort((x, y) => { return x.StartDate.CompareTo(y.StartDate);}); //sortowanie

                foreach (var task in taskList)
                {
                    if (close.HasValue)
                    {
                        var diff = DateTime.Now - task.StartDate;
                        if ((int)diff.TotalDays > close)
                        {
                            continue;
                        }
                    }

                    int index = taskList.IndexOf(task);
                    var l = new StringBuilder();

                    string eDate;

                    if (task.EndDate.HasValue)
                    {
                        eDate = task.EndDate.Value.ToString("d");
                    }
                    else
                    {
                        eDate = "";
                    }

                    l.Append("| ");
                    l.Append(index.ToString().PadLeft(5));
                    l.Append(" | ");
                    l.Append(task.Description.PadLeft(20));
                    l.Append(" | ");
                    l.Append(task.StartDate.ToString("d").PadLeft(15));
                    l.Append(" | ");
                    l.Append(eDate.PadLeft(15));
                    l.Append(" | ");
                    l.Append(task.IsAllDayTask.ToString().PadLeft(15));
                    l.Append(" | ");
                    l.Append(task.IsImportant.ToString().PadLeft(10));
                    l.Append(" |");

                    //string l = $"";
                    Console.WriteLine(l);
                    Console.WriteLine("".PadLeft(h.Length, '-'));
                }
            }
            else
            {
                ConsoleEx.WriteLine("Brak elementów do wyświetlenia - lista jest pusta", ConsoleColor.Red);
            }

        }

        public static void RemoveTask(int taskIndex)
        {
            if (taskIndex < 0 || taskIndex > taskList.Count)
            {
                ConsoleEx.WriteLine("Wprowadzona wartość idenksu jest niepoprawna - mniejsza od zera lub przekracza liczebność zadań", ConsoleColor.Red);
            }
            else
            {
                taskList.RemoveAt(taskIndex);
            }
        }

        public static string[] Export()
        {
            string[] taskTab = new string[taskList.Count];
            var sb = new StringBuilder();

            for (int i = 0; i < taskList.Count; i++)
            {
                sb.Clear();
                string sDate;
                if (taskList[i].EndDate.HasValue)
                {
                    sDate = taskList[i].EndDate.Value.ToString("d");
                }
                else
                {
                    sDate = "";
                }

                sb.Append(taskList[i].Description);
                sb.Append(",");
                sb.Append(taskList[i].StartDate.ToString("d"));
                sb.Append(",");
                sb.Append(sDate);
                sb.Append(",");
                sb.Append(taskList[i].IsAllDayTask.ToString());
                sb.Append(",");
                sb.Append(taskList[i].IsImportant.ToString());

                taskTab[i] = sb.ToString();
            }

            return taskTab;
        }

        public static void SaveTask(string filePath)
        {
            if (taskList.Count == 0)
            {
                ConsoleEx.WriteLine("Brak elementów do zapisania - lista jest pusta", ConsoleColor.Red);
            }
            else
            {
                string[] taskListTab = Export();

                if (File.Exists(filePath))
                {
                    File.AppendAllLines(filePath, taskListTab);
                }
                else
                {
                    File.WriteAllLines(filePath, taskListTab);
                }
                ConsoleEx.WriteLine("Zapisano", ConsoleColor.Green);

            }

        }

        public static void LoadTask(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] fileLines = File.ReadAllLines(filePath);

                if (fileLines.Length != 0)
                {
                    foreach (var line in fileLines)
                    {
                        string[] taskListTab = line.Split(',');
                        taskListTab[3] = "";

                        string tempLine = string.Join(';', taskListTab).Replace(";;", ";").Replace(";;", ";");
                        AddTask(tempLine);
                    }
                }
                else
                {
                    ConsoleEx.WriteLine("W pliku nie znaleziono żadnych linii do wczytania.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleEx.WriteLine("Nie znaleziono pliku do wczytania.", ConsoleColor.Red);
            }
        }
    }
}