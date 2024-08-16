using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class DroneModelRepository : IDroneModelRepository
    {
        private DroneApplicationDbContext _context;

        public DroneModelRepository(IDroneApplicationDbContext context)
        {
            this._context = context as DroneApplicationDbContext;
        }
        public  bool CreateDrone(DroneModel model)
        {
            
            if (model != null)
            {
                if (model.categoryZone == "") {
                    model.categoryZone = "Worldwide";
                }
                    _context.DroneModel.Add(model);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteDrone(int id)
        {
            var droneModel = _context.DroneModel.Find(id);
            if (droneModel != null)
            {
                _context.DroneModel.Remove(droneModel);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
        
        public DroneModel GetByName(string name) {
            return _context.DroneModel.FirstOrDefault(i => i.Name == name);
        }
    }
}
