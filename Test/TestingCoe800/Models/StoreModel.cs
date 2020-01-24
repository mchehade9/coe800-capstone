using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TestingCoe800.Models
{
    public class Stores

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; }
        public string StoreName { get; set; }
        public string  Location  { get; set; }
         public string ManagerIDFk { get; set; }
        public string PhoneNumber { get; set; }

        
       // public string DateOpened { get; set; } = DateTime.Now.ToString();
    }
}