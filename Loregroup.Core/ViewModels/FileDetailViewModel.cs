using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    //class FileDetailViewModel
    //{
    //}
    public class FileDetailViewModel
    {
        public Int64 Id { get; set; }
        public bool IsInFileSystem { get; set; }
        public String FileName { get; set; }
        public String ThumbnailName { get; set; }
        public String FilePath { get; set; }
        public String ThumbnailPath { get; set; }
        public FileType FileType { get; set; }
        public FileType ThumbnailFileType { get; set; }
        public FileRelation FileRelation { get; set; }
        public Status Status { get; set; }
        public Int64 CreatedById { get; set; }
        public Int64 ModifiedById { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
