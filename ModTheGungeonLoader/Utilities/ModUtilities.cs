using Dungeonator;
using Gungeon.Debug;
using Gungeon.Events;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Utilities to help with your mod.
    /// </summary>
    public static class ModUtilities
    {
        /// <summary>
        /// Load plenty of assets in one go
        /// </summary>
        /// <typeparam name="T"><see cref="Object"/></typeparam>
        /// <param name="assetPath">The path to the asset</param>
        /// <param name="assetNames">The asset names, can be empty. Uses all asset names</param>
        /// <returns>An array of Assets</returns>
        public static T[] LoadAssetArray<T>(string assetPath, params string[] assetNames) where T : Object
        {
            if (!HandleExist(assetPath))
                return new T[0];

            AssetBundle bundle = AssetBundle.LoadFromFile(assetPath);

            if (assetNames.Length < 1)
                assetNames = bundle.GetAllAssetNames();

            T[] assets = new T[assetNames.Length];
            for (int i = 0; i < assetNames.Length; i++)
            {
                assets[i] = bundle.LoadAsset<T>(assetNames[i]);
            }

            return assets;
        }

        /// <summary>
        /// Load a single Asset
        /// </summary>
        /// <typeparam name="T"><see cref="Object"/></typeparam>
        /// <param name="assetPath">The path to the asset</param>
        /// <param name="assetName">The asset name, can be null. Uses the first asset</param>
        /// <returns></returns>
        public static T LoadAsset<T>(string assetPath, string assetName = null) where T : Object
        {
            var loaded = LoadAssetArray<T>(assetPath, assetName);

            if (loaded.Length < 1)
                return default;

            return loaded[0];
        }

        /// <summary>
        /// Load an audio clip from a file. .MP3/.MP2/.OGG/.WAV supported.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static AudioClip LoadAudio(string file)
        {
            if (!HandleExist(file))
                return null;

            string ext = Path.GetExtension(file);
            var type = GetAudioType(ext);

            if (type == AudioType.UNKNOWN)
            {
                "Audio type is unknown and won't work with Unity".LogError();
                return null;
            }

            using (UnityWebRequest loadedAudio = UnityWebRequestMultimedia.GetAudioClip(file, type))
            {
                loadedAudio.SendWebRequest();

                while (!loadedAudio.isDone)
                {
                    //do nothing until done
                }

                "Finished Loading Audio...".Log(ConsoleColor.Green);

                if (!string.IsNullOrEmpty(loadedAudio.error))
                {
                    $"{loadedAudio.error}".LogError();
                    return null;
                }

                "Audio successfully loaded".Log(ConsoleColor.Green);
                return DownloadHandlerAudioClip.GetContent(loadedAudio);
            }
        }

        /// <summary>
        /// Load either a .jpg or .png
        /// </summary>
        /// <param name="file">JPG OR PNG</param>
        /// <param name="width">Def. 500</param>
        /// <param name="height">Def. 500</param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string file, int width = 500, int height = 500)
        {
            if (!HandleExist(file))
                return null;

            string ext = Path.GetExtension(file);

            if (ext != ".jpg" || ext != ".png")
            {
                "Your image may only be a '.jpg' or '.png'".LogError();
                return null;
            }

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            if (ImageConversion.LoadImage(texture, File.ReadAllBytes(file)))
            {
                "Texture successfully loaded".Log(ConsoleColor.Green);
                return texture;
            }

            "Texture failed to load!".LogError();
            return null;
        }

        static AudioType GetAudioType(string ext)
        {
            switch (ext)
            {
                case ".mp3":
                    return AudioType.MPEG;
                case ".mp2":
                    return AudioType.MPEG;
                case ".wav":
                    return AudioType.WAV;
                case ".ogg":
                    return AudioType.OGGVORBIS;
                default:
                    return AudioType.UNKNOWN;
            }

        }

        static bool HandleExist(string path)
        {
            bool a = File.Exists(path);

            if (!a)
                $"{path} does not exist".LogError();

            return a;
        }

        static bool ToBool(this bool? pass)
        {
            return pass ?? false;
        }

        /// <summary>
        /// Before a player changes their gun. 
        /// </summary>
        public static event GungeonDelegates.GunChange BeforeGunChange;
        /// <summary>
        /// After a player changes their gun. 
        /// </summary>
        public static event GungeonDelegates.GunChange AfterGunChange;

        /// <summary>
        /// Before a player clears a room.
        /// </summary>
        public static event GungeonDelegates.RoomClear BeforeRoomClear;

        /// <summary>
        /// After a player clears a room.
        /// </summary>
        public static event GungeonDelegates.RoomClear AfterRoomClear;

        /// <summary>
        /// Before a player enters a new room.  
        /// </summary>
        public static event GungeonDelegates.RoomEnter BeforeRoomEnter;
        /// <summary>
        /// After a player enters a new room.
        /// </summary>
        public static event GungeonDelegates.RoomEnter AfterRoomEnter;

        /// <summary>
        /// Before a player hits a <see cref="HealthHaver"/> 
        /// </summary>
        public static event GungeonDelegates.PlayerDidDamage BeforePlayerDoesDamage;
        /// <summary>
        /// After a player hits a <see cref="HealthHaver"/>.  
        /// </summary>
        public static event GungeonDelegates.PlayerDidDamage AfterPlayerDoesDamage;

        /// <summary>
        /// Before a player loses their armor 
        /// </summary>
        public static event GungeonDelegates.PlayerLostArmor BeforePlayerLostArmor;
        /// <summary>
        /// After a player loses their armor. 
        /// </summary>
        public static event GungeonDelegates.PlayerLostArmor AfterPlayerLostArmor;

        /// <summary>
        /// Before the player is damaged.
        /// </summary>
        public static event GungeonDelegates.PlayerDamaged BeforePlayerDamaged;

        /// <summary>
        /// After the player is damaged. 
        /// </summary>
        public static event GungeonDelegates.PlayerDamaged AfterPlayerDamaged;


        [HarmonyPatch(typeof(PlayerController), "OnGunChanged", typeof(Gun), typeof(Gun), typeof(Gun), typeof(Gun), typeof(bool))]
        internal class _gunchange
        {
            public static void Prefix(PlayerController __instance, Gun previous, Gun current, Gun previousSecondary, Gun currentSecondary, bool newGun)
            {
                BeforeGunChange?.Invoke(__instance, previous, current, previousSecondary, currentSecondary, newGun);
            }

            public static void Postfix(PlayerController __instance, Gun previous, Gun current, Gun previousSecondary, Gun currentSecondary, bool newGun)
            {
                AfterGunChange?.Invoke(__instance, previous, current, previousSecondary, currentSecondary, newGun);
            }
        }

        [HarmonyPatch(typeof(PlayerController), "OnRoomCleared")]
        internal class _roomclear
        {
            public static void Prefix(PlayerController __instance)
            {
                BeforeRoomClear?.Invoke(__instance);
            }

            public static void Postfix(PlayerController __instance)
            {
                AfterRoomClear?.Invoke(__instance);
            }
        }

        [HarmonyPatch(typeof(PlayerController), "EnteredNewRoom", typeof(RoomHandler))]
        internal class _roomEnter
        {
            public static void Prefix(PlayerController __instance, RoomHandler newRoom)
            {
                BeforeRoomEnter?.Invoke(__instance, newRoom);
            }

            public static void Postfix(PlayerController __instance, RoomHandler newRoom)
            {
                AfterRoomEnter?.Invoke(__instance, newRoom);
            }
        }

        [HarmonyPatch(typeof(PlayerController), "OnDidDamage", typeof(float), typeof(bool), typeof(HealthHaver))]
        internal class _didDamage
        {
            public static void Prefix(PlayerController __instance, ref float damageDone, ref bool fatal, HealthHaver target)
            {
                BeforePlayerDoesDamage?.Invoke(__instance, ref damageDone, ref fatal, target);
            }

            public static void Postfix(PlayerController __instance, ref float damageDone, ref bool fatal, HealthHaver target)
            {
                AfterPlayerDoesDamage?.Invoke(__instance, ref damageDone, ref fatal, target);
            }
        }

        [HarmonyPatch(typeof(PlayerController), "OnLostArmor")]
        internal class _lostArmor
        {
            public static void Prefix(PlayerController __instance)
            {
               BeforePlayerLostArmor?.Invoke(__instance);
            }

            public static void Postfix(PlayerController __instance)
            {
                AfterPlayerLostArmor?.Invoke(__instance);
            }
        }

        [HarmonyPatch(typeof(PlayerController), "Damaged", typeof(float), typeof(float), typeof(CoreDamageTypes), typeof(DamageCategory), typeof(Vector2))]
        internal class _plyerDmg
        {
            public static void Prefix(PlayerController __instance, float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
            {
                 BeforePlayerDamaged?.Invoke(__instance, resultValue, maxValue, damageTypes, damageCategory, damageDirection);
            }

            public static void Postfix(PlayerController __instance, float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
            {
                AfterPlayerDamaged?.Invoke(__instance, resultValue, maxValue, damageTypes, damageCategory, damageDirection);
            }
        }
    }
}
