using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinesLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using BussinesLayer;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Security.Cryptography;

namespace BussinesLayer.Services
{
    public class GeoCoordsService : IGeoCoordsService
    {
        private IMiddledEventRepository _middledEventRepository;
        

        public GeoCoordsService(IMiddledEventRepository _middledEventRepository)
        {
                this._middledEventRepository = _middledEventRepository;
        }

        public bool CreateMiddledEventModel(MiddledEventModel model)
        {
            return _middledEventRepository.CreateMiddledEvent(model);

        }


        public List<MiddledEventModel> initFile(string path, int id, int fileId, FileModel file)
        {
            try
            {
                if (File.Exists(path))
                {
                    MiddlePointCalc(File.ReadAllLines(path), id, fileId,file);
                }
                else
                {
                    throw new FileNotFoundException();
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("FileNotFound");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in file");
            }
            return null;
        }


        public List<EventLine> InitEvents(string[] input)
        {
            List<EventLine> events = new List<EventLine>();
            int br = 1;

            foreach (string line in input)
            {
                if (br < 27)
                {
                    br++;
                    continue;
                }
                EventLine temp = new EventLine(line);
                events.Add(temp);
                
            }
            return events;
        }
        public void MiddlePointCalc(string[] input, int id,  int fileId,FileModel file)
        {
            List<EventLine> events = InitEvents(input);
            List<EventLine> singlePoints = new List<EventLine>();
            List<MiddledEventModel> MiddleddEventModels = new List<MiddledEventModel>();
            Console.WriteLine(events.Count);

            #region Variables
            double h = 0f;
            TimeSpan diftime;
            TimeSpan diftimeSum = TimeSpan.Zero;
            double DeltaX;
            double DeltaY;
            double DeltaZ;
            double s;
            double s1;
            double offset = 0.05;
            double Xmid;
            double Ymid;
            double Zmid;
            #endregion


            for (int i = 0; i < events.Count; i++)
            {
                h = events[i].H + events[i + 1].H;
                diftime = events[i+1].time - events[i].time;
                if (diftime.TotalMilliseconds >= 350 && diftime.TotalMilliseconds <= 650)
                {
                    DeltaX = events[i+1].X - events[i].X;
                    DeltaY = events[i+1].Y - events[i].Y;
                    DeltaZ = events[i+1].Z - events[i].Z;

                    events.Remove(events[i + 1]);
                    s = Math.Sqrt(Math.Pow(DeltaX, 2) + Math.Pow(DeltaY, 2) + Math.Pow(DeltaZ, 2));

                    if (diftime.TotalMilliseconds >= 350 && diftime.TotalMilliseconds <= 450)
                    {
                        s1 = s / 2 + offset;
                        diftime /= 2;
                    }
                    else
                    {
                        s1 = s / 3 + offset;
                        diftime /= 3;
                        diftime *= 2;
                    }

                    Xmid = (DeltaX / s) * s1 + (events[i].X);
                    Ymid = (DeltaY / s) * s1 + (events[i].Y);
                    Zmid = (DeltaZ / s) * s1 + (events[i].Z);

                    diftimeSum += diftime;

                    MiddledEventModel temp = new MiddledEventModel();
                    temp.Xmid = Xmid;
                    temp.Ymid = Ymid;
                    temp.Zmid = Zmid;
                    MiddleddEventModels.Add(temp);
                }
            }
            file.h = h;
            foreach (MiddledEventModel i in MiddleddEventModels)
            {
                i.DroneId = id;
                i.FileId = fileId;
                CreateMiddledEventModel(i);
            }
        }

        public IEnumerable<MiddledEventModel> getEventsByFileId(int? id)
        {
            return _middledEventRepository.getEventsByFileId(id);
        }

        public void DeleteByFileId(int id) {
           _middledEventRepository.DeleteByFileId(id);
        }

        public void DeleteByDroneId(int id)
        {
            _middledEventRepository.DeleteByDroneId(id);
        }

        public bool ExistByFileId(int id)
        {
            return _middledEventRepository.ExistByFileId(id);
        }

        public bool CreateExifInfoModel(ExifInfoModel model)
        {
            return _middledEventRepository.CreateExifInfoModel(model);
        }

        public IEnumerable<ExifInfoModel> getExifInfoModelsByFileId(int? id) {
            return _middledEventRepository.getExifInfoModelsByFileId(id);
        }

        public void DeleteExifInfoModelByFileID(int id) {
            _middledEventRepository.DeleteExifInfoModelByFileID(id);
        }

        public void DeleteExifInfoModelByDroneID(int id) {
            _middledEventRepository.DeleteExifInfoModelByDroneID(id);
        }

        public void MatchCoords(List<MiddledEventModel> middledEvents, List<ExifInfoModel> exifInfoModels,double h,FileModel FileId) {
            
            List<EventLine> eventLines = new List<EventLine>();
            List<MatchedEventModel> mathcedList = new List<MatchedEventModel>();
            foreach (ExifInfoModel i in exifInfoModels)
            {
                EventLine temp = new EventLine(i.gps_latitude,i.gps_longtitude,i.gps_altitude,h);
                eventLines.Add(temp);
            }
            double s;
			int br = 0;
            string name = "";
			foreach (EventLine low in eventLines) {
                EventLine match = eventLines[0];
                s = Math.Sqrt(Math.Pow((middledEvents[0].Xmid - low.X), 2) + Math.Pow((middledEvents[0].Ymid - low.Y), 2) + Math.Pow((middledEvents[0].Zmid - low.Z), 2));
                foreach (MiddledEventModel mid in  middledEvents) {
                    double tempS = Math.Sqrt(Math.Pow((mid.Xmid-low.X),2) + Math.Pow((mid.Ymid - low.Y), 2) + Math.Pow((mid.Zmid - low.Z), 2));

                    if (tempS < s)
                    {
                        s = tempS;
                        match = low;
                        name = exifInfoModels[br].name;
                        //Do stuff with the match
                    }
                }
                br++;
                MatchedEventModel matchedeventModel = new MatchedEventModel();
                matchedeventModel.Name = name;
                matchedeventModel.X = match.X;
                matchedeventModel.Y = match.Y;
                matchedeventModel.Z = match.Z;
                matchedeventModel.FileId = FileId.Id;

                CreateMatchedModel(matchedeventModel);
            }
        }

		public void CreateMatchedModel(MatchedEventModel model)
		{
            _middledEventRepository.CreateMatchedModel(model);
		}

        public void DeleteMatchedModelByFileId(int id) {
            _middledEventRepository.DeleteMatchedModelByFileId(id);
        }



    }
}
