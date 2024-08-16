using BussinesLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories.Interfaces;

namespace BussinesLayer.Services
{
    public class DroneModelService : IDroneModelService
    {
        private IDroneModelRepository _repository;
        public DroneModelService(IDroneModelRepository droneModelRepository)
        {
            this._repository = droneModelRepository;
        }
        public bool DeleteDrone(int id) {
            return _repository.DeleteDrone(id);
            
        }
        public  bool CreateDrone(DroneModel model) {
            return _repository.CreateDrone(model);
        }

        public DroneModel GetByName(string name)
        {
            return _repository.GetByName(name);
        }
    }
}
