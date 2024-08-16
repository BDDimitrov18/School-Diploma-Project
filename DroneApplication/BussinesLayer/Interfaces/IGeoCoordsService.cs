using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace BussinesLayer.Interfaces
{
    public interface IGeoCoordsService
    {
        public bool CreateMiddledEventModel(MiddledEventModel model);

        public List<MiddledEventModel> initFile(string path, int id, int fileId, FileModel file);

        public List<EventLine> InitEvents(string[] input);

        public  void MiddlePointCalc(string[] input, int id,int fileId, FileModel file);

        public IEnumerable<MiddledEventModel> getEventsByFileId(int? id);

        public void DeleteByFileId(int id);

        public void DeleteByDroneId(int id);

        public bool ExistByFileId(int id);

        public bool CreateExifInfoModel(ExifInfoModel model);

        public IEnumerable<ExifInfoModel> getExifInfoModelsByFileId(int? id);

        public void DeleteExifInfoModelByFileID(int id);

        public void DeleteExifInfoModelByDroneID(int id);

        public void MatchCoords(List<MiddledEventModel> middledEvents, List<ExifInfoModel> exifInfoModels, double h, FileModel FileId);

        public void CreateMatchedModel(MatchedEventModel model);

        public void DeleteMatchedModelByFileId(int id);
    }
}
