using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IMiddledEventRepository
    {
        public bool CreateMiddledEvent(MiddledEventModel middledEvent);
        public MiddledEventModel GetEventById(int id);
        public IEnumerable<MiddledEventModel> GetEvents(int DroneId);
        public void DeleteByDroneId(int id);
        public IEnumerable<MiddledEventModel> getEventsByFileId(int? id);

        public void DeleteByFileId(int id);

        public bool ExistByFileId(int id);

        public bool CreateExifInfoModel(ExifInfoModel model);

        public IEnumerable<ExifInfoModel> getExifInfoModelsByFileId(int? id);

        public void DeleteExifInfoModelByFileID(int id);

        public void DeleteExifInfoModelByDroneID(int id);

        public void CreateMatchedModel(MatchedEventModel model);

        public void DeleteMatchedModelByFileId(int id);
    }
}
