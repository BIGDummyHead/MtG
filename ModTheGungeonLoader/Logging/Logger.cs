using System;

namespace Gungeon.Debug
{
    /// <summary>
    /// A class for logging events in your mod.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Format the message you would like to send.
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string Format(string msgType, string msg)
        {
            return $"{msgType.ToUpper()}: {msg}";
        }

        /// <summary>
        /// Basic log, choose your color
        /// </summary>
        /// <param name="msg">The message you would like to print</param>
        /// <param name="color">Foreground color</param>
        public static void Log(this object msg, ConsoleColor color = ConsoleColor.White)
        {
            LogUnformatted(Format("log", msg?.ToString()), clr: color);
        }

        /// <summary>
        /// Warning Log, Yellow foreground, 'warning' msg type
        /// </summary>
        /// <param name="msg">The message you would like to print</param>
        public static void LogWarning(this object msg)
        {
            LogUnformatted(Format("warning", msg?.ToString()), clr: ConsoleColor.Yellow);
        }

        /// <summary>
        /// Warning Log, Red foreground, 'error' msg type
        /// </summary>
        /// <param name="msg">The message you would like to print</param>
        public static void LogError(this object msg)
        {
            LogUnformatted(Format("error", msg?.ToString()), clr: ConsoleColor.Red);
        }

        /// <summary>
        /// Log an unformatted msg, which you can include <paramref name="withTime"/>
        /// </summary>
        /// <param name="msg">Print</param>
        /// <param name="withTime">Time Format</param>
        /// <param name="clr">Color (Foreground)</param>
        public static void LogUnformatted(object msg, bool withTime = true, ConsoleColor clr = ConsoleColor.White)
        {
            if (withTime)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{DateTime.UtcNow}");

                Console.ResetColor();
                Console.Write("] | ");
            }

            Console.ForegroundColor = clr;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
