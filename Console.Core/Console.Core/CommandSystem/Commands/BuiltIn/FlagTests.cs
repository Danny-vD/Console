namespace Console.Core.CommandSystem.Commands.BuiltIn
{
    /// <summary>
    /// Class Containing Test Commands for the CommandFlagAttribute
    /// </summary>
    public class FlagTests
    {
        /// <summary>
        /// Adds the FlagTests Commands
        /// </summary>
        public static void AddFlagTestCommands()
        {
            CommandAttributeUtils.AddCommands<FlagTests>();
        }

        /// <summary>
        /// Flag Test Command. 2 normal arguments, 2 flags
        /// </summary>
        /// <param name="arg0">Argument 1</param>
        /// <param name="arg1">Argument 2</param>
        /// <param name="flag1">Flag -x</param>
        /// <param name="flag2">Flag -y</param>
        [Command("ft", "Flag Test. This Command is Legal")]
        private static void FlagTest1(string arg0, int arg1, [CommandFlag("x")] bool flag1, [CommandFlag("y")]bool flag2) //(2-4)
        {
            ConsoleCoreConfig.CoreLogger.Log("(string)arg0: " + arg0 + "\n(int)arg1: " + arg1 + "\nx: " + flag1 + "\ny: " + flag2);
        }
        /// <summary>
        /// Flag Test Command. 2 normal arguments, 1 Flag.
        /// This Command is Illegal when FlagTest1 is added.
        /// </summary>
        /// <param name="arg0">Argument 1</param>
        /// <param name="arg1">Argument 2</param>
        /// <param name="flag1">Flag -x</param>
        [Command("ft", "Flag Test. This Command is Illegal  when FlagTest1 is added")]
        private static void FlagTest2(string arg0, int arg1, [CommandFlag("x")] bool flag1) //Illegal (2-3)
        {
            ConsoleCoreConfig.CoreLogger.Log("(string)arg0: " + arg0 + "\n(int)arg1: " + arg1 + "\nx: " + flag1);
        }

        /// <summary>
        /// Flag Test Command. 1 normal argument, 2 Flags.
        /// </summary>
        /// <param name="arg0">Argument 1</param>
        /// <param name="flag1">Flag -x</param>
        /// <param name="flag2">Flag -y</param>
        [Command("ft", "Flag Test. This Command is Legal")]
        private static void FlagTest3(string arg0, [CommandFlag("x")] bool flag1, [CommandFlag("y")]bool flag2) //Legal(1-3)
        {
            ConsoleCoreConfig.CoreLogger.Log("(string)arg0: " + arg0 + "\nx: " + flag1 + "\ny: " + flag2);
        }

        /// <summary>
        /// Flag Test Command. 1 normal argument, 2 Flags.
        /// This Command is Illegal when FlagTest3 is added.
        /// </summary>
        /// <param name="arg0">Argument 1</param>
        /// <param name="flag1">Flag -x</param>
        /// <param name="flag2">Flag -y</param>
        [Command("ft", "Flag Test. This Command is Illegal when FlagTest3 is added")]
        private static void FlagTest4(int arg0, [CommandFlag("x")] bool flag1, [CommandFlag("y")]bool flag2) //Illegal(1-3)
        {
            ConsoleCoreConfig.CoreLogger.Log("(int)arg0: " + arg0 + "\nx: " + flag1 + "\ny: " + flag2);
        }
    }
}