using System;

namespace Console.Core.CommandSystem.Builder.IOAutoFill
{

    /// <summary>
    /// When a Parameter Gets Decorated with the IOAutoFillAttribute it enables the CommandBuilder to Suggest Possible File System Entry Values for this Parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class IOAutoFillAttribute : ConsoleAttribute { }
}