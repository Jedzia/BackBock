using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Jedzia.BackBock.Tasks
{
    public interface ITaskItem
    {
        string ItemSpec { get; set; }
        ICollection MetadataNames { get; }
        int MetadataCount { get; }
        string GetMetadata(string metadataName);
        void SetMetadata(string metadataName, string metadataValue);
        void RemoveMetadata(string metadataName);
        void CopyMetadataTo(ITaskItem destinationItem);
        IDictionary CloneCustomMetadata();
    }

    public interface ITask
    {
        string Name { get; }
        bool Execute();
    }
}
