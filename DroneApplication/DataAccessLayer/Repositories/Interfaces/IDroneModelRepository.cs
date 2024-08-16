using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IDroneModelRepository
    {
        
        public bool CreateDrone(DroneModel model);
        public bool DeleteDrone(int Id);

        public DroneModel GetByName(string name);


    }
}
