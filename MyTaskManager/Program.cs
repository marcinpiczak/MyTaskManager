using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Tasks_2
{
    class Program
    {
        static void Main(string[] args)
        {

            string command = "";
            var listManager = new ProgramLogic();

            if (File.Exists(ProgramLogic.DefultPath))
            {
                ConsoleEx.WriteLine(ConsoleColor.DarkYellow, "Odnaleziono plik {0}. Dane z pliku zostaną zaimportowane.", ProgramLogic.DefultPath);
                listManager.LoadTasks();
                Console.WriteLine();
            }

            ConsoleEx.WriteLine(ConsoleColor.Green, "Zbliżające się zadania: ");
            listManager.ShowTaskUpcomming(3);
            Console.WriteLine();

            do
            {
                ConsoleEx.WriteLine(Console.ForegroundColor, "Zarządzania zadaniami: ");
                ConsoleEx.Write(Console.ForegroundColor, "      1. Dodanie zadania - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "add");
                ConsoleEx.Write(Console.ForegroundColor, "      2. Usuwanie zadań - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "remove");
                ConsoleEx.Write(Console.ForegroundColor, "      3. Wyświetlenie listy zadań - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "show");
                ConsoleEx.Write(Console.ForegroundColor, "      4. Wyszukiwanie zadań - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "filter");
                ConsoleEx.Write(Console.ForegroundColor, "      5. Zapisanie zadań do pliku - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "save");
                ConsoleEx.Write(Console.ForegroundColor, "      6. Wczytanie zadań z pliku - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "load");
                ConsoleEx.Write(Console.ForegroundColor, "      7. Wyjście - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "exit");
                Console.WriteLine();

                ConsoleEx.Write(ConsoleColor.Green, "Podaj polecenie: ");
                command = Console.ReadLine().ToLower();

                if (command == "add")
                {
                    string h = "------Dodawanie nowego zadania------";
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length,'-'));
                    ConsoleEx.WriteLine(ConsoleColor.Blue, h);
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length, '-'));

                    string taskDescription = AskForString("Podaj opis zadania");
                    DateTime startDate = AskForDate("Podaj datę rozpoczęcia zadania");
                    bool isAllDay = AskForBool("Czy zadanie jest całodniowe: t/n");

                    DateTime? endDate = null;

                    if (!isAllDay)
                    {
                        endDate = AskForDate("Podaj datę zakończenia zadania");
                    }

                    bool isImportant = AskForBool("Czy zadanie jest ważne: t/n");

                    listManager.AddTask(taskDescription, startDate, endDate, isImportant);
                }

                if (command == "show")
                {
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "Znaleziono nastepujące zadania: ");
                    listManager.ShowTask();
                }

                if (command == "remove")
                {
                    if (listManager.ListCount == 0)
                    {
                        ConsoleEx.WriteLine(ConsoleColor.Red, "Lista nie zawiera żadnych zadań. ");
                        continue;
                    }

                    string h = "------Usuwanie zadania------";
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length, '-'));
                    ConsoleEx.WriteLine(ConsoleColor.Blue, h);
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length, '-'));

                    listManager.ShowTask();

                    int taskIndex = AskForItemListIndex("Podaj numer zadania do usunięcia", listManager.ListCount);

                    if (taskIndex == -1)
                    {
                        Console.WriteLine();
                        continue;
                    }

                    bool answer = AskForBool($"Czy napewno usunąć zadanie numerze {taskIndex}. t/n");
                    if (answer)
                    {
                        listManager.RemoveTask(taskIndex);
                        ConsoleEx.WriteLine(ConsoleColor.Green, "Usunięto zadanie o numerze {0}", taskIndex);
                    }
                    else
                    {
                        ConsoleEx.WriteLine(ConsoleColor.Green, "Anulowano usuwanie zadania o numerze {0}", taskIndex);
                    }

                }

                if (command == "save")
                {
                    string h = "------Zapisywanie zadań------";
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length, '-'));
                    ConsoleEx.WriteLine(ConsoleColor.Blue, h);
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length, '-'));

                    string filePath = "";

                    ConsoleEx.WriteLine(ConsoleColor.Green, "Do pliku zostaną zapisane nastepujące zadania: ");
                    listManager.ShowTask();

                    bool saveToFile = AskForBool($"Czy zapisać zadania do domyślnego pliku: {Path.GetFullPath(ProgramLogic.DefultPath)} ? t /n");

                    if (saveToFile)
                    {
                        filePath = ProgramLogic.DefultPath;
                    }
                    else
                    {
                        filePath = AskForFilePath("Podaj ścieżkę do pliku to którego mają zostać zapisane zadania.");
                    }

                    if (File.Exists(filePath))
                    {
                        bool overrideFile = AskForBool($"Wskazany plik: {Path.GetFullPath(filePath)} już istnieje. Nadisać? t /n");
                        if (overrideFile)
                        {
                            listManager.SaveTasks(filePath);
                        }
                    }
                    else
                    {
                        listManager.SaveTasks(filePath);
                    }

                }
                
                if (command == "load")
                {
                    string h = "------Wczytywanie zadań------";
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length, '-'));
                    ConsoleEx.WriteLine(ConsoleColor.Blue, h);
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length, '-'));

                    string filePath = "";

                    bool loadFromFile = AskForBool($"Czy wczytać zadania z domyślnego pliku: {Path.GetFullPath(ProgramLogic.DefultPath)} ? t /n");

                    if (loadFromFile)
                    {
                        filePath = ProgramLogic.DefultPath;
                    }
                    else
                    {
                        filePath = AskForFilePath("Podaj ścieżkę do pliku z którego mają zostać wczytane zadania.");
                    }

                    listManager.LoadTasks(filePath);
                }

                if (command == "filter")
                {
                    string h = "------Filtorwanie zadań------";
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length, '-'));
                    ConsoleEx.WriteLine(ConsoleColor.Blue, h);
                    ConsoleEx.WriteLine(ConsoleColor.Blue, "".PadLeft(h.Length, '-'));

                    Func<TaskModel, bool> filter = null;

                    int filterChoice = 0;
                    bool addNext;

                    do
                    {
                        ConsoleEx.WriteLine(ConsoleColor.DarkCyan, "Pola dostępne do filtorwania zadań: ");
                        ConsoleEx.WriteLine(ConsoleColor.DarkCyan, " 1. Opis,");
                        ConsoleEx.WriteLine(ConsoleColor.DarkCyan, " 2. Data rozpoczęcia,");
                        ConsoleEx.WriteLine(ConsoleColor.DarkCyan, " 3. Data zakończenia,");
                        ConsoleEx.WriteLine(ConsoleColor.DarkCyan, " 4. Znacznik zadania całodniowego,");
                        ConsoleEx.WriteLine(ConsoleColor.DarkCyan, " 5. Znacznik ważności zadania.");
                        //ConsoleEx.WriteLine(ConsoleColor.DarkCyan, "Aby zakończyć budowanie filtra wpisz 0.");

                        filterChoice = AskForInt("Wprowadź numer");

                        switch (filterChoice)
                        {
                            case 0:
                                break;
                            case 1:
                                string descr = AskForString("Podaj wartość dla pola \"Opis\"");
                                filter += task => task.Description.Contains(descr);
                                break;
                            case 2:
                                DateTime startDate = AskForDate("Podaj wartość dla pola \"Data rozpoczęcia\"");
                                filter += task => task.StartDate == startDate;
                                break;
                            case 3:
                                DateTime endDate = AskForDate("Podaj wartość dla pola \"Data zakończenia\"");
                                filter += task => task.StartDate == endDate;
                                break;
                            case 4:
                                bool isAllDay = AskForBool("Podaj wartość dla pola \"Znacznik ważności zadania\" t/n");
                                filter += task => task.IsAllDayTask == isAllDay;
                                break;
                            case 5:
                                bool isimportant = AskForBool("Podaj wartość dla pola \"Znacznik ważności zadania\" t/n");
                                filter += task => task.IsImportant == isimportant;
                                break;
                            default:
                                ConsoleEx.WriteLine(ConsoleColor.Red, "Niepoprawny wybór.");
                                break;
                        }

                        addNext = AskForBool("Dodać kolejne pole do filtra? t/n");

                    //} while (filterChoice != 0);
                    } while (addNext);

                    ConsoleEx.WriteLine(ConsoleColor.Blue, "List zadań zgodna z filterm: ");
                    listManager.ShowTask(filter);
                }

            } while (command != "exit");
        }

        public static string AskForString(string message)
        {
            ConsoleEx.Write(ConsoleColor.Green, "{0} : ", message);
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleEx.Write(ConsoleColor.Red, "Podałeś pustą wartość. ");
                input = AskForString(message);
            }

            return input;
        }

        public static DateTime AskForDate(string message)
        {
            string input = AskForString(message);
            bool dateOK = DateTime.TryParse(input, out DateTime startDate);
            if (!dateOK)
            {
                ConsoleEx.Write(ConsoleColor.Red, "Podana data jest nieprawidłowa. ");
                startDate = AskForDate(message);
            }

            return startDate;
        }

        public static bool AskForBool(string message)
        {
            string input = AskForString(message);
            bool result = false;

            if (input.ToLower() != "t" && input.ToLower() != "n")
            {
                ConsoleEx.Write(ConsoleColor.Red, "Odpowiadaj tylko t/n. ");
                result = AskForBool(message);
            }

            if (input.ToLower() == "t")
            {
                result = true;
            }

            return result;
        }

        public static int AskForInt(string message)
        {
            string input = AskForString(message);

            if (!int.TryParse(input, out int result))
            {
                ConsoleEx.Write(ConsoleColor.Red, "Podana wartość nie jest liczbą całkowitą. ");
                result = AskForInt(message);
            }

            return result;
        }

        public static int AskForItemListIndex(string message, int listCount)
        {
            if (listCount == 0)
            {
                ConsoleEx.Write(ConsoleColor.Red, "Lista nie zawiera żadnych zadań. ");
                return -1;
            }

            int input = AskForInt(message);

            if (input < 1 || input > listCount)
            {
                ConsoleEx.Write(ConsoleColor.Red, $"Podany indeks zadania jest niepoprawny. Podaj liczbę z zakresu: od 1 do {listCount}. ");
                input = AskForItemListIndex(message, listCount);
            }

            return input;
        }

        public static string AskForFilePath(string message)
        {
            string filePath = AskForString(message);

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                ConsoleEx.Write(ConsoleColor.Red, "Wskazana ścieżka nie istnieje. ");
                filePath = AskForFilePath(message);
            }

            return filePath;
        }

    }
}
