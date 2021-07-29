using Dungeonator;

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
        public delegate void GunChange(PlayerController player, ref Gun previous, ref Gun current, Gun previousSecondary, Gun currentSecondary, bool newGun);
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
        public delegate void PlayerDidDamage(PlayerController player, float dmg, bool wasFatal, HealthHaver enemy);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public delegate void PlayerLostArmor(PlayerController player);
    }


}
