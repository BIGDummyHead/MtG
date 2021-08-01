﻿using Gungeon.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gungeon.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class JournalUtilities
    {
        /// <summary>
        /// Set the full entry of a <see cref="JournalEntry"/> in the ammonomicon
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="full"></param>
        public static void SetFullEntry(this JournalEntry entry, string full)
        {
            SetItem(entry.AmmonomiconFullEntry, full);
        }


        /// <summary>
        /// Set the full entry of a <see cref="JournalEntry"/> in the ammonomicon
        /// </summary>
        /// <param name="trackable"></param>
        /// <param name="full"></param>
        public static void SetFullEntry(this EncounterTrackable trackable, string full)
        {
            trackable?.journalData?.SetFullEntry(full);
        }

        /// <summary>
        /// Set the full entry of a <see cref="JournalEntry"/> in the ammonomicon
        /// </summary>
        /// <param name="pickup"></param>
        /// <param name="full"></param>
        public static void SetFullEntry(this PickupObject pickup, string full)
        {
            pickup?.encounterTrackable?.journalData?.SetFullEntry(full);
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
        /// Change an item's value from the key. The table is automatically determined via the key provided.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetItem(string key, string value)
        {
            if (KeyNull(key))
                return;

            var table = GetTable(key);

            if (table == null)
            {
                $"'{key}' is not present in Item Table".LogError();
                return;
            }

            table[key] = (ImplicitStringCollection)value;
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
}