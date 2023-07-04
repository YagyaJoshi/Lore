﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class CollectionYear : DBaseEntity
    {
        [Key]
        public Int64 Id { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
    }
}
