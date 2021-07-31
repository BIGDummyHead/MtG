using Dungeonator;
using Gungeon.Debug;
using Gungeon.Events;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            return Sprite.Create(texture, rect,pivot );
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
        /// Create a chest at a position.
        /// </summary>
        /// <param name="prefab"><see cref="ChestPrefabs"/></param>
        /// <param name="vec">Position at which to spawn the chest, <see cref="Convert(Vector2)"/></param>
        /// <param name="target">Can be left null, target room to spawn chest</param>
        /// <param name="forceMimicChest">Force this chest to be a mimic?</param>
        /// <returns></returns>
        public static Chest CreateChest(Chest prefab, IntVector2 vec, RoomHandler target = null, bool forceMimicChest = false)
        {
            Chest spawned = (target == null) ? Chest.Spawn(prefab, vec) : Chest.Spawn(prefab, vec, target);

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
        /// Get an item by name | <see cref="StringComparison.OrdinalIgnoreCase"/> is used.
        /// </summary>
        /// <param name="name">Item's name</param>
        /// <param name="byDisplayName">Use display name</param>
        /// <returns></returns>
        public static PickupObject GetItem(string name, bool byDisplayName = false)
        {
            return byDisplayName ? PickupObjectDatabase.GetByEncounterName(name) : PickupObjectDatabase.GetByName(name);
        }

        /// <summary>
        /// Get an item via ID.
        /// </summary>
        /// <param name="id">Object ID</param>
        /// <returns></returns>
        public static PickupObject GetItem(int id)
        {
            return PickupObjectDatabase.GetById(id);
        }

        /// <summary>
        /// Get an ID via a object.
        /// </summary>
        /// <param name="pickup">Object</param>
        /// <returns></returns>
        public static int ID(this PickupObject pickup)
        {
            return PickupObjectDatabase.GetId(pickup);
        }

        /// <summary>
        /// Every <see cref="PickupObject"/> in game, can be added to.
        /// </summary>
        public static List<PickupObject> AllObjects => PickupObjectDatabase.Instance?.Objects;

        /// <summary>
        /// A completely random gun, every single time
        /// </summary>
        public static Gun RandomGun => PickupObjectDatabase.GetRandomGun();

        /// <summary>
        /// Get a random passive.
        /// </summary>
        public static PassiveItem RandomPassive
        {
            get
            {
                List<PassiveItem> passives = new List<PassiveItem>((IEnumerable<PassiveItem>)AllObjects.Where(x => x is PassiveItem));

                int pick = new System.Random().Next(0, passives.Count);
                
                return passives[pick];
            }
        }

        /// <summary>
        /// Get a random gun of quality.
        /// </summary>
        /// <param name="excludeIDs">Ids to exclude</param>
        /// <param name="qualities">Gun qualities</param>
        /// <returns></returns>
        public static Gun GetRandomGunOfQuality(List<int> excludeIDs, params PickupObject.ItemQuality[] qualities)
        {
            System.Random ran = new System.Random();
            return PickupObjectDatabase.GetRandomGunOfQualities(ran, excludeIDs, qualities);
        }

        /// <summary>
        /// Get a random passive item of quality
        /// </summary>
        /// <param name="excludeIDs"></param>
        /// <param name="qualities"></param>
        /// <returns></returns>
        public static PassiveItem GetRandomPassiveOfQuality(List<int> excludeIDs, params PickupObject.ItemQuality[] qualities)
        {
            System.Random ran = new System.Random();
            return PickupObjectDatabase.GetRandomPassiveOfQualities(ran, excludeIDs, qualities);
           
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

        /// <summary>
        /// Before a projectile hits
        /// </summary>
        public static event GungeonDelegates.OnProjectileHit BeforeProjectileHit;

        /// <summary>
        /// After a projectile hits
        /// </summary>
        public static event GungeonDelegates.OnProjectileHit AfterProjectileHit;

        /// <summary>
        /// Before a projectile is shot
        /// </summary>
        public static event GungeonDelegates.OnProjectileShot BeforeProjectileShot;
        /// <summary>
        /// After a projectile is shot
        /// </summary>
        public static event GungeonDelegates.OnProjectileShot AfterProjectileShot;

        /// <summary>
        /// Called while a bullet is moving.
        /// </summary>
        public static event GungeonDelegates.WhileMoving BeforeMoving;
        /// <summary>
        /// Called while a bullet is moving.
        /// </summary>
        public static event GungeonDelegates.WhileMoving AfterMoving;


        [HarmonyPatch(typeof(Projectile), "HandleDestruction", typeof(CollisionData), typeof(bool), typeof(bool))]
        internal class _projHit
        {
            public static void Prefix(Projectile __instance, out bool __state, CollisionData lcr, ref bool allowActorSpawns, ref bool allowProjectileSpawns)
            {
                __state = __instance.Owner is PlayerController;
                BeforeProjectileHit?.Invoke(__instance, lcr, __state, ref allowActorSpawns, ref allowProjectileSpawns);
            }

            public static void Postfix(Projectile __instance, bool __state, CollisionData lcr, ref bool allowActorSpawns, ref bool allowProjectileSpawns)
            {
                AfterProjectileHit?.Invoke(__instance, lcr, __state, ref allowActorSpawns, ref allowProjectileSpawns);
            }
        }

        [HarmonyPatch(typeof(Projectile), "Move")]
        internal class _projShot
        {
            public static void Prefix(Projectile __instance, out bool __state)
            {
                __state = __instance.Owner is PlayerController;

                BeforeMoving?.Invoke(__instance, __state);
            }

            public static void Postfix(Projectile __instance, bool __state)
            {
               AfterMoving?.Invoke(__instance, __state);
            }
        }

        [HarmonyPatch(typeof(Projectile), "Start")]
        internal class _projMoving
        {
            public static void Prefix(Projectile __instance, out bool __state)
            {
                __state = __instance.Owner is PlayerController;

                BeforeProjectileShot?.Invoke(__instance, __state);
            }

            public static void Postfix(Projectile __instance, bool __state)
            {
                AfterProjectileShot?.Invoke(__instance, __state);
            }
        }

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
