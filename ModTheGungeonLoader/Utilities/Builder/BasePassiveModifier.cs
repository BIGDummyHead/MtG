using System;
using static Gungeon.Events.GameEvents;

namespace Gungeon.Utilities
{
    /// <summary>
    /// Passive modifier
    /// </summary>
    public abstract class BasePassiveModifier : BasePickupModifier
    {
        /// <summary>
        /// The passive item
        /// </summary>
        protected PassiveItem Passive { get; private set; }
        void Awake()
        {
            Passive = GetComponent<PassiveItem>() ?? throw new Exception("Passive item is null, please use this on objects with the passive item component.");

            AfterPassiveDrop += BefDrop;
            AfterPassivePickup += BefPick;
        }

        private void BefPick(PassiveItem obj, PlayerController pickUpUser)
        {
            if (Passive?.PickupObjectId != obj?.PickupObjectId)
                return;

            OnPickup(obj, pickUpUser);
        }

        private void BefDrop(PassiveItem item, PlayerController player, DebrisObject o)
        {
            if (Passive?.PickupObjectId != item?.PickupObjectId)
                return;

            z = o;
            OnDrop(item, player);
            z = null;

            BeforePassiveDrop -= BefDrop;
            BeforePassivePickup -= BefPick;
        }

        /// <summary>
        /// When the passive item is picked up
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        public virtual void OnPickup(PassiveItem item, PlayerController player)
        {

        }

        /// <summary>
        /// When the passive item is dropped.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        public virtual DebrisObject OnDrop(PassiveItem item, PlayerController player)
        {
            return z;
        }

        private DebrisObject z = null;
    }
}
