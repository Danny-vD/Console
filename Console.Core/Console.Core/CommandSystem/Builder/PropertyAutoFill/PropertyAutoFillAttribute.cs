using System;

namespace Console.Core.CommandSystem.Builder.PropertyAutoFill
{
    /// <summary>
    /// When a Parameter Gets Decorated with the PropertyAutoFillAttribute it enables the CommandBuilder to Suggest Possible Property Values for this Parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PropertyAutoFillAttribute : ConsoleAttribute { }
}