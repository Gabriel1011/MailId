using System;
using System.Diagnostics;

namespace MailId
{
    public static class Extensions
    {
        public static void WriteConsoleConversor(this Stopwatch timer, string conversor)
        {
            timer.Stop();
            Console.WriteLine(conversor);
            Console.WriteLine(timer.Elapsed);
            Console.WriteLine();
        }

        public static void ResetTimer(this Stopwatch timer)
        {
            timer.Reset();
            timer.Start();
        }

        public static void TesteString(this string texto) => texto.ToUpper();
    }
}
