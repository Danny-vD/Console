using System.Linq;
using Console.Core.CommandSystem.Commands;

namespace Console.Core.CommandSystem.Builder.CommandAutoFill
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandAutoFillProvider : AutoFillProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramNum"></param>
        /// <returns></returns>
        public override bool CanFill(CommandBuilder builder, AbstractCommand cmd, int paramNum)
        {
            if (paramNum == 0)
                return true;
            if (cmd.MetaData.Count >= paramNum)
                return cmd.MetaData[paramNum - 1].Attributes.Any(x => x is CommandAutoFillAttribute);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public override string[] GetAutoFills(AbstractCommand cmd, int paramNum, string start)
        {
            return CommandManager.commands.Where(x => x.Name.StartsWith(start)).Select(x => x.Name).ToArray();
        }
    }
}