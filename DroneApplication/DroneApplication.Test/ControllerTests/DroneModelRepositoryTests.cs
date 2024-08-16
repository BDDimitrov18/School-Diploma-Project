using BussinesLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using DroneApplication.FileUploadService;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneApplication.Test.ControllerTests
{

    public class DroneModelRepositoryTests
    {
        
        private async Task<DroneApplicationDbContext> GetDbContext() {
            var options = new DbContextOptionsBuilder<DroneApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new DroneApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.DroneModel.CountAsync() < 0) {
                for (int i = 0; i < 10; i++)
                {


                    databaseContext.DroneModel.Add(
                        new DroneModel()
                        {
                            Id = 1,
                            Name = "stoqn"

                        });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        [Fact]
        public async void CreateDrone_IsNotNull_ReturnTrue()
        {
            //Arrange
            var droneModel = new DroneModel()
            {
                Id = 1,
                Name = "stoqn"

            };
            var _context = await GetDbContext();
            var droneRepository = new DroneModelRepository(_context);
            //Act
            var result = droneRepository.CreateDrone(droneModel);
            result.Should().BeTrue();
        }
        [Fact]
        public async void CreateDrone_IsNull_ReturnFalse()
        {
            //Arrange
            var droneModel = new DroneModel();
            droneModel = null;
            
            var _context = await GetDbContext();
            var droneRepository = new DroneModelRepository(_context);
            //Act
            var result = droneRepository.CreateDrone(droneModel);
            result.Should().BeFalse();
        }
        [Fact]
        public async void DeleteDrone_IdDoesNotExist_ReturnFalse()
        {
            //Arrange
            var droneModel = new DroneModel()
            {
                Id = 1,
                Name = "stoqn"

            };

            var _context = await GetDbContext();
            var droneRepository = new DroneModelRepository(_context);
            droneRepository.CreateDrone(droneModel);
            //Act
            var result = droneRepository.DeleteDrone(-3);
            result.Should().BeFalse();
        }
        [Fact]
        public async void DeleteDrone_IdExist_ReturnTrue()
        {
            //Arrange
            var droneModel = new DroneModel()
            {
                Id = 1,
                Name = "stoqn"

            };

            var _context = await GetDbContext();
            var droneRepository = new DroneModelRepository(_context);
            droneRepository.CreateDrone(droneModel);
            //Act
            var result = droneRepository.DeleteDrone(1);
            result.Should().BeTrue();
        }
    }
}
