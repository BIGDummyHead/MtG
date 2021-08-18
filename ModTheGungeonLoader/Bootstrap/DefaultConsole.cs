using Gungeon.Debug;
using Gungeon.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace Gungeon.Bootstrap
{
    /// <summary>
    /// The default console
    /// </summary>
    public static class DefaultConsole
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

                Application.logMessageReceived += UnityMessageReceived;
                consoleOpen = true;
                EnableReader();

                ParseArgument.Add("give", x =>
                {
                    int a = (int)x[0];

                    PickupIDs.GiveItem(a);

                }, ParseArgument.Create("item"), ParseArgument.Create(typeof(int)));
            }
        }

        private static void ParseCommand(string obj)
        {
            string[] words = CodeExtensions.Words(obj);

            if (words.Length < 1)
                return;

            string command = words[0].ToLower();

            bool success = _commands.TryGetValue(command, out ParseArgument[] args);

            if (!success)
            { 
                "Command is not recognized".LogError();
                return;
            }


            if (Match(words, args, out object[] res))
            {
                _commandMethod[command]?.Invoke(res);
            }
            else
            {
                "Command failed".LogError();
            }
        }

        private static bool Match(string[] words, ParseArgument[] ar, out object[] results)
        {
            List<object> oos = new List<object>();
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                ParseArgument arg = ar[i];

                if (arg.dynamic)
                {
                    arg.value = Convert.ChangeType(word, arg.convert);
                    oos.Add(arg.value);
                }
                else if(!arg.dynamic && !word.Equals(arg.name, StringComparison.OrdinalIgnoreCase))
                {
                    results = null;
                    return false;
                }
            }

            results = oos.ToArray();
            return true;
        }



        private static Dictionary<string, ParseArgument[]> _commands = new Dictionary<string, ParseArgument[]>();
        private static Dictionary<string, Action<object[]>> _commandMethod = new Dictionary<string, Action<object[]>>();

        /// <summary>
        /// Parse-able argument.
        /// </summary>
        public sealed class ParseArgument
        {

            private ParseArgument()
            {

            }

            /// <summary>
            /// Only required if <see cref="dynamic"/> is false. Required for player to type in.
            /// </summary>
            public string name;

            /// <summary>
            /// Is this a value? Everchanging value.
            /// </summary>
            public bool dynamic;

            /// <summary>
            /// If <see cref="dynamic"/> must set.
            /// </summary>
            public Type convert;

            /// <summary>
            /// Set if is <see cref="dynamic"/> 
            /// </summary>
            public object value;

            /// <summary>
            /// Add a command
            /// </summary>
            /// <param name="commandName"></param>
            /// <param name="onCommandExecute"></param>
            /// <param name="args"></param>
            public static void Add(string commandName, Action<object[]> onCommandExecute, params ParseArgument[] args)
            {
                if (args.Length < 1)
                    throw new Exception("Parse-able arguments must be > 0");

                List<ParseArgument> oo = new List<ParseArgument>();


                commandName = commandName.ToLower();

                oo.Add(Create(commandName));
                oo.AddRange(args);

                _commands.Add(commandName, oo.ToArray());
                _commandMethod.Add(commandName, onCommandExecute);
            }

            /// <summary>
            /// Create a static argument.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public static ParseArgument Create(string name)
            {
                return new ParseArgument
                {
                    dynamic = false,
                    name = name
                };
            }

            /// <summary>
            /// Create a dynamic argument. 
            /// </summary>
            /// <remarks>Must be string parse-able.</remarks>
            /// <param name="a"></param>
            /// <returns></returns>
            public static ParseArgument Create(Type a)
            {
                return new ParseArgument
                {
                    dynamic = true,
                    convert = a
                };
            }
        }

        private static StreamWriter writer;
        private static StreamReader reader;

        private const string Unity = "UnityEngine.dll";
        private static void UnityMessageReceived(string condition, string stackTrace, LogType type)
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

        private static bool readerEnabled = false;

        private const int showCon = 5;

        private const int hideCon = 0;

        /// <summary>
        /// When the console is read.
        /// </summary>
        public static event Action<string> OnConsoleRead;
        static Thread consoleThread;

        internal static void EnableReader()
        {
            if (!consoleOpen || readerEnabled)
                return;

            UnityEngine.Object.DontDestroyOnLoad(new GameObject("__CONSOLETHREADMANAGER").AddComponent<ThreadManager>().gameObject);

            consoleThread = new Thread(new ThreadStart(ThreadAccess));
            consoleThread.IsBackground = true;
            consoleThread.Start();


            readerEnabled = true;
        }

        private static void ThreadAccess()
        {
            string eventInvoke = Console.ReadLine();
            OnConsoleRead?.Invoke(eventInvoke);
            ParseCommand(eventInvoke);
            ThreadAccess();
        }
        
        class ThreadManager : MonoBehaviour
        {
            void OnApplicationQuit()
            {
                FreeConsole();
            }
        }



    }
}
