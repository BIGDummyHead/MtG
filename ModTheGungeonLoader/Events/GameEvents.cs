using Dungeonator;
using HarmonyLib;
using UnityEngine;

namespace Gungeon.Events
{
    /// <summary>
    /// In game events.
    /// </summary>
    public static class GameEvents
    {
        /// <summary>
        /// The harmony patch for gungeon events.
        /// </summary>
        public static Harmony HarmonyPatch { get; private set; }
        internal static void Patch()
        {
            HarmonyPatch = new Harmony("com.Gungeon.Mods");
            HarmonyPatch.PatchAll();
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
        public static event GungeonDelegates.PlayerDidDamage BeforeEnemyDamaged;
        /// <summary>
        /// After a player hits a <see cref="HealthHaver"/>.  
        /// </summary>
        public static event GungeonDelegates.PlayerDidDamage AfterEnemyDamaged;

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
        public static event GungeonDelegates.OnItemPickup<PassiveItem> BeforePassivePickup;
        /// <summary>
        /// After any passive item in game is picked up by a player
        /// </summary>
        public static event GungeonDelegates.OnItemPickup<PassiveItem> AfterPassivePickup;

        /// <summary>
        /// Before any gun is picked up
        /// </summary>
        public static event GungeonDelegates.OnItemPickup<Gun> BeforeGunPickup;
        /// <summary>
        /// After any gun is picked up
        /// </summary>
        public static event GungeonDelegates.OnItemPickup<Gun> AfterGunPickup;

        /// <summary>
        /// Before any active is picked up.
        /// </summary>
        public static event GungeonDelegates.OnItemPickup<PlayerItem> BeforeActivePickup;
        /// <summary>
        /// After any active is picked up.
        /// </summary>
        public static event GungeonDelegates.OnItemPickup<PlayerItem> AfterActivePickup;

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

        /// <summary>
        /// Before an active item is dropped
        /// </summary>
        public static event GungeonDelegates.OnActiveDrop BeforeActiveDrop;
        /// <summary>
        /// After an active item is dropped
        /// </summary>
        public static event GungeonDelegates.OnActiveDrop AfterActiveDrop;

        [HarmonyPatch(typeof(PlayerItem), "Pickup", typeof(PlayerController))]
        internal class _playerItemPick
        {
            public static void Prefix(PlayerItem __instance, PlayerController player)
            {
                BeforeActivePickup?.Invoke(__instance, player);
            }

            public static void Postfix(PlayerItem __instance, PlayerController player)
            {
                AfterActivePickup?.Invoke(__instance, player);
            }
        }


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
                BeforeEnemyDamaged?.Invoke(__instance, ref damageDone, ref fatal, target);
            }

            public static void Postfix(PlayerController __instance, ref float damageDone, ref bool fatal, HealthHaver target)
            {
                AfterEnemyDamaged?.Invoke(__instance, ref damageDone, ref fatal, target);
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

        [HarmonyPatch(typeof(PlayerItem), "Drop", typeof(PlayerController), typeof(float))]
        internal class _playerItemDrop
        {
            public static void Prefix(PlayerItem __instance, DebrisObject __result, PlayerController player, ref float overrideForce)
            {
                BeforeActiveDrop?.Invoke(__instance, player, ref overrideForce, __result);
            }

            public static void Postfix(PlayerItem __instance, DebrisObject __result, PlayerController player, ref float overrideForce)
            {
                AfterActiveDrop?.Invoke(__instance, player, ref overrideForce, __result);
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
