using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ExifInfoModel
    {
        public int Id { get; set; }
        public string? name { get; set; }

        public int FileId { get; set; }
        public int DroneId { get; set; }
        public double gps_longtitude { get; set; }
        public double gps_latitude { get; set; }
        public double gps_altitude { get; set; }
        public DateTime date { get; set; }
    }
}
