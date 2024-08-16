using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class MiddledEventRepository : IMiddledEventRepository
    {
        private DroneApplicationDbContext _context;

        public MiddledEventRepository(IDroneApplicationDbContext context)
        {
            this._context = context as DroneApplicationDbContext;
        }
        public bool CreateMiddledEvent(MiddledEventModel middledEvent)
        {
            if (middledEvent == null) { 
            return false;
            }
            _context.Events.Add(middledEvent);
            _context.SaveChanges();
            return true;
        }


        

        public IEnumerable<MiddledEventModel> GetEvents(int DroneId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MiddledEventModel> getEventsByFileId(int? id) {
            return _context.Events.Where(i => i.FileId == id);
        }

        public void DeleteByFileId(int id) {
            _context.Events.RemoveRange(_context.Events.Where(i => i.FileId == id));
            _context.SaveChanges();
        }

        public MiddledEventModel GetEventById(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteByDroneId(int id)
        {
            _context.Events.RemoveRange(_context.Events.Where(i => i.DroneId == id));
            _context.SaveChanges();
        }

        public bool ExistByFileId(int id) {
            if (_context.ExifInfoModels.Any(i => i.FileId == id))
            {
                return true;
            }
            return false;
        }

        public bool CreateExifInfoModel(ExifInfoModel model) {
            if (model == null)
            {
                return false;
            }
            _context.ExifInfoModels.Add(model);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<ExifInfoModel> getExifInfoModelsByFileId(int? id)
        {
            return _context.ExifInfoModels.Where(i => i.FileId == id);
        }

        public void DeleteExifInfoModelByFileID(int id) {
            _context.ExifInfoModels.RemoveRange(_context.ExifInfoModels.Where(i => i.FileId == id));
            _context.SaveChanges();
        }

        public void DeleteExifInfoModelByDroneID(int id) {
            _context.ExifInfoModels.RemoveRange(_context.ExifInfoModels.Where(i => i.DroneId == id));
            _context.SaveChanges();
        }

        public void CreateMatchedModel(MatchedEventModel model) {
            _context.MatchedEventModels.Add(model);
            _context.SaveChanges();
        }


        public void DeleteMatchedModelByFileId(int id) {
            _context.MatchedEventModels.RemoveRange(_context.MatchedEventModels.Where(i => i.FileId == id));
            _context.SaveChanges();
        }

    }
}
