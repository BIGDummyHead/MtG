using Gungeon.Debug;
using System.Collections.Generic;
using System.Reflection;

namespace Gungeon.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class JournalUtilities
    {

        /// <summary>
        /// Get key for full entry in ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetFullEntry(this PickupObject entry)
        {
            return entry.encounterTrackable.journalData.GetFullEntry();
        }
        /// <summary>
        /// Get key for full entry in ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetFullEntry(this JournalEntry entry)
        {
            return GetItem(entry.AmmonomiconFullEntry);
        }
        /// <summary>
        /// Get key for full entry in ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetFullEntry(this EncounterTrackable entry)
        {
            return entry.journalData.GetFullEntry();
        }


        /// <summary>
        /// Get key for display name
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetDisplayName(this JournalEntry entry)
        {
            return GetItem(entry.PrimaryDisplayName);
        }
        /// <summary>
        /// Get key for display name
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetDisplayName(this EncounterTrackable entry)
        {
            return entry.journalData.GetDisplayName();
        }
        /// <summary>
        /// Get key for display name
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetDisplayName(this PickupObject entry)
        {
            return entry.encounterTrackable.journalData.GetDisplayName();
        }


        /// <summary>
        /// Get key for notification display name
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetNotificationName(this JournalEntry entry)
        {
            return GetItem(entry.NotificationPanelDescription);
        }
        /// <summary>
        /// Get key for notification display name
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetNotificationName(this EncounterTrackable entry)
        {
            return entry.journalData.GetNotificationName();
        }
        /// <summary>
        /// Get key for notification display name
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetNotificationName(this PickupObject entry)
        {
            return entry.encounterTrackable.journalData.GetNotificationName();
        }

        /// <summary>
        /// Get key for sprite
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetSprite(this JournalEntry entry)
        {
            return GetItem(entry.AmmonomiconSprite);
        }
        /// <summary>
        /// Get key for sprite
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetSprite(this EncounterTrackable entry)
        {
            return entry.journalData.GetSprite();
        }
        /// <summary>
        /// Get key for sprite
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static string GetSprite(this PickupObject entry)
        {
            return entry.encounterTrackable.journalData.GetSprite();
        }


        /// <summary>
        /// Set the display name of a <see cref="JournalEntry"/> in the ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="display"></param>
        public static void SetDisplayName(this JournalEntry entry, string display)
        {
            SetItem(entry.PrimaryDisplayName, display);
        }
        /// <summary>
        /// Set the display name of a <see cref="JournalEntry"/> in the ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="display"></param>
        public static void SetDisplayName(this EncounterTrackable entry, string display)
        {
            SetItem(entry?.journalData?.PrimaryDisplayName, display);
        }
        /// <summary>
        /// Set the display name of a <see cref="JournalEntry"/> in the ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="display"></param>
        public static void SetDisplayName(this PickupObject entry, string display)
        {
            SetItem(entry?.encounterTrackable?.journalData?.PrimaryDisplayName, display);
        }


        /// <summary>
        /// Set the notification display name.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="name"></param>
        public static void SetNotificationName(this JournalEntry entry, string name)
        {
            SetItem(entry?.NotificationPanelDescription, name);
        }
        /// <summary>
        /// Set the notification display name.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="name"></param>
        public static void SetNotificationName(this EncounterTrackable entry, string name)
        {
            SetItem(entry?.journalData?.NotificationPanelDescription, name);
        }
        /// <summary>
        /// Set the notification display name.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="name"></param>
        public static void SetNotificationName(this PickupObject entry, string name)
        {
            SetItem(entry?.encounterTrackable?.journalData?.NotificationPanelDescription, name);
        }


        /// <summary>
        /// Set the sprite of the ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="resource"></param>
        public static void SetSprite(this JournalEntry entry, string resource)
        {
            SetItem(entry?.AmmonomiconSprite, resource);
        }
        /// <summary>
        /// Set the sprite of the ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="resource"></param>
        public static void SetSprite(this EncounterTrackable entry, string resource)
        {
            SetItem(entry?.journalData?.AmmonomiconSprite, resource);
        }
        /// <summary>
        /// Set the sprite of the ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="resource"></param>
        public static void SetSprite(this PickupObject entry, string resource)
        {
            SetItem(entry?.encounterTrackable?.journalData?.AmmonomiconSprite, resource);
        }


        /// <summary>
        /// Get table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static Dictionary<string, StringTableManager.StringCollection> GetTable(Table table)
        {
            switch (table)
            {
                case Table.Core:
                    return StringTableManager.CoreTable;
                case Table.Item:
                    return StringTableManager.ItemTable;
                case Table.Intro:
                    return StringTableManager.IntroTable;
                case Table.Enemy:
                    return StringTableManager.EnemyTable;
            }

            return StringTableManager.ItemTable;
        }

        /// <summary>
        /// Add a value to a table, for later use.
        /// </summary>
        /// <param name="table">Table to add to</param>
        /// <param name="key">Key value</param>
        /// <param name="value">Value</param>
        public static void AddTo(Table table, string key, string value)
        {
            if (KeyNull(key))
                return;

            var _table = GetTable(table);

            if (_table.ContainsKey(key))
            {
                $"'{key}' is already present in {table}".LogInternal(System.Reflection.Assembly.GetCallingAssembly(), Logger.LogTypes.error);
                return;
            }

            _table.Add(key, (ImplicitStringCollection)value);
        }


        /// <summary>
        /// Change an item's value from the key. The table is automatically determined via the key provided.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetItem(string key, string value)
        {
            if (KeyNull(key))
                return;
            Dictionary<string, StringTableManager.StringCollection> table = GetTable(key);

            if (table == null)
            {
                $"'{key}' is not present in any Tables".LogInternal(Assembly.GetCallingAssembly(), Logger.LogTypes.error);
                return;
            }

            table[key] = (ImplicitStringCollection)value;
        }

        /// <summary>
        /// Get an item from a table.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetItem(string key)
        {
            return GetTable(key)[key].GetCombinedString();
        }

        static Dictionary<string, StringTableManager.StringCollection> GetTable(string key)
        {
            if (StringTableManager.CoreTable.ContainsKey(key))
                return StringTableManager.CoreTable;
            else if (StringTableManager.EnemyTable.ContainsKey(key))
                return StringTableManager.EnemyTable;
            else if (StringTableManager.ItemTable.ContainsKey(key))
                return StringTableManager.ItemTable;
            else if (StringTableManager.IntroTable.ContainsKey(key))
                return StringTableManager.IntroTable;

            return null;
        }

        static bool KeyNull(string key)
        {
            bool r = string.IsNullOrEmpty(key);

            if (r)
                "Key may not be null or empty".LogError();

            return r;
        }
    }

    /// <summary>
    /// A table with <see cref="StringTableManager.StringCollection"/>
    /// </summary>
    public enum Table
    {
        /// <summary>
        /// Core table
        /// </summary>
        Core,
        /// <summary>
        /// Item table
        /// </summary>
        Item,
        /// <summary>
        /// Intro table
        /// </summary>
        Intro,
        /// <summary>
        /// Enemy table
        /// </summary>
        Enemy
    }
}
