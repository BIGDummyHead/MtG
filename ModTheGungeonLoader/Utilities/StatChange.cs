namespace Gungeon.Utilities
{
    /// <summary>
    /// Used when changing stats.
    /// </summary>
    public enum StatChange
    {
        /// <summary>
        /// Add to an existing stat
        /// </summary>
        Add,
        /// <summary>
        /// Multiply an existing stat
        /// </summary>
        Mult,
        /// <summary>
        /// Subtract from an existing
        /// </summary>
        Sub,
        /// <summary>
        /// Divide an existing stat
        /// </summary>
        Div,
        /// <summary>
        /// Change the total of the stat
        /// </summary>
        Total
    }
}
