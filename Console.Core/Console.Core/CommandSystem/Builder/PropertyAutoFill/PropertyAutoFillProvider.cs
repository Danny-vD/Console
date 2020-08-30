using System.Linq;
using Console.Core.CommandSystem.Commands;
using Console.Core.PropertySystem;

namespace Console.Core.CommandSystem.Builder.PropertyAutoFill
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyAutoFillProvider : AutoFillProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cmd"></param>
        /// <param name="paramNum"></param>
        /// <returns></returns>
        public override bool CanFill(CommandBuilder builder, AbstractCommand cmd, int paramNum)
        {
            if (paramNum == 0) //The Command itself
                return false;
            if (cmd.MetaData.Count >= paramNum) //Check metadata (parameter) count
                return cmd.MetaData[paramNum - 1].Attributes.Any(x => x is PropertyAutoFillAttribute); //Use offset by one because of command name
            return false; //Default Case
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramNum"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public override string[] GetAutoFills(AbstractCommand cmd, int paramNum, string start)
        {
            return PropertyManager.AllPropertyPaths.Where(x => x.StartsWith(start)).ToArray();
        }
    }
}