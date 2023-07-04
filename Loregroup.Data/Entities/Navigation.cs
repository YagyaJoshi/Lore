using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities {
    public class Navigation : BaseEntity {
        public bool HasSubMenu { get; set; }
        public String Icon { get; set; }
        public String Text { get; set; }
        public String ActionUrl { get; set; }
        /// <summary>
        /// Based on HttpRequestType
        /// </summary>
        public int ActionUrlRequestType { get; set; }
        public int Order { get; set; }
    }

    public class SubNavigation : BaseEntity
    {
        public String Icon { get; set; }
        public String Text { get; set; }
        public String ActionUrl { get; set; }

        /// <summary>
        /// Based on HttpRequestType
        /// </summary>
        public int ActionUrlRequestType { get; set; }

        public Int64 NavigationId { get; set; }
        public virtual Navigation Navigation { get; set; }
        public int Order { get; set; }
    }
}
