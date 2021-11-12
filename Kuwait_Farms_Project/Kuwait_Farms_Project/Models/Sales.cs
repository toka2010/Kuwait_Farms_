using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kuwait_Farms_Project.Models
{
    public class Sales
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Product_id { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }
        public int Price { get; set; }
        [NotMapped]
        public string SalesrMsg { get; set; }
    }
}