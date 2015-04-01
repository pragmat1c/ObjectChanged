using System;

namespace ObjectChanged
{
    /// <summary>
    ///     ChangesId indicates that the property is an unique identity field. On object with a ChangesID attribute can be
    ///     compared in an IEnumerable. It can be used on Fields and Properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ChangesIdAttribute : Attribute
    {
    }
}