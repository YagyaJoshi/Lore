using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Data.Entities
{
    public class Slider
    {
        [Key]
        public Int64 Id { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public string FirstText { get; set; }
        public string SecondText { get; set; }
        public bool IsVisible { get; set; }
    }
}
