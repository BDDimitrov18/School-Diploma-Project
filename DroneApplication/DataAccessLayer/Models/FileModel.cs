using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public  class FileModel
    {

        public int Id { get; set; }
        [Required]
        [DisplayName("FileName")]
        public string? Name { get; set; }

        public int? DroneId { get; set; }

        public  int  Type { get; set; }

        public double? h { get; set; }
    }
}
