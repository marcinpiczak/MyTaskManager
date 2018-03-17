using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks
{
    public static class ConsoleEx
    {
        public static void WriteLine(string message, ConsoleColor color, params object[] args)
        {
            var colorOld = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message, args);
            Console.ForegroundColor = colorOld;
        }

        public static void Write(string message, ConsoleColor color, params object[] args)
        {
            var colorOld = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message, args);
            Console.ForegroundColor = colorOld;
        }
    }
}
