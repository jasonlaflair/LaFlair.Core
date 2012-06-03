using System;

namespace LaFlair.Core.IoC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ContainerVisibleAttribute : Attribute
    {
    }
}
