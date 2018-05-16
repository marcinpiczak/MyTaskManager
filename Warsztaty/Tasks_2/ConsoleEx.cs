using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks_2
{
    public static class ConsoleEx
    {
        public static void WriteLine(ConsoleColor color, string message, params object[] args)
        {
            var colorOld = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message, args);
            Console.ForegroundColor = colorOld;
        }

        public static void Write(ConsoleColor color, string message, params object[] args)
        {
            var colorOld = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message, args);
            Console.ForegroundColor = colorOld;
        }
    }
}
