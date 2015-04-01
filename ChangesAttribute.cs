using System;

namespace ObjectChanged
{
    /// <summary>
    ///     Changes indicates that the property should be checked for changes. It can be used on Fields and Properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ChangesAttribute : Attribute
    {
    }
}