using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = "";
            
            do
            {
                ConsoleEx.WriteLine("Podaj polecenie: ", ConsoleColor.Green);
                command = Console.ReadLine().ToLower();




            } while (command != "exit");

        }
    }
}