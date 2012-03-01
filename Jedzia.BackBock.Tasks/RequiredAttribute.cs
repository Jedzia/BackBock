namespace Jedzia.BackBock.Tasks
{
    using System;

    /// <summary>
    /// Defines the metadata attribute that task authors use to identify required task properties. Task properties with this attribute must have a set value when the task is run.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RequiredAttribute : Attribute
    {
    }
}