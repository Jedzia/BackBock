namespace Jedzia.BackBock.Tasks
{
    using System.Collections;

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
}