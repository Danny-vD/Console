using Console.Core.ActivationSystem;
using Console.Core.CommandSystem.Commands;

namespace Console.Core.CommandSystem.Builder
{
    /// <summary>
    /// 
    /// </summary>
    [ActivateOn]
    public abstract class AutoFillProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramNum"></param>
        /// <returns></returns>
        public abstract bool CanFill(CommandBuilder builder, AbstractCommand cmd, int paramNum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public abstract string[] GetAutoFills(AbstractCommand cmd, int paramNum, string start);
    }
}