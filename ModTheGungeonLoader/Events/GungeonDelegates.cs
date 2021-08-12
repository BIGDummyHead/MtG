using Dungeonator;
using HarmonyLib;
using System;
using UnityEngine;

namespace Gungeon.Events
{
    /// <summary>
    /// Delegates for events that happen in Enter the Gungeon.
    /// </summary>
    public class GungeonDelegates
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="previous"></param>
        /// <param name="current"></param>
        /// <param name="previousSecondary"></param>
        /// <param name="currentSecondary"></param>
        /// <param name="newGun"></param>
        public delegate void GunChange(PlayerController player, Gun previous, Gun current, Gun previousSecondary, Gun currentSecondary, bool newGun);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public delegate void RoomClear(PlayerController player);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="room"></param>
        public delegate void RoomEnter(PlayerController player, RoomHandler room);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="dmg"></param>
        /// <param name="wasFatal"></param>
        /// <param name="enemy"></param>
        public delegate void PlayerDidDamage(PlayerController player, ref float dmg, ref bool wasFatal, HealthHaver enemy);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public delegate void PlayerLostArmor(PlayerController player);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="gun"></param>
        /// <param name="makeActive"></param>
        public delegate void GunAddedToInventory(GunInventory inventory, Gun gun, ref bool makeActive);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="resultValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="damangeTypes"></param>
        /// <param name="damageCategory"></param>
        /// <param name="damageDirection"></param>
        /// <returns></returns>
        public delegate void PlayerDamaged(PlayerController player, float resultValue, float maxValue, CoreDamageTypes damangeTypes, DamageCategory damageCategory, Vector2 damageDirection);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="belongsToPlayer"></param>
        public delegate void OnProjectileShot(Projectile projectile, bool belongsToPlayer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="belongsToPlayer"></param>
        public delegate void WhileMoving(Projectile projectile, bool belongsToPlayer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="lcr"></param>
        /// <param name="allowActorSpawns"></param>
        /// <param name="allowProjectileSpawns"></param>
        /// <param name="belongsToPlayer"></param>
        public delegate void OnProjectileHit(Projectile projectile, CollisionData lcr, bool belongsToPlayer, ref bool allowActorSpawns, ref bool allowProjectileSpawns);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="finalDamageDirection"></param> 
        public delegate void OnEnemyDied(AIActor enemy, Vector2 finalDamageDirection);


        /// <summary>
        /// For objects that can be picked up.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="pickUpUser"></param>
        public delegate void OnPickup<T>(T obj, PlayerController pickUpUser) where T : PickupObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gun"></param>
        /// <param name="overrideData"></param>
        /// <param name="overrideBullet"></param>
        /// <param name="__result"/>
        /// <param name="isPlayer"/>
        public delegate void OnGunAttack(Gun gun, bool isPlayer, ref Gun.AttackResult __result, ref ProjectileData overrideData, ref GameObject overrideBullet);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gun"></param>
        /// <param name="dropHeight"></param>
        /// <param name="droppedItem"></param>
        public delegate void OnGunDrop(Gun gun, ref float dropHeight, DebrisObject droppedItem);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        /// <param name="droppedItem"></param>
        public delegate void OnPassiveDrop(PassiveItem item, PlayerController player, DebrisObject droppedItem);
    }

    

}
