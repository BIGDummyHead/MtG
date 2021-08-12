using Dungeonator;
using Gungeon.Debug;
using Gungeon.Events;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <summary>
        /// Before a enemy has died
        /// </summary>
        public static event GungeonDelegates.OnEnemyDied BeforeEnemyDied;
        /// <summary>
        /// After a enemy has died
        /// </summary>
        public static event GungeonDelegates.OnEnemyDied AfterEnemyDied;

        /// <summary>
        /// Before any passive item in game is picked up by a player
        /// </summary>
        public static event GungeonDelegates.OnPickup<PassiveItem> BeforePassivePickup;
        /// <summary>
        /// After any passive item in game is picked up by a player
        /// </summary>
        public static event GungeonDelegates.OnPickup<PassiveItem> AfterPassivePickup;

        /// <summary>
        /// Before any gun is picked up
        /// </summary>
        public static event GungeonDelegates.OnPickup<Gun> BeforeGunPickup;
        /// <summary>
        /// After any gun is picked up
        /// </summary>
        public static event GungeonDelegates.OnPickup<Gun> AfterGunPickup;

        /// <summary>
        /// Before a gun attacks, from any source.
        /// </summary>
        public static event GungeonDelegates.OnGunAttack BeforeGunAttack;

        /// <summary>
        /// After a gun attacks, from any source.
        /// </summary>
        public static event GungeonDelegates.OnGunAttack AfterGunAttack;

        /// <summary>
        /// Before a passive is dropped
        /// </summary>
        public static event GungeonDelegates.OnPassiveDrop BeforePassiveDrop;
        /// <summary>
        /// After a passive is dropped
        /// </summary>
        public static event GungeonDelegates.OnPassiveDrop AfterPassiveDrop;

        /// <summary>
        /// Before a gun is dropped
        /// </summary>
        public static event GungeonDelegates.OnGunDrop BeforeGunDrop;
        /// <summary>
        /// After a gun is dropped
        /// </summary>
        public static event GungeonDelegates.OnGunDrop AfterGunDrop;




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

        [HarmonyPatch(typeof(AIActor), "PreDeath", typeof(Vector2))]
        internal class _enemyDeath
        {
            public static void Prefix(AIActor __instance, Vector2 finalDamageDirection)
            {
                BeforeEnemyDied?.Invoke(__instance, finalDamageDirection);
            }

            public static void Postfix(AIActor __instance, Vector2 finalDamageDirection)
            {
                AfterEnemyDied?.Invoke(__instance, finalDamageDirection);
            }
        }

        [HarmonyPatch(typeof(PassiveItem), "Pickup", typeof(PlayerController))]
        internal class _onPassivePicked
        {
            public static void Prefix(PassiveItem __instance, PlayerController player)
            {
                BeforePassivePickup?.Invoke(__instance, player);
            }

            public static void Postfix(PassiveItem __instance, PlayerController player)
            {
                AfterPassivePickup?.Invoke(__instance, player);
            }
        }

        [HarmonyPatch(typeof(Gun), "Pickup", typeof(PlayerController))]
        internal class _onGunPicked
        {
            public static void Prefix(Gun __instance, PlayerController player)
            {
                BeforeGunPickup?.Invoke(__instance, player);
            }

            public static void Postfix(Gun __instance, PlayerController player)
            {
                AfterGunPickup?.Invoke(__instance, player);
            }
        }

        [HarmonyPatch(typeof(Gun), nameof(Gun.DropGun), typeof(float))]
        internal class _onGunDrop
        {
            public static void Prefix(Gun __instance, DebrisObject __result, ref float dropHeight)
            {
                BeforeGunDrop?.Invoke(__instance, ref dropHeight, __result);
            }

            public static void Postfix(Gun __instance, DebrisObject __result, ref float dropHeight)
            {
                AfterGunDrop?.Invoke(__instance, ref dropHeight, __result);
            }
        }

        [HarmonyPatch(typeof(PassiveItem), nameof(PassiveItem.Drop), typeof(PlayerController))]
        internal class _onPassiveDrop
        {
            public static void Prefix(PassiveItem __instance, DebrisObject __result, PlayerController player)
            {
                BeforePassiveDrop?.Invoke(__instance, player, __result);
            }

            public static void Postfix(PassiveItem __instance, DebrisObject __result, PlayerController player)
            {
                AfterPassiveDrop?.Invoke(__instance, player, __result);
            }
        }

        [HarmonyPatch(typeof(Gun), "Attack", typeof(ProjectileData), typeof(GameObject))]
        internal class _onFire
        {
            public static void Prefix(Gun __instance, ref Gun.AttackResult __result, ref ProjectileData overrideProjectileData, ref GameObject overrideBulletObject)
            {
                BeforeGunAttack?.Invoke(__instance, __instance.CurrentOwner is PlayerController, ref __result, ref overrideProjectileData, ref overrideBulletObject);
            }

            public static void Postfix(Gun __instance, ref Gun.AttackResult __result, ref ProjectileData overrideProjectileData, ref GameObject overrideBulletObject)
            {
                AfterGunAttack?.Invoke(__instance, __instance.CurrentOwner is PlayerController, ref __result, ref overrideProjectileData, ref overrideBulletObject);
            }
        }
    }
}
