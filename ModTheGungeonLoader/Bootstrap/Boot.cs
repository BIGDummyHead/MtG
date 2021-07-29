namespace Gungeon.Bootstrap
{
    //a simple class at handling the loading process.
    internal class Boot
    {
        public static void Load()
        {
            //hey whatchu you doin ;)
            ModLoader.Patch();
            ModLoader.GetGlobal().LoaderLoad();
        }
    }
}
