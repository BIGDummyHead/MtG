using Gungeon.Debug;
using Gungeon.Utilities;
using StringParser;
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

        private static CommandHandler handler = new CommandHandler();

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

                handler.Options.OnLog += Error;

                handler.Register<BootCommandModule>();
            }
        }

        private static void Error(string obj)
        {
            obj.LogError();
        }

        /// <summary>
        /// Register to the handle, used for invoking commands.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>() where T : BaseCommandModule
        {
            handler.Register<T>();
        }

        //private static void ParseCommand(string obj)
        //{
        //    string[] words = CodeExtensions.Words(obj);

        //    if (words.Length < 1)
        //        return;

        //    string commandName = words[0].ToLower();

        //    if(ParseArgument._commands.TryGetValue(commandName, out ParseArgument[] args))
        //    {
        //        if(GetInvocation(words, args, out string[] invoke))
        //        {
        //            ParseArgument._commandMethod[commandName]?.Invoke(invoke);
        //        }
        //    }
        //    else
        //        $"Command does not exist '{commandName}".LogWarning();
        //}

        //private static bool GetInvocation(string[] everyWord, ParseArgument[] parse, out string[] parsed)
        //{
        //    if(everyWord.Length != parse.Length)
        //    {
        //        "Command missing arguments".LogError();
        //        parsed = new string[0];
        //        return false;
        //    }    

        //    List<string> _args = new List<string>();

        //    for (int i = 0; i < everyWord.Length; i++)
        //    {
        //        var p = parse[i];
        //        var s = everyWord[i];

        //        if (p.dynamic)
        //            _args.Add(s);
        //    }

        //    parsed = _args.ToArray();
        //    return true;
        //}



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
            handler.Invoke(eventInvoke);
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

    internal class BootCommandModule : BaseCommandModule
    {
        [Command]
        public void Heal()
        {
            foreach (var x in ModUtilities.AllPlayers)
            {
                x.healthHaver?.FullHeal();
            }

            "Players healed".Log(ConsoleColor.Green);
        }

        [Command]
        public void Refill()
        {
            foreach (var x in ModUtilities.AllPlayers)
            {
                foreach (var gun in x.inventory.AllGuns)
                {
                    gun.GainAmmo(gun.GetBaseMaxAmmo());
                }
            }

            "Weapons refilled".Log(ConsoleColor.Green);
        }

        [Command]
        public void Spawn(string guid)
        {
            AIActor actor = EnemyIDs.Spawn(guid, ModUtilities.CurrentPlayer.CurrentRoom);

            if (actor == null)
                Debug.Logger.LogError("Failed to spawn enemy");
            else
                Debug.Logger.Log($"{actor.GetActorName()} spawned", ConsoleColor.Cyan);
        }

        [Command]
        public void Give(string item, string id)
        {
            if (!item.Equals("item", StringComparison.OrdinalIgnoreCase))
                return;

            PickupObject o;

            if(int.TryParse(id, out int idRes))
            {
                o = PickupIDs.GiveItem(idRes);

                if(o == null)
                {
                    $"Could not spawn item with ID of '{id}'".LogError();
                }
                else
                {
                    $"Item added to inventory {o.DisplayName}".Log(ConsoleColor.Cyan);
                }
            }
            else
            {
                o = PickupIDs.GiveItem(id);

                if (o == null)
                {
                    $"Could not spawn item with name of '{id}'".LogError();
                }
                else
                {
                    $"Item added to inventory {o.DisplayName}".Log(ConsoleColor.Cyan);
                }
            }
        }
    }

    /// <summary>
    /// Parse-able argument.
    /// </summary>
    //public sealed class ParseArgument
    //{

    //    internal static Dictionary<string, ParseArgument[]> _commands = new Dictionary<string, ParseArgument[]>();
    //    internal static Dictionary<string, Action<string[]>> _commandMethod = new Dictionary<string, Action<string[]>>();

    //    private ParseArgument()
    //    {

    //    }

    //    /// <summary>
    //    /// Only required if <see cref="dynamic"/> is false. Required for player to type in.
    //    /// </summary>
    //    public string name;

    //    /// <summary>
    //    /// Is this a value? Everchanging value.
    //    /// </summary>
    //    public bool dynamic;

    //    /// <summary>
    //    /// Set if is <see cref="dynamic"/> 
    //    /// </summary>
    //    public object value;

    //    /// <summary>
    //    /// Add a command
    //    /// </summary>
    //    /// <param name="commandName"></param>
    //    /// <param name="onCommandExecute"></param>
    //    /// <param name="args"></param>
    //    public static void Add(string commandName, Action<string[]> onCommandExecute, params ParseArgument[] args)
    //    {
    //        List<ParseArgument> oo = new List<ParseArgument>();

    //        commandName = commandName.ToLower();

    //        oo.Add(Create(commandName));
    //        oo.AddRange(args);

    //        _commands.Add(commandName, oo.ToArray());
    //        _commandMethod.Add(commandName, onCommandExecute);
    //    }

    //    /// <summary>
    //    /// Create a static argument.
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <returns></returns>
    //    public static ParseArgument Create(string name)
    //    {
    //        return new ParseArgument
    //        {
    //            dynamic = false,
    //            name = name
    //        };
    //    }

    //    /// <summary>
    //    /// Create a dynamic value.
    //    /// </summary>
    //    /// <returns></returns>
    //    public static ParseArgument Create()
    //    {
    //        return new ParseArgument
    //        {
    //            dynamic = true
    //        };
    //    }

    //}
}
