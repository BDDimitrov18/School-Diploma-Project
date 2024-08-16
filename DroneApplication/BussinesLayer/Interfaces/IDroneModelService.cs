﻿using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Interfaces
{
    public interface IDroneModelService
    {
        public bool DeleteDrone(int id);
        
        public bool CreateDrone(DroneModel model);

        public DroneModel GetByName(string name);
    }
}
