using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IDroneApplicationDbContext
    {
        public DbSet<MiddledEventModel> Events { get; set; }
        public DbSet<DroneModel> DroneModel { get; set; }
    }
}
