using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DroneApplication.FileUploadService;
using BussinesLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;

namespace DroneApplication.Controllers
{
    public class MiddlingController : Controller
    {
        private readonly DroneApplicationDbContext _context;
        private readonly IFileUploadService fileUploadService;
        private readonly IGeoCoordsService geoCoordsService;
        private readonly IDroneModelService droneModelService;
        private readonly IFileModelService fileModelService;

        public string FilePath;

       
        public MiddlingController(DroneApplicationDbContext context,
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

        // GET: Middling
        public async Task<IActionResult> Index()
        {
              return _context.FileModel.Where(o => o.Type == 1) != null ? 
                          View(await _context.FileModel.Where(o => o.Type == 1).ToListAsync()) :
                          Problem("Entity set 'DroneApplicationContext.FileModel'  is null.");
        }

        // GET: Middling/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FileModel == null)
            {
                return NotFound();
            }

            IEnumerable<MiddledEventModel> events = geoCoordsService.getEventsByFileId(id);
            if (events == null)
            {
                return NotFound();
            }
            FileModel model = fileModelService.GetFile(id);
            ViewData["Name"] = model.Name;
            return View(events);
        }

        // GET: Middling/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Middling/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] MiddlingFileCategory category)
        {
            
            if (ModelState.IsValid)
            {
                
                if (category.file != null)
                {
                  
                    FilePath = await fileUploadService.UploadFileAsync(category.file);
                    if(!_context.FileModel.Any(o => o.Name == category.file.FileName)) {
                        if (!_context.DroneModel.Any(o => o.Name == category.droneModel.Name))
                        {
                            droneModelService.CreateDrone(category.droneModel);
                        }
                        else {
                            category.droneModel = droneModelService.GetByName(category.droneModel.Name);
                        }
                        FileModel fileModel = new FileModel();
                        fileModel.DroneId = category.droneModel.Id;
                        fileModel.Name = category.file.FileName;
                        fileModel.Type = 1;
                        await fileModelService.CreateFile(fileModel);
                        _context.SaveChanges();
                        geoCoordsService.initFile(FilePath, category.droneModel.Id, fileModel.Id, fileModel);
                        _context.Update(fileModel);
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            
            
            return View(category.droneModel);
        }

        // GET: Middling/Edit/5
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

        // GET: Middling/Delete/5
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

        // POST: Middling/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FileModel == null)
            {
                return Problem("Entity set 'DroneApplicationContext.FileModel'  is null.");
            }
            geoCoordsService.DeleteByFileId(id);
            fileModelService.DeleteFile(id);
            return RedirectToAction(nameof(Index));
        }

        private bool FileModelExists(int id)
        {
          return (_context.FileModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public void OnPost() { 
        
        }
    }
}
