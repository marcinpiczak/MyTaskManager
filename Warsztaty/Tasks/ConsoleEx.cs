using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks
{
    public static class ConsoleEx
    {
        public static void WriteLine(string message, ConsoleColor color, string[] args = null)
        {
            var colorOld = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (args != null) Console.WriteLine(message, args);
            else Console.WriteLine(message);
            Console.ForegroundColor = colorOld;
        }

        public static void Write(string message, ConsoleColor color, string[] args = null)
        {
            var colorOld = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (args != null) Console.Write(message, args);
            else Console.Write(message);
            Console.ForegroundColor = colorOld;
        }
    }
}
