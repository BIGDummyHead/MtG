using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace Gungeon.Bootstrap
{
    /// <summary>
    /// The default console
    /// </summary>
    public  static class DefaultConsole
    {
        /// <summary>
        /// Allocate the console
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        /// <summary>
        /// Get the ptr for the window
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        /// <summary>
        /// Show window
        /// </summary>
        /// <param name="win"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr win, int i);
        /// <summary>
        /// Free up the console, closes.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        /// <summary>
        /// Show window.
        /// </summary>
        public static void Show()
        {
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, showCon);
        }

        /// <summary>
        /// Close window
        /// </summary>
        public static void Close()
        {
            IntPtr handle = GetConsoleWindow();

            ShowWindow(handle, hideCon);
        }

        /// <summary>
        /// Open console, if opened <see cref="Show"/> is called
        /// </summary>
        public static void Open()
        {
            if (consoleOpen)
                Show();
            else
            {
                AllocConsole();
                writer = new StreamWriter(System.Console.OpenStandardOutput());
                writer.AutoFlush = true;

                reader = new StreamReader(Console.OpenStandardInput());

                System.Console.SetError(writer);
                System.Console.SetOut(writer);
                Console.SetIn(reader);

                Application.logMessageReceived += Application_logMessageReceived;
                consoleOpen = true;
            }
        }

        private static StreamWriter writer;
        private static StreamReader reader;

        private const string Unity = "UnityEngine.dll";
        private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {

            switch (type)
            {
                case LogType.Error:
                    Debug.Logger.LogUnformatted($"ERROR: {condition}", Unity, ConsoleColor.Red);
                    break;
                case LogType.Assert:
                    Debug.Logger.LogUnformatted($"ASSERT: {condition}", Unity, ConsoleColor.DarkYellow);
                    break;
                case LogType.Warning:
                    Debug.Logger.LogUnformatted($"WARNING: {condition}", Unity, ConsoleColor.Yellow);
                    break;
                case LogType.Log:
                    Debug.Logger.LogUnformatted($"LOG: {condition}", Unity);
                    break;
                case LogType.Exception:
                    Debug.Logger.LogUnformatted($"ERROR: {condition}", Unity, ConsoleColor.Red);
                    break;
            }
        }

        private static bool consoleOpen = false;

        private const int showCon = 5;

        private const int hideCon = 0;

        /// <summary>
        /// When the console is read.
        /// </summary>
        public static event Action<string> OnConsoleRead;

        /// <summary>
        /// Read a line from the console, non-thread blocking.
        /// </summary>
        public static void ReadLine()
        {
            if (!consoleOpen)
                return;

            new Thread(new ThreadStart(ThreadAccess)).Start();
        }

        private static void ThreadAccess()
        {
            string evenInvoke = Console.ReadLine();
            OnConsoleRead?.Invoke(evenInvoke);
            //thread = new Thread(new ThreadStart(ThreadAccess));
        }
    }
}
