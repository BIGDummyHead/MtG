using System;
using static Gungeon.Events.GameEvents;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Active item modifier
    /// </summary>
    public abstract class BaseActiveModifier : BasePickupModifier
    {
        /// <summary>
        /// The active item
        /// </summary>
        protected PlayerItem Active { get; private set; }
        void Awake()
        {
            Active = GetComponent<PlayerItem>() ?? throw new Exception("Passive item is null, please use this on objects with the passive item component.");
            BeforeActiveDrop += BefDrop;
            BeforeActivePickup += BefPick;
            BeforeActiveUse += BefUse;
        }

        private void BefUse(PlayerItem active, PlayerController user)
        {
            if (!Active.SameItem(active))
                return;

            OnUse(active, user);
        }

        private void BefPick(PlayerItem obj, PlayerController pickUpUser)
        {
            if (!Active.SameItem(obj))
                return;

            OnPickup(obj, pickUpUser);
        }

        private void BefDrop(PlayerItem item, PlayerController player, ref float overrideForce, DebrisObject dropped)
        {
            if (!Active.SameItem(item))
                return;

            z = dropped;
            OnDrop(item, player);
            z = null;

            BeforeActiveDrop -= BefDrop;
            BeforeActivePickup -= BefPick;
            BeforeActiveUse -= BefUse;
        }

        /// <summary>
        /// When the active item is picked up
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        public virtual void OnPickup(PlayerItem item, PlayerController player)
        {

        }

        /// <summary>
        /// When the active item is used
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        public virtual void OnUse(PlayerItem item, PlayerController player)
        {

        }

        /// <summary>
        /// When the active item is dropped.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        public virtual DebrisObject OnDrop(PlayerItem item, PlayerController player)
        {
            return z;
        }

        private DebrisObject z = null;
    }
}
