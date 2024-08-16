using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;
using DataAccessLayer;
using BussinesLayer.Services;
using DroneApplication.FileUploadService;
using BussinesLayer.Interfaces;

namespace DroneApplication.Controllers
{
    public class ExifInfoModelsController : Controller
    {
        private readonly DroneApplicationDbContext _context;
        private readonly IFileUploadService fileUploadService;
        private readonly IGeoCoordsService geoCoordsService;
        private readonly IDroneModelService droneModelService;
        private readonly IFileModelService fileModelService;

        public string FilePath; 


        public ExifInfoModelsController(DroneApplicationDbContext context,
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

        // GET: ExifInfoModels
        public async Task<IActionResult> Index()
        {
            return _context.FileModel.Where(o => o.Type == 2) != null ?
                        View(await _context.FileModel.Where(o => o.Type == 2).ToListAsync()) :
                        Problem("Entity set 'DroneApplicationContext.FileModel'  is null.");
        }

        // GET: ExifInfoModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FileModel == null)
            {
                return NotFound();
            }

            IEnumerable<ExifInfoModel> exifInfoModels = geoCoordsService.getExifInfoModelsByFileId(id);
            if (exifInfoModels == null)
            {
                return NotFound();
            }
            FileModel model = fileModelService.GetFile(id);
            ViewData["Name"] = model.Name;
            return View(exifInfoModels);
        }

        // GET: ExifInfoModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExifInfoModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExifFileCategory category)
        {
            if (ModelState.IsValid)
            {
                if (category.fileCollection != null)
                {
                    if (!_context.FileModel.Any(o => o.Name == category.fileModel.Name))
                    {
                        category.fileModel.Type = 2;//ExifType
                        if (!_context.DroneModel.Any(o => o.Name == category.droneModel.Name))
                        {
                            droneModelService.CreateDrone(category.droneModel);
                        }
                        else
                        {
                            category.droneModel = droneModelService.GetByName(category.droneModel.Name);
                        }
                        category.fileModel.DroneId = category.droneModel.Id;
                        await fileModelService.CreateFile(category.fileModel);
                        foreach (IFormFile i in category.fileCollection)
                        {

                            FilePath = await fileUploadService.UploadFileAsync(i);

                            ExifInfoModel exifInfoModel = new ExifInfoModel();
                            exifInfoModel = fileModelService.ExtractExif(FilePath);
                            exifInfoModel.DroneId = category.droneModel.Id;
                            exifInfoModel.FileId = category.fileModel.Id;
                            exifInfoModel.name = i.FileName;

                            geoCoordsService.CreateExifInfoModel(exifInfoModel);
                        }
                    }
                    
                }
                return RedirectToAction(nameof(Index));
            }



            return View(category.droneModel);
        }

        // GET: ExifInfoModels/Edit/5
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
            ViewData["Name"] = fileModel.Name;
            return View(fileModel);
        }

        // POST: Middling/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DroneId,Type")] FileModel fileModel)
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

        // GET: ExifInfoModels/Delete/5
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

        // POST: ExifInfoModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FileModel == null)
            {
                return Problem("Entity set 'DroneApplicationContext.FileModel'  is null.");
            }
            fileModelService.DeleteFile(id);
            geoCoordsService.DeleteExifInfoModelByFileID(id);
            return RedirectToAction(nameof(Index));
        }

        private bool FileModelExists(int id)
        {
            return (_context.FileModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
