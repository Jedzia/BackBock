namespace Jedzia.BackBock.Tasks
{
    using System;

    /// <summary>
    /// Defines the metadata attribute that task authors use to identify task properties that output data from the task.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class OutputAttribute : Attribute
    {
    }
}