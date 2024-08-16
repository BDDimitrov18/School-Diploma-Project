using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Interfaces
{
    public interface IFileModelService
    {
        public Task<bool> CreateFile(FileModel model);
        public bool DeleteFile(int id);

        public FileModel GetFile(int? id);

        public IEnumerable<FileModel> GetFileByDroneId(int? id);

        public void DeleteFilesByDroneId(int id);

        public ExifInfoModel ExtractExif(string path);
    }
}
