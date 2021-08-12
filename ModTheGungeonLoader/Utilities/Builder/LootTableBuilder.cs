using Gungeon.Debug;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Gungeon.Utilities
{
    /// <summary>
    /// A builder for Loot Tables.
    /// </summary>
    public sealed class LootTableBuilder
    {
        /// <summary>
        /// Create an instance for your table
        /// </summary>
        /// <param name="include"></param>
        /// <param name="prerequisites"></param>
        public LootTableBuilder(List<GenericLootTable> include = null, params DungeonPrerequisite[] prerequisites)
        {
            FinalTable = ScriptableObject.CreateInstance<GenericLootTable>();
            FinalTable.includedLootTables = include ?? new List<GenericLootTable>();
            FinalTable.defaultItemDrops = new WeightedGameObjectCollection
            {
                elements = new List<WeightedGameObject>()
            };
            FinalTable.tablePrerequisites = prerequisites;
        }

        /// <summary>
        /// Pool an item from <see cref="PickupIDs.GetItem(string, bool)"/>
        /// </summary>
        /// <param name="name">The display name</param>
        /// <param name="encounterName">Find by encounter name?</param>
        /// <param name="weight">Object's weight in table</param>
        /// <param name="prerequisites">Any prerequisites for this item?</param>
        /// <returns>The item added to the pool</returns>
        public WeightedGameObject PoolItem(string name, bool encounterName = false, float weight = 1, params DungeonPrerequisite[] prerequisites)
        {
            PickupObject pickup = PickupIDs.GetItem(name, encounterName);

            if (pickup == null)
            {
                "The object you would like to pool does not exist.".LogError();
                return null;
            }

            WeightedGameObject o = ToWeighted(pickup, weight, prerequisites);


            FinalTable.defaultItemDrops.Add(o);
            return o;
        }

        /// <summary>
        /// Pool an item from <see cref="PickupIDs.GetItem(int)"/>
        /// </summary>
        /// <param name="id">The object ID</param>
        /// <param name="weight">Object's weight in table</param>
        /// <param name="prerequisites">Any prerequisites for this item?</param>
        /// <returns>The item added to the pool</returns>
        public WeightedGameObject PoolItem(int id, float weight = 1, params DungeonPrerequisite[] prerequisites)
        {
            PickupObject pickup = PickupIDs.GetItem(id);

            if (pickup == null)
            {
                "The object you would like to pool does not exist.".LogInternal(Assembly.GetCallingAssembly(), Debug.Logger.LogTypes.error);
                return null;
            }

            WeightedGameObject o = ToWeighted(pickup, weight, prerequisites);
            FinalTable.defaultItemDrops.Add(o);
            return o;
        }

        /// <summary>
        /// Pool <see cref="WeightedGameObject"/>
        /// </summary>
        /// <param name="pickup"></param>
        /// <returns></returns>
        public WeightedGameObject PoolItem(WeightedGameObject pickup)
        {
            if(pickup == null)
            {
                "The object you would like to pool is null".LogInternal(Assembly.GetCallingAssembly(), Debug.Logger.LogTypes.error);
                return null;
            }    

            FinalTable.defaultItemDrops.Add(pickup);
            return pickup;
        }

        /// <summary>
        /// Pool <see cref="PickupObject"/>
        /// </summary>
        /// <param name="pickup"></param>
        /// <param name="weight"></param>
        /// <param name="prerequisites"></param>
        /// <returns></returns>
        public WeightedGameObject PoolItem(PickupObject pickup, float weight = 1, params DungeonPrerequisite[] prerequisites)
        {
            if (pickup == null)
            {
                "The object you would like to pool is null".LogInternal(Assembly.GetCallingAssembly(), Debug.Logger.LogTypes.error);
                return null;
            }

            WeightedGameObject o = ToWeighted(pickup, weight, prerequisites);

            return PoolItem(o);
        }

        /// <summary>
        /// Remove a <see cref="WeightedGameObject"/> from your loot table
        /// </summary>
        /// <param name="weighted">Pooled Item</param>
        public void UnpoolItem(WeightedGameObject weighted)
        {
            if (FinalTable.defaultItemDrops.elements.Contains(weighted))
            {
                FinalTable.defaultItemDrops.elements.Remove(weighted);
            }
        }

        /// <summary>
        /// Change a <see cref="PickupObject"/> to a <seealso cref="WeightedGameObject"/>
        /// </summary>
        /// <param name="pickup"></param>
        /// <param name="weight"></param>
        /// <param name="prerequisites"></param>
        /// <returns></returns>
        public static WeightedGameObject ToWeighted(PickupObject pickup, float weight = 1, params DungeonPrerequisite[] prerequisites)
        {
            return new WeightedGameObject()
            {
                pickupId = pickup.PickupObjectId,
                weight = weight,
                forceDuplicatesPossible = false,
                additionalPrerequisites = prerequisites,
                rawGameObject = pickup.gameObject
            };
        }

        /// <summary>
        /// A Table setup all for you!
        /// </summary>
        public GenericLootTable FinalTable { get; }

        /// <summary>
        /// Convert to builder
        /// </summary>
        /// <param name="builder"></param>

        public static explicit operator GenericLootTable(LootTableBuilder builder) => builder.FinalTable;
    }
}
