using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class MiddlingFileCategory
    {
        public DroneModel droneModel { get; set; }
        public IFormFile file { get; set; }
    }
}
