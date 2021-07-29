using System;

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
        [Serializable]
        public class Info
        {
            /// <summary>
            /// Name of the mod
            /// </summary>
            public string name;
            /// <summary>
            /// Description of the mod
            /// </summary>
            public string desc;
            /// <summary>
            /// Developer of the mod
            /// </summary>
            public string dev;
            /// <summary>
            /// Version of the mod
            /// </summary>
            public string ver;

            /// <summary>
            /// The directory of your mod, relevant to your mod folder.
            /// </summary>
            public string Directory { get; internal set; }

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
                return $"{name} : created by {dev}\r\n\r\n{desc}\r\n\r\nMod Version : {ver}";
            }
        }
    }
}
