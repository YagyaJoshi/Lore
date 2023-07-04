using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class FileLibrary : BaseEntity
    {
        public String FileName { get; set; }
        public int FileType { get; set; }
        public string FileRelation { get; set; }
        public byte[] FileBytes { get; set; }
        public Int64 FileRelationId { get; set; }
    }
}
