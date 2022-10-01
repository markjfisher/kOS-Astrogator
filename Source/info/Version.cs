using System.Reflection;

namespace kOS.AddOns.kOSAstrogator.info
{
    /// <summary>
    /// Deals with Version information
    /// </summary>
    public static class Version
    {
        private static readonly System.Version modVersion = Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Create a formatted version string for the mod.
        /// </summary>
        /// <returns></returns>
        public static string GetVersion() => string.Format("kOS-Astrogator: v{0}.{1}.{2}, Astrogator: {3}", modVersion.Major, modVersion.Minor, modVersion.Build, Astrogator.ViewTools.versionString);
    }
}
