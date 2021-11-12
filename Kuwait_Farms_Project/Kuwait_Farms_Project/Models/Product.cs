using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kuwait_Farms_Project.Models
{
    public class Product
    {[Key]
        public int Id { get; set; }
       [Required]
        public string ProductName { get; set; }
        [Required]
        public int Price { get; set; }
        
        public int Quntity { get; set; }
        public string  Image { get; set; }
        [NotMapped]
        public string ProductErrorMsg { get; set; }
    }
}