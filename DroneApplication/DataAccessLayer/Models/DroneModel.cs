using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DataAccessLayer.Models
{
    public class DroneModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Име на дрона")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Категория на зона")]
        public string? categoryZone { get; set; } = "WorldWide";

    }
}
