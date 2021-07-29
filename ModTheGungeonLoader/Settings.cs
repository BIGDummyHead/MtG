using System;
using System.IO;
using UnityEngine;

namespace Gungeon
{
    /// <summary>
    /// Used for Serializing / Deserializing JSON files.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Convert JSON to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="settings">File path</param>
        /// <returns></returns>
        public static T Read<T>(string settings)
        {
            if (!FileExistInternal(settings))
                return default;

            try
            {
                string content = File.ReadAllText(settings);
                T fin = JsonUtility.FromJson<T>(content);
                return fin;
            }
            catch (Exception ex)
            {
                Debug.Logger.LogWarning($"An error occured while parsing {settings}, see more below.");
                Debug.Logger.LogError(ex.Message);
            }

            return default;
        }

        /// <summary>
        /// Write Json to a File
        /// </summary>
        /// <typeparam name="T">An instance</typeparam>
        /// <param name="settings">File path</param>
        /// <param name="instance">An instance</param>
        /// <param name="format">Format Type</param>
        public static void Write<T>(string settings, T instance, Format format = Format.Indent)
        {
            if (!FileExistInternal(settings))
                return;

            if (instance == null)
            {
                Debug.Logger.LogError("Instance cannot be null when serializing DATA : Error at - Settings.Write(string, T)");
                return;
            }

            string json = JsonUtility.ToJson(instance, Pretty(format));

            File.WriteAllText(settings, json);
        }

        /// <summary>
        /// Determines whether to indent or leave Converted Json alone.
        /// </summary>
        public enum Format
        {
            /// <summary>
            /// Indented Json, readable
            /// </summary>
            Indent = 0,
            /// <summary>
            /// Plain Json in one Line
            /// </summary>
            Line = 10
        }

        private static bool Pretty(Format mat)
        {
            switch (mat)
            {
                case Format.Indent:
                    return true;
                case Format.Line:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// <see cref="File.Exists(string)"/>
        /// </summary>
        /// <param name="path">Path to your file</param>
        /// <returns></returns>
        public static bool Exist(string path)
        {
            return File.Exists(path);
        }

        private static bool FileExistInternal(string path)
        {
            bool ret = Exist(path);

            if (!ret)
                Debug.Logger.LogWarning($"File does not exist {path}");

            return ret;
        }
    }
}
