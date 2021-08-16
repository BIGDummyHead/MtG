using Gungeon.Debug;
using System;
using System.Reflection;
using static Gungeon.Events.GameEvents;

namespace Gungeon.Utilities
{
    /// <summary>
    /// For modifying weaponry
    /// </summary>
    public abstract class BaseGunModifier : BasePickupModifier
    {
        /// <summary>
        /// The gun to modify.
        /// </summary>
        protected Gun UserGun { get; private set; }

        /// <summary>
        /// Is this weapon being held?
        /// </summary>
        public bool BeingHeld => UserGun.CurrentOwner != null;

        /// <summary>
        /// The owner of the current weapon
        /// </summary>
        /// <remarks>May be null if <see cref="BeingHeld"/> is false or if the owner is not of the <seealso cref="PlayerController"/> type</remarks>
        public PlayerController Owner
        {
            get
            {
                if (!BeingHeld)
                    return null;
                else if (!(UserGun.CurrentOwner is PlayerController))
                    return null;


                return UserGun.CurrentOwner as PlayerController;
            }
        }

        void Awake()
        {
            UserGun = GetComponent<Gun>() ?? throw new Exception("Gun may not be null and may only be applied to gun objects.");
            AfterProjectileShot += BefGunSht;
            AfterGunPickup += BefGunPick;
            AfterGunDrop += BefGunDrp;
            UserGun.OnReloadPressed += ManRel;
        }


        void Undo()
        {
            UserGun = GetComponent<Gun>() ?? throw new Exception("Gun may not be null and may only be applied to gun objects.");
            AfterProjectileShot -= BefGunSht;
            AfterGunPickup -= BefGunPick;
            AfterGunDrop -= BefGunDrp;
            UserGun.OnReloadPressed -= ManRel;
        }

        void ManRel(PlayerController player, Gun gun, bool a)
        {
            if (gun.ClipShotsRemaining < gun.ClipCapacity && gun.CurrentAmmo > 0)
                OnReload(gun, player);
        }

        private void BefGunDrp(Gun gun, ref float dropHeight, DebrisObject droppedItem)
        {
            if (!UserGun.SameItem(gun))
                return;

            z = droppedItem;
            OnDrop(gun, ref dropHeight);
            z = null;

            Undo();
        }

        private void BefGunPick(Gun obj, PlayerController pickUpUser)
        {
            if (UserGun == null)
            {
                "COULD NOT HANDLE USER GUN BEING PICKED UP!".LogInternal(Assembly.GetCallingAssembly(), Logger.LogTypes.error);
                return;
            }


            if (UserGun.SameItem(obj))
                OnPickup(obj, pickUpUser);
        }

        private void BefGunSht(Projectile projectile, bool belongsToPlayer)
        {
            if (projectile.PossibleSourceGun.SameItem(UserGun) && belongsToPlayer)
                OnFire(projectile.PossibleSourceGun, projectile);
        }

        private DebrisObject z = null;

        /// <summary>
        /// When the gun is dropped
        /// </summary>
        /// <param name="gun"></param>
        /// <param name="dropHeight"></param>
        /// <returns></returns>
        public virtual DebrisObject OnDrop(Gun gun, ref float dropHeight)
        {
            return z;
        }

        /// <summary>
        /// When the gun is picked up.
        /// </summary>
        /// <param name="gun"></param>
        /// <param name="player"></param>
        public virtual void OnPickup(Gun gun, PlayerController player)
        {

        }

        /// <summary>
        /// When the weapon is fired, only by the player.
        /// </summary>
        /// <param name="gun"></param>
        /// <param name="proj"></param>
        public virtual void OnFire(Gun gun, Projectile proj)
        {

        }
        /// <summary>
        /// When the gun is reloaded, manually or automatically.
        /// </summary>
        /// <param name="gun"></param>
        /// <param name="owner"></param>
        public virtual void OnReload(Gun gun, PlayerController owner)
        {

        }


    }
}
