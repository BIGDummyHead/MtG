using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Gungeon.Bootstrap
{
    internal static class DefaultConsole
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr win, int i);

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public static void Show()
        {
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, showCon);
        }

        public static void Close()
        {
            IntPtr handle = GetConsoleWindow();

            ShowWindow(handle, hideCon);
        }

        public static void Open()
        {
            if (consoleOpen)
                Show();
            else
            {
                AllocConsole();
                StreamWriter streamWriter = new StreamWriter(System.Console.OpenStandardOutput());
                streamWriter.AutoFlush = true;
                System.Console.SetError(streamWriter);
                System.Console.SetOut(streamWriter);

                Application.logMessageReceived += Application_logMessageReceived;
                consoleOpen = true;
            }
        }

        private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    Debug.Logger.LogError(condition);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    Debug.Logger.LogWarning(condition);
                    break;
                case LogType.Log:
                    Debug.Logger.Log(condition);
                    break;
                case LogType.Exception:
                    Debug.Logger.LogError(condition);
                    break;
            }
        }

        private static bool consoleOpen = false;

        private const int showCon = 5;

        private const int hideCon = 0;
    }
}
