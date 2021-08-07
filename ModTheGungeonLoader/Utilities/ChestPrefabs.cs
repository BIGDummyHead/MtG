namespace Gungeon.Utilities
{
    /// <summary>
    /// A collection of chest in game.
    /// </summary>
    public static class ChestPrefabs
    {
        private static RewardManager Manager => GameManager.Instance.RewardManager;

        /// <summary>
        /// The 'A' Chest
        /// </summary>
        public static Chest A_Chest => Manager.A_Chest;
        /// <summary>
        /// The 'B' Chest
        /// </summary>
        public static Chest B_Chest => Manager.B_Chest;
        /// <summary>
        /// The 'C' Chest
        /// </summary>
        public static Chest C_Chest => Manager.C_Chest;
        /// <summary>
        /// The 'D' Chest
        /// </summary>
        public static Chest D_Chest => Manager.D_Chest;
        /// <summary>
        /// The 'S' Chest
        /// </summary>
        public static Chest S_Chest => Manager.S_Chest;
        /// <summary>
        /// The 'Evil' Chest
        /// </summary>
        public static Chest Synergy_Chest => Manager.Synergy_Chest;
        /// <summary>
        /// The 'Rainbow' Chest
        /// </summary>
        public static Chest Rainbow_Chest => Manager.Rainbow_Chest;

        /// <summary>
        /// The quality of the chest.
        /// </summary>
        /// <param name="chest"></param>
        /// <returns></returns>
        public static PickupObject.ItemQuality Quality(this Chest chest)
        {
            return Manager.GetQualityFromChest(chest);
        }

        /// <summary>
        /// Open a chest
        /// </summary>
        /// <param name="chest">Chest to open</param>
        /// <param name="player">Who opened this chest?</param>
        public static void Open(this Chest chest, PlayerController player)
        {
            chest.GetMethod("Open", typeof(PlayerController)).Invoke(player);
        }
    }
}
