using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Gungeon.Bootstrap
{
    /// <summary>
    /// A global mod loader, called by the boostrap, the middle man of the <see cref="ILoader"/>
    /// </summary>
    public sealed class ModLoader
    {
        /// <summary>
        /// Creates or sets the new global loader.
        /// </summary>
        /// <param name="mods">Mods folder</param>
        /// <param name="loader">How to load</param>
        /// <returns></returns>
        public static ModLoader CreateGlobal(string mods, ILoader loader = null)
        {
            _globe = new ModLoader(mods, loader);

            return GetGlobal();
        }

        internal static void Patch()
        {
            new Harmony("com.Gungeon.Mods").PatchAll();
        }

        /// <summary>
        /// Get the global ModLoader
        /// </summary>
        /// <returns></returns>
        public static ModLoader GetGlobal()
        {
            if (_globe == null)
                return _globe = new ModLoader(Path.Combine(Directory.GetCurrentDirectory(), "Mods"));

            return _globe;
        }

        private static ModLoader _globe;

        /// <summary>
        /// The mods folder
        /// </summary>
        public string ModsFolder { get; private set; }
        private readonly ILoader _modLoader;
        internal ModLoader(string mods, ILoader loader = null)
        {
            ModsFolder = mods;
            _modLoader = loader ?? new DefaultLoader();
        }

        internal void LoaderLoad()
        {
            _modLoader.HandleConsole();
            _modLoader.HandleLoad(ModsFolder);
            _modLoader.FinishLoad();
            return;
        }
    }

    /// <summary>
    /// A default loader.
    /// </summary>
    internal sealed class DefaultLoader : ILoader
    {
        public Dictionary<string, Pack> LoadedMods { get; private set; } = new Dictionary<string, Pack>();

        public class Pack
        {
            public string name;
            public Mod mod;
            public Mod.Info info;
        }

        /// <summary>
        /// Refer to <see cref="ILoader.FinishLoad"/>
        /// </summary>
        public void FinishLoad()
        {
            Console.WriteLine();
            foreach (var item in LoadedMods.Values)
            {
                item.mod.Load(item.info);
            }

            Console.WriteLine();
            Console.WriteLine("===========================================");
            Console.WriteLine($"Finished Loading Mods!");
            Console.WriteLine($"A total of {LoadedMods.Count} were loaded!");
            Console.WriteLine("===========================================");
            Console.WriteLine();
        }
        /// <summary>
        /// Refer to <see cref="ILoader.HandleConsole"/>
        /// </summary>
        public void HandleConsole()
        {
            DefaultConsole.Open();
        }

        /// <summary>
        /// Refer to <see cref="ILoader.HandleLoad(string)"/>
        /// </summary>
        public void HandleLoad(string modsFolder)
        {
            foreach (string dir in Directory.GetDirectories(modsFolder))
            {
                string[] files = Directory.GetFiles(dir);

                string modInfoFile = (files.Length < 1) ? default : files.FirstOrDefault(x => Path.GetExtension(x).Equals(".mod"));

                if (modInfoFile != default)
                {
                    Mod.Info infoOnMod = Settings.Read<Mod.Info>(modInfoFile);
                    infoOnMod.Directory = dir;

                    if (LoadedMods.ContainsKey(infoOnMod.name))
                    {
                        Debug.Logger.LogWarning($"Mod has already been loaded : {infoOnMod.name}");
                    }
                    else
                    {
                        foreach (string file in files)
                        {
                            string ext = Path.GetExtension(file);

                            if (ext.Equals(".dll"))
                            {
                                try
                                {
                                    Assembly assem = Assembly.LoadFrom(file);
                                    var mods = assem.GetTypes().Where(x => x.BaseType == typeof(Mod) && x.GetConstructor(new Type[0]) != null && !x.IsAbstract);

                                    foreach (Type mod in mods)
                                    {
                                        var instance = Activator.CreateInstance(mod) as Mod;

                                        Console.WriteLine(infoOnMod.Format());

                                        LoadedMods.Add(infoOnMod.name, new Pack
                                        {
                                            name = infoOnMod.name,
                                            info = infoOnMod,
                                            mod = instance
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.Logger.LogWarning($"Woah, something happened while loading {infoOnMod.name}");
                                    Debug.Logger.LogError($"{ex.Message}\r\n{ex.InnerException?.Message}");
                                }
                            }
                        }

                    }
                }


            }
        }
    }

}
