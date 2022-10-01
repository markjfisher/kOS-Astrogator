namespace kOS.AddOns.kOSAstrogator.info
{
    /// <summary>
    /// Help command
    /// </summary>
    public static class Help
    {
        /// <summary>
        /// The help command
        /// </summary>
        /// <param name="shared">The shared data</param>
        public static void PrintHelp(SharedObjects shared)
        {
            shared.Screen.Print("--------------------------------------------");
            shared.Screen.Print("kOS-Astrogator");
            shared.Screen.Print("Usage: addons:astrogator:<cmd>");
            shared.Screen.Print("See https://github.com/markjfisher/kOS-Astrogator/blob/master/GameData/kOS-Astrogator/README.md");
            shared.Screen.Print("for full command details.");
            shared.Screen.Print("--------------------------------------------");
        }
    }
}
