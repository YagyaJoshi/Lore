using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class FrontendContent : DBaseEntity
    {
        public Int64 Id { get; set; }
        public string TextData { get; set; }
        public string SystemName { get; set; }
        public string Image { get; set; }
    }
}
