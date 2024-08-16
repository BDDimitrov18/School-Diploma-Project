using DataAccessLayer.Interfaces;
using DataAccessLayer.Migrations;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class DroneApplicationDbContext : DbContext, IDroneApplicationDbContext
    {
        public DbSet<MiddledEventModel> Events { get ; set; }
        public DbSet<DroneModel> DroneModel { get; set; }
        public DbSet<FileModel> FileModel { get; set; }
        public DbSet<ExifInfoModel> ExifInfoModels { get; set; }
        public DbSet<MatchedEventModel> MatchedEventModels { get; set; }



        public DroneApplicationDbContext(DbContextOptions<DroneApplicationDbContext> options) : base(options)
        {
        }
    }
}