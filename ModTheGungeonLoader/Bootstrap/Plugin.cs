namespace Gungeon.Bootstrap
{
    /// <summary>
    /// Represents a plugin, loads before all mods. Allows you to change the loader and mods however you would like.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Called before any Mod loading or patching done by the <see cref="Boot"/>
        /// </summary>
        void Load();
    }
}
