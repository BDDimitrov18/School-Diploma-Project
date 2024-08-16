using BussinesLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using ExifLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer.Services
{
    public class FileModelService : IFileModelService
    {
        private IFileModelRepository fileModelRepository;

        public FileModelService(IFileModelRepository fileModelRepository)
        {
            this.fileModelRepository = fileModelRepository;
        }
        public async Task<bool> CreateFile(FileModel model)
        {
            return await fileModelRepository.CreateFile(model);
        }

        public bool DeleteFile(int id)
        {
            return fileModelRepository.DeleteFile(id); 
        }

        public void DeleteFilesByDroneId(int id)
        {
            fileModelRepository.DeleteFileByDroneId(id);
        }

        public FileModel GetFile(int? id)
        {
            return fileModelRepository.GetFile(id);
        }

        public IEnumerable<FileModel> GetFileByDroneId(int? id) {
            return fileModelRepository.GetFileByDroneId(id);
        }

        public ExifInfoModel ExtractExif(string path) {
            var file = ImageFile.FromFile(path);
            var latTag =file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLatitude);
            var longTag = file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLongitude);
            var altTag = file.Properties.Get<ExifURational>(ExifTag.GPSAltitude);
            var date = file.Properties.Get<ExifDateTime>(ExifTag.DateTime);
            ExifInfoModel temp = new ExifInfoModel();
            temp.date = date;
            temp.gps_latitude = latTag.ToFloat();
            temp.gps_longtitude = longTag.ToFloat();
            temp.gps_altitude = altTag.ToFloat();
            return temp;
        }
    }
}
