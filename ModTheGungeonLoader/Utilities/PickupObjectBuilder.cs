using Gungeon.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gungeon.Utilities
{
    /// <summary>
    /// An active item builder
    /// </summary>
    public class PickupObjectBuilder
    {
        //Structure.
        //Each method shall return on itself so item building may be done in one line.
        //Return types shall be saved inside the appropriate properties.


        public PickupObjectBuilder(GameObject obj, string name, bool instantiateObj = true)
        {
            this.obj = instantiateObj ? UnityEngine.Object.Instantiate(obj) : obj;

            m_item = obj.AddComponent<PickupObject>();
            m_item.quality = PickupObject.ItemQuality.D;
            m_item.name = name;
            SetItemName(name);
            this.name = name;
        }

        /// <summary>
        /// Set the protected Item Name - Display Name of your object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PickupObjectBuilder SetItemName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return this;

            Item?.GetVariable("itemName")?.SetValue(name);

            return this;
        }

        /// <summary>
        /// Add your object to the pool.
        /// </summary>
        /// <returns></returns>
        public PickupObjectBuilder Pool()
        {
            if (!Pooled)
                ModUtilities.AllObjects.Add(Item);

            _poolID = ModUtilities.AllObjects.Count;

            return this;
        }

        /// <summary>
        /// Remove your object from the pool.
        /// </summary>
        /// <returns></returns>
        public PickupObjectBuilder Unpool()
        {
            if (Pooled)
                ModUtilities.AllObjects.Remove(Item);

            return this;
        }

        /// <summary>
        /// Add your object to a loot table.
        /// </summary>
        /// <param name="lootTable"></param>
        /// <param name="weighted"></param>
        /// <returns></returns>
        public PickupObjectBuilder AddToTable(GenericLootTable lootTable, WeightedGameObject weighted = null)
        {
            if (!Pooled)
            {
                $"Item is not pooled, will not be added to table".LogWarning();
                return this;
            }

            weighted = weighted == null ? weighted : new WeightedGameObject
            {
                forceDuplicatesPossible = false,
                pickupId = PoolID,
                weight = 1,
                
            };

            weighted.rawGameObject = obj;

            lootTable.defaultItemDrops.Add(weighted);

            return this;
        }

        /// <summary>
        /// The Item you are working with
        /// </summary>
        public PickupObject Item => m_item;

        private int PoolID
        {
            get
            {
                return !Pooled ? -1 : _poolID;
            }
        }

        /// <summary>
        /// Has you object been pooled?
        /// </summary>
        public bool Pooled => ModUtilities.AllObjects.Contains(Item);

        private int _poolID;
        private string name;
        private GameObject obj;
        private PickupObject m_item;
    }
}
