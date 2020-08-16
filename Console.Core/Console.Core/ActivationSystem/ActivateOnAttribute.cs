using System;

namespace Console.Core.ActivationSystem
{
    /// <summary>
    /// Decorate any Class with this attribute to be able to Load all Classes of this Type with ActivateOnAttributeUtils.Activate
    /// This attribute can be attached to a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ActivateOnAttribute : Attribute { }
}