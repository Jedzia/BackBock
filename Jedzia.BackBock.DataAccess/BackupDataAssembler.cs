using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jedzia.BackBock.DataAccess;
using Jedzia.BackBock.Model.Data;

namespace Jedzia.BackBock.DataAccess
{
    /// <summary>
    /// Converts from DTO classes to the model and back.
    /// </summary>
    internal class BackupDataAssembler
    {
        public BackupData ConvertFromDTO(DTO.BackupData source)
        {
            return source.FromDTO();
        }

        public DTO.BackupData ConvertToDTO(BackupData source)
        {
            return source.ToDTO();
        }
    }

    internal static class MappingHelper
    {
        public static BackupData FromDTO(this DTO.BackupData source)
        {
            var h = new BackupData();
            h.DatasetGroup = source.DatasetGroup;
            h.DatasetName = source.DatasetName;
            if (source.BackupItem != null)
                h.BackupItem = source.BackupItem.Select(wld => wld.FromDTO()).ToList();
            return h;
        }

        public static DTO.BackupData ToDTO(this BackupData source)
        {
            var h = new DTO.BackupData();
            h.DatasetGroup = source.DatasetGroup;
            h.DatasetName = source.DatasetName;
            if (source.BackupItem != null)
                h.BackupItem = source.BackupItem.ConvertAll(wld => wld.ToDTO()).ToArray();
            return h;
        }

        public static Wildcard FromDTO(this DTO.Wildcard source)
        {
            var h = new Wildcard();
            h.Enabled = source.Enabled;
            h.Pattern = source.Pattern;
            return h;
        }

        public static DTO.Wildcard ToDTO(this Wildcard source)
        {
            var h = new DTO.Wildcard();
            h.Enabled = source.Enabled;
            h.Pattern = source.Pattern;
            return h;
        }

        public static PathDataType FromDTO(this DTO.PathDataType source)
        {
            var h = new PathDataType();
            h.Path = source.Path;
            h.UserData = source.UserData;
            //h.Exclusion = source.Exclusion.ToList().ConvertAll(wld => wld.FromDTO());
            //h.Inclusion = source.Inclusion.ToList().ConvertAll(wld => wld.FromDTO());
            if (source.Exclusion != null)
                h.Exclusion = source.Exclusion.Select(wld => wld.FromDTO()).ToList();
            if (source.Inclusion != null)
                h.Inclusion = source.Inclusion.Select(wld => wld.FromDTO()).ToList();
            return h;
        }

        public static DTO.PathDataType ToDTO(this PathDataType source)
        {
            var h = new DTO.PathDataType();
            h.Path = source.Path;
            h.UserData = source.UserData;
            if (source.Exclusion != null)
                h.Exclusion = source.Exclusion.ConvertAll(wld => wld.ToDTO()).ToArray();
            if (source.Inclusion != null)
                h.Inclusion = source.Inclusion.ConvertAll(wld => wld.ToDTO()).ToArray();
            return h;
        }

        public static TaskType FromDTO(this DTO.TaskType source)
        {
            var h = new TaskType();
            h.TypeName = source.TypeName;
            if (source.AnyAttr != null)
                h.AnyAttr = source.AnyAttr.ToList();
            return h;
        }

        public static DTO.TaskType ToDTO(this TaskType source)
        {
            var h = new DTO.TaskType();
            h.TypeName = source.TypeName;
            if (source.AnyAttr != null)
                h.AnyAttr = source.AnyAttr.ToArray();
            return h;
        }

        public static BackupItemType FromDTO(this DTO.BackupItemType source)
        {
            var h = new BackupItemType();
            h.IsEnabled = source.IsEnabled;
            h.ItemGroup = source.ItemGroup;
            h.ItemName = source.ItemName;
            if (source.Task != null)
                h.Task = source.Task.FromDTO();
            if (source.Path != null)
                h.Path = source.Path.Select(wld => wld.FromDTO()).ToList();
            return h;
        }

        public static DTO.BackupItemType ToDTO(this BackupItemType source)
        {
            var h = new DTO.BackupItemType();
            h.IsEnabled = source.IsEnabled;
            h.ItemGroup = source.ItemGroup;
            h.ItemName = source.ItemName;
            if (source.Task != null)
                h.Task = source.Task.ToDTO();
            if (source.Path != null)
                h.Path = source.Path.ConvertAll(wld => wld.ToDTO()).ToArray();
            return h;
        }


    }
}
