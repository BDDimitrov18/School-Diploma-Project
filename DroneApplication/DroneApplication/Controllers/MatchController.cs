using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;
using DataAccessLayer;
using BussinesLayer.Interfaces;
using DroneApplication.FileUploadService;

namespace DroneApplication.Controllers
{
    public class MatchController : Controller
    {
        private readonly DroneApplicationDbContext _context;
        private readonly IFileUploadService fileUploadService;
        private readonly IGeoCoordsService geoCoordsService;
        private readonly IDroneModelService droneModelService;
        private readonly IFileModelService fileModelService;
        public MatchController(DroneApplicationDbContext context,
            IFileUploadService fileUploadService,
            IGeoCoordsService geoCoordsFile,
            IDroneModelService droneModelService,
            IFileModelService fileModelService)
        {
            _context = context;
            this.fileUploadService = fileUploadService;
            this.geoCoordsService = geoCoordsFile;
            this.droneModelService = droneModelService;
            this.fileModelService = fileModelService;
        }

        // GET: Match
        public async Task<IActionResult> Index()
        {
            return _context.FileModel.Where(o => o.Type == 3) != null ?
                       View(await _context.FileModel.Where(o => o.Type == 3).ToListAsync()) :
                       Problem("Entity set 'DroneApplicationContext.FileModel'  is null.");
        }

        // GET: Match/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FileModel == null)
            {
                return NotFound();
            }

            var fileModel = await _context.MatchedEventModels
                .ToListAsync();
            if (fileModel == null)
            {
                return NotFound();
            }

            return View(fileModel);
        }

        // GET: Match/Create
        public IActionResult Create()
        {
            var exifData = _context.FileModel.Where(i => i.Type == 2).ToList();
            var midData = _context.FileModel.Where(i => i.Type == 1).ToList();
            MatchCategory model = new MatchCategory();
            model.exifFilesList = new List<SelectListItem>();
            model.midFilesList = new List<SelectListItem>();
            foreach (var i in exifData) {
                model.exifFilesList.Add(new SelectListItem {Text = i.Name, Value = i.Id.ToString() });
            }
            foreach (var i in midData)
            {
                model.midFilesList.Add(new SelectListItem { Text = i.Name, Value = i.Id.ToString() });
            }
            return View(model);
        }

        // POST: Match/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] MatchCategory fileModel)
        {
            if (ModelState.IsValid)
            {
                FileModel midFile = new FileModel();
                FileModel exifFile = new FileModel();
                midFile = fileModelService.GetFile(int.Parse(fileModel.midFileId));
                exifFile = fileModelService.GetFile(int.Parse(fileModel.exifFileId));
                FileModel newFile = new FileModel();
                newFile.Name = fileModel.newFileName;
                newFile.Type = 3;
                fileModelService.CreateFile(newFile);
                List<MiddledEventModel> middledEvents = new List<MiddledEventModel>();
                List<ExifInfoModel> exifInfoModels = new List<ExifInfoModel>();


                middledEvents = geoCoordsService.getEventsByFileId(midFile.Id).ToList();
                exifInfoModels = geoCoordsService.getExifInfoModelsByFileId(exifFile.Id).ToList();

                geoCoordsService.MatchCoords(middledEvents,exifInfoModels,(double)midFile.h,newFile);
                
            }
            return View(fileModel);
        }

        // GET: Match/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FileModel == null)
            {
                return NotFound();
            }

            var fileModel = await _context.FileModel.FindAsync(id);
            if (fileModel == null)
            {
                return NotFound();
            }
            return View(fileModel);
        }

        // POST: Match/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DroneId,Type,h")] FileModel fileModel)
        {
            if (id != fileModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileModelExists(fileModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fileModel);
        }

        // GET: Match/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FileModel == null)
            {
                return NotFound();
            }

            var fileModel = await _context.FileModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileModel == null)
            {
                return NotFound();
            }

            return View(fileModel);
        }

        // POST: Match/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FileModel == null)
            {
                return Problem("Entity set 'DroneApplicationContext.FileModel'  is null.");
            }
            var fileModel = await _context.FileModel.FindAsync(id);
            if (fileModel != null)
            {
                geoCoordsService.DeleteMatchedModelByFileId(fileModel.Id);
                _context.FileModel.Remove(fileModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileModelExists(int id)
        {
          return (_context.FileModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
