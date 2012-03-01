namespace Jedzia.BackBock.Tasks
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class OutputAttribute : Attribute
    {
    }
}