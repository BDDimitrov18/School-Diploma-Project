using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class FileModelRepository : IFileModelRepository
    {
        private DroneApplicationDbContext _context;

        public FileModelRepository(IDroneApplicationDbContext context)
        {
            this._context = context as DroneApplicationDbContext;
        }
        public async Task<bool> CreateFile(FileModel model)
        {
            if (model != null)
            {
                _context.FileModel.Add(model);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteFile(int id)
        {
            var fileModel = _context.FileModel.Find(id);
            if (fileModel != null)
            {
                _context.FileModel.Remove(fileModel);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public void DeleteFileByDroneId(int id)
        {
            _context.FileModel.RemoveRange(_context.FileModel.Where(i => i.DroneId == id));
            _context.SaveChanges();
        }

        public FileModel GetFile(int? id)
        {
            return _context.FileModel.FirstOrDefault(d => d.Id == id);
        }

        public IEnumerable<FileModel> GetFileByDroneId(int? id)
        {
            return _context.FileModel.Where(i => i.DroneId == id);
        }
    }
}
