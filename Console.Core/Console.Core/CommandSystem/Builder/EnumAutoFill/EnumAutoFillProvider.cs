using System;
using System.Linq;
using Console.Core.CommandSystem.Commands;

namespace Console.Core.CommandSystem.Builder.EnumAutoFill
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumAutoFillProvider : AutoFillProvider
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
            if (paramNum == 0) return false;
            if (cmd.MetaData.Count >= paramNum)
            {
                bool ret = cmd.MetaData[paramNum - 1].Attributes.Any(x => x is EnumAutoFillAttribute);
                return ret;
            }
            return false;
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
            if (paramNum == 0) return new string[0];
            if (cmd.MetaData.Count >= paramNum)
            {
                EnumAutoFillAttribute ret = cmd.MetaData[paramNum - 1].Attributes.FirstOrDefault(x => x is EnumAutoFillAttribute) as EnumAutoFillAttribute;
                if (ret == null) return new string[0];
                Type t = ret.EnumType ?? cmd.MetaData[paramNum - 1].ParameterType;
                if (t.IsEnum)
                {
                    return Enum.GetNames(t);
                }
            }
            return new string[0];
        }
    }
}