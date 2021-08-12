using Dungeonator;
using Gungeon.Debug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

        /// <summary>
        /// Convert a <see cref="Texture2D"/> to a <seealso cref="Sprite"/>
        /// </summary>
        /// <param name="texture">A loaded texture</param>
        /// <param name="pos">The position of the sprite.</param>
        /// <param name="pivot">Sprite's pivot point relative position and size | <seealso cref="Rect"/></param>
        /// <returns></returns>
        public static Sprite ToSprite(this Texture2D texture, Vector2 pos, Vector2 pivot)
        {
            Rect rect = new Rect(pos, new Vector2(texture.width, texture.height));

            return Sprite.Create(texture, rect, pivot);
        }

        /// <summary>
        /// Rounds x and y from your vector into ints then converts to <see cref="IntVector2"/>
        /// </summary>
        /// <param name="from">Vector</param>
        /// <returns></returns>
        public static IntVector2 Convert(this Vector2 from)
        {
            int x = Mathf.RoundToInt(from.x);
            int y = Mathf.RoundToInt(from.y);

            return new IntVector2(x, y);
        }

        /// <summary>
        /// Convert from <see cref="Vector3"/> to <seealso cref="IntVector2"/>
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static IntVector2 Convert(this Vector3 from)
        {
            return ((Vector2)from).Convert();
        }

        /// <summary>
        /// Create a chest at a position.
        /// </summary>
        /// <param name="prefab"><see cref="ChestPrefabs"/></param>
        /// <param name="vec">Position at which to spawn the chest, <see cref="Convert(Vector2)"/></param>
        /// <param name="target">Can be left null, target room to spawn chest</param>
        /// <param name="forceMimicChest">Force this chest to be a mimic?</param>
        /// <param name="lootTables">Loot tables to include.</param>
        /// <param name="includeThisTable">Include the base loot table with the chest?</param>
        /// <returns></returns>
        public static Chest CreateChest(Chest prefab, IntVector2 vec, RoomHandler target = null, bool forceMimicChest = false, bool includeThisTable = true, params GenericLootTable[] lootTables)
        {
            Chest spawned = (target == null) ? Chest.Spawn(prefab, vec) : Chest.Spawn(prefab, vec, target);

            List<GenericLootTable> tables = new List<GenericLootTable>();

            if (includeThisTable && spawned.lootTable != null)
                tables.Add(spawned.lootTable.lootTable);

            if (lootTables.Length > 0)
                tables.AddRange(tables);

            if (lootTables.Length > 0)
            {
                if (!includeThisTable)
                {
                    spawned.lootTable.lootTable = lootTables[0];
                    tables.RemoveAt(0);
                }

                spawned.lootTable.overrideItemLootTables = tables;
            }

            if (forceMimicChest)
            {
                spawned.GetVariable("m_isMimic").SetValue(true);
            }

            return spawned;
        }

        /// <summary>
        /// Spawn currency at a position
        /// </summary>
        /// <param name="position">Position to spawn currency</param>
        /// <param name="amount">Amount of money</param>
        public static void SpawnCurrency(Vector2 position, int amount)
        {
            LootEngine.SpawnCurrencyManual(position, amount);
        }



        /// <summary>
        /// Spawn a heart at a position
        /// </summary>
        /// <param name="position">Position to spawn hearts</param>
        /// <param name="halfHearts">For every 2 a full heart is created</param>
        public static void SpawnHearts(Vector2 position, int halfHearts)
        {
            LootEngine.SpawnHealth(position, halfHearts, null);
        }

        /// <summary>
        /// Spawn an Item, uses <see cref="LootEngine"/> internal spawning method.
        /// </summary>
        /// <param name="toSpawn">Obj to spawn</param>
        /// <param name="spawnPos">Where to spawn</param>
        /// <param name="spawnDir">The direction to spawn, default of <seealso cref="Vector2.up"/></param>
        /// <param name="force">Force</param>
        /// <param name="invalidTillHitGround">Invalid till object hits ground?</param>
        /// <param name="doDefaultItemPoof">Poof?</param>
        /// <param name="disablePostProcessing">Disable post processing?</param>
        /// <param name="disableHeightBoost">Disable height boost?</param>
        public static DebrisObject SpawnItem(GameObject toSpawn, Vector3 spawnPos, Vector2? spawnDir = null, float force = 4, bool invalidTillHitGround = true, bool doDefaultItemPoof = false, bool disablePostProcessing = false, bool disableHeightBoost = false)
        {
            var method = typeof(LootEngine).GetStaticMethod("SpawnInternal", typeof(GameObject), typeof(Vector3), typeof(Vector2), typeof(float), typeof(bool), typeof(bool), typeof(bool), typeof(bool));

            Vector2 dir = spawnDir == null ? Vector2.up : spawnDir.Value;

            return (DebrisObject)method.Invoke(toSpawn, spawnPos, dir, force, invalidTillHitGround, doDefaultItemPoof, disablePostProcessing, disableHeightBoost);
        }

        /// <summary>
        /// Reload a gun
        /// </summary>
        /// <param name="gun">Gun to reload</param>
        /// <param name="isActiveGun">Is the gun active?</param>
        /// <param name="silent">Silent reload?</param>
        /// <param name="immediate">Immediately reload gun?</param>
        public static void Reload(this Gun gun, bool isActiveGun = false, bool silent = false, bool immediate = false)
        {
            gun.GetMethod("FinishReload", typeof(bool), typeof(bool), typeof(bool)).Invoke(isActiveGun, silent, immediate);
        }




        /// <summary>
        /// Is the player in a Gungeon?
        /// </summary>
        public static bool IsInGungeon
        {
            get
            {
                if (CurrentPlayer == null || CurrentPlayer?.CurrentRoom == null)
                    return false;

                string roomName = CurrentPlayer.CurrentRoom.GetRoomName() ?? string.Empty;
                return !roomName.StartsWith("Gungeon_Foyer") || !roomName.StartsWith("Tutorial");
            }
        }



        /// <summary>
        /// Changes a base stat, uses <see cref="PlayerStats.StatType"/> and <seealso cref="StatChange"/> to calculate change in stats value
        /// </summary>
        /// <param name="player">The player's stats you want to change</param>
        /// <param name="statType">The type of stat you want to change</param>
        /// <param name="stat">The stat change effect</param>
        /// <param name="value">The value to apply to your <paramref name="stat"/></param>
        public static void ChangeStat(PlayerController player, PlayerStats.StatType statType, StatChange stat, float value)
        {
            if (player is null)
            {
                "Player may not be null when changing stat.".LogInternal(Assembly.GetCallingAssembly(), Debug.Logger.LogTypes.error);
                return;
            }

            PlayerStats stats = player.stats;

            if (stats is null)
            {
                "Stats of player may not be null when changing stat.".LogInternal(Assembly.GetCallingAssembly(), Debug.Logger.LogTypes.error);
                return;
            }

            int ent = (int)statType;

            float changed = GetStatC(stat, stats.BaseStatValues[ent], value);

            stats.SetBaseStatValue(statType, changed, player);
        }

        private static float GetStatC(StatChange change, float existing, float userChange)
        {
            switch (change)
            {
                case StatChange.Add:
                    return existing + userChange;
                case StatChange.Mult:
                    return existing * userChange;
                case StatChange.Sub:
                    return existing - userChange;
                case StatChange.Div:
                    return existing / userChange;
                case StatChange.Total:
                    return userChange;
                default:
                    return userChange;
            }
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

        /// <summary>
        /// All player's playing.
        /// </summary>
        public static PlayerController[] AllPlayers => GameManager.Instance.AllPlayers;

        /// <summary>
        /// The current player, most active.
        /// </summary>
        public static PlayerController CurrentPlayer => GameManager.Instance.PrimaryPlayer;
        /// <summary>
        /// The secondary player.
        /// </summary>
        public static PlayerController SecondaryPlayer => GameManager.Instance.SecondaryPlayer;

        /// <summary>
        /// The most active player
        /// </summary>
        public static PlayerController MostActivePlayer => GameManager.Instance.BestActivePlayer;



    }
}
