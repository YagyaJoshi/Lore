using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Gallery : DBaseEntity
    {
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public string image { get; set; }
        public string VideoUrl { get; set;}
        public bool IsVideo { get; set; }
        public int GalleryTypeId { get; set; }
    }
}
