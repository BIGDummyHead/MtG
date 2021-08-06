using Gungeon.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gungeon
{
    /// <summary>
    /// A class that all Mods inherit
    /// </summary>
    public abstract class Mod
    {
        /// <summary>
        /// Called on Mod load.
        /// </summary>
        /// <param name="info">Information about the mod</param>
        public virtual void Load(Info info)
        {
        }

        /// <summary>
        /// Info about a mod
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
        public sealed class Info : Attribute
        {
            /// <summary>
            /// Basic info of your Mod
            /// </summary>
            /// <param name="name">Mod name</param>
            /// <param name="description">Mod description</param>
            /// <param name="developer">YOU</param>
            /// <param name="version">Mod's version</param>
            /// <param name="colorName">The color of your console when loading</param>
            public Info(string name, string description, string developer, string version, string colorName = null)
            {
                Name = name ?? string.Empty;
                Description = description ?? string.Empty;
                Developer = developer ?? string.Empty;
                Version = version ?? string.Empty;

                bool z = CodeExtensions.TryParse(colorName, true, out ConsoleColor color);

                WriteColor = z ? color : ConsoleColor.White;
            }

            

            /// <summary>
            /// Name of the mod
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// Description of the mod
            /// </summary>
            public string Description { get; private set; }
            /// <summary>
            /// Developer of the mod
            /// </summary>
            public string Developer { get; private set; }
            /// <summary>
            /// Version of the mod
            /// </summary>
            public string Version { get; private set; }

            /// <summary>
            /// The directory of your mod, relevant to your mod folder.
            /// </summary>
            public string Directory { get; internal set; }


            /// <summary>
            /// The console color when writing out your mod.
            /// </summary>
            public ConsoleColor WriteColor { get; private set; }

            internal string Format()
            {
                return $"=================================\r\n\r\n{ToString()}\r\n\r\n=================================";
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"{Name} : created by {Developer}\r\n\r\n{Description}\r\n\r\nMod Version : {Version}";
            }
        }
    }
}
