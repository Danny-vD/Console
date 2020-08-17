using System;

/// <summary>
/// The Console.Core.ActivationSystem enables the Instanciation of any Type that is decorated with the ActivateOnAttribute
/// </summary>
namespace Console.Core.ActivationSystem
{
    /// <summary>
    /// Decorate any Class with this attribute to be able to Load all Classes of this Type with ActivateOnAttributeUtils.Activate
    /// This attribute can be attached to a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ActivateOnAttribute : Attribute { }
}