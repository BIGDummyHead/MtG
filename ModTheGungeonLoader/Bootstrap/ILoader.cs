namespace Gungeon.Bootstrap
{
    /// <summary>
    /// Represents a Loader for mods, can be passed into <see cref="ModLoader.CreateGlobal(string, ILoader)"/> for customization of loading.
    /// </summary>
    public interface ILoader
    {
        /// <summary>
        /// This is second in line to be called.
        /// </summary>
        /// <param name="modsFolder">The mods folder passed in from the <see cref="ModLoader"/></param>
        void HandleLoad(string modsFolder);

        /// <summary>
        /// This is first in line to be called, please make your own Console or use the <see cref="DefaultConsole.Open"/> for best results.
        /// </summary>
        void HandleConsole();

        /// <summary>
        /// Called third in line, finish loading and clean up any mess you've made in <see cref="HandleConsole"/> and <seealso cref="HandleLoad(string)"/>
        /// </summary>
        void FinishLoad();
    }
}
