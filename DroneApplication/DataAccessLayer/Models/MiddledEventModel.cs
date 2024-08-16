using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class MiddledEventModel
    {
        public int Id { get; set; }
        public int DroneId { get; set; }
        public int FileId { get; set; }
        public double Xmid { get; set; }
        public double Ymid { get; set; }
        public double Zmid { get; set; }
        
    }
}
