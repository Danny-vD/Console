using System;

namespace Console.Core.CommandSystem.Builder.BuiltIn.CommandAutoFill
{
    /// <summary>
    /// When a Parameter Gets Decorated with the CommandAutoFillAttribute it enables the CommandBuilder to Suggest Possible Command Names for this Parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CommandAutoFillAttribute : ConsoleAttribute
    {

    }
}