using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IFileModelRepository
    {
        public Task<bool> CreateFile(FileModel model);

        public bool DeleteFile(int id);

        public FileModel GetFile(int? id);

        public IEnumerable<FileModel> GetFileByDroneId(int? id);

        public void DeleteFileByDroneId(int id);
    }
}
