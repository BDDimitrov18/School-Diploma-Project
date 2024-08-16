using BussinesLayer.Interfaces;
using BussinesLayer.Services;
using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace TestingProject
{
    [TestClass]
    public class UnitTest1
    {
        [Fact]
        public void CreateDrone_IsNotNull_ReturnsTrue()
        {
            DroneModel droneModel = new DroneModel();
            

            var context = new Mock<DroneApplicationDbContext>();
            var dbSetMock = new Mock<DbSet<DroneModel>>(); 

            var repository = new DroneModelRepository(context.Object);
            repository.CreateDrone(droneModel);

            
        }
    }
}