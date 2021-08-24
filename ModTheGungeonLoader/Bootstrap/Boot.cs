using Gungeon.Debug;
using Gungeon.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Gungeon.Bootstrap
{
    /// <summary>
    /// A simple class that is invoked by the UnityEngine.dll
    /// </summary>
    public class Boot
    {
        private static bool _devMode = false;
        /// <summary>
        /// Enable the developer mode? Enables a UI to debug different objects. 
        /// </summary>
        /// <remarks>Must be enabled in Plugin. Setting to true will create an instance of <see cref="UnityExplorer.ExplorerStandalone"/></remarks>
        public static bool DeveloperModeEnabled
        {
            get
            {
                return _devMode;
            }

            set
            {
                if (value)
                {
                    UnityExplorer.ExplorerStandalone.CreateInstance();
                    _devMode = true;
                }
                else
                    Logger.LogWarning("CANNOT DISABLE DEVELOPER MODE");
            }
        }

        internal static void Load()
        {
            //hey whatchu you doin ;)
            LoadPlugins();
            Events.GameEvents.Patch();

            ModLoader.GetGlobal().LoaderLoad();
        }


        private static void LoadPlugins()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

            if (!Directory.Exists(path))
                return;

            foreach (string file in Directory.GetFiles(path).Where(x => Path.GetExtension(x).Equals(".dll")))
            {
                try
                {
                    var plug = Assembly.LoadFrom(file);

                    var plugs = plug.GetTypes().Where(x => x.HasInterface(typeof(IPlugin)) && !x.IsAbstract && x.GetConstructor(new Type[0]) != null);

                    foreach (var item in plugs)
                    {
                        IPlugin plugin = Activator.CreateInstance(item) as IPlugin;

                        plugin.Load();
                    }
                }
                catch (Exception ex)
                {
                    $"Something went wrong when loading your Plugin.\r\n{ex.Message}\r\n{ex.InnerException?.Message}".LogError();
                }
            }
        }
    }
}
