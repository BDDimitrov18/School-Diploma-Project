using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ExifFileCategory
    {
        public DroneModel droneModel { get; set; }
        public FileModel fileModel { get; set; }

        [Required]
        [DisplayName("files")]
        public IFormFileCollection fileCollection { get; set; }
    }
}
