using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kuwait_Farms_Project.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Product_id { get; set; }
   
        public int Price { get; set; }
    }
}