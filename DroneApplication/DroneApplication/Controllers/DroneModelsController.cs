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
    public class DroneModelsController : Controller
    {
        private readonly DroneApplicationDbContext _context;
        private readonly IFileUploadService fileUploadService;
        private readonly IGeoCoordsService geoCoordsService;
        private readonly IDroneModelService droneModelService;
        private readonly IFileModelService fileModelService;

        public DroneModelsController(DroneApplicationDbContext context,
            IFileUploadService fileUploadService,
            IGeoCoordsService geoCoordsFile,
            IDroneModelService droneModelService,
            IFileModelService fileModelService)
        {
            _context = context;
            this.fileModelService = fileModelService;
            this.geoCoordsService = geoCoordsFile;
            this.droneModelService = droneModelService;
            this.fileUploadService = fileUploadService;
        }

        // GET: DroneModels
        public async Task<IActionResult> Index()
        {
              return _context.DroneModel != null ? 
                          View(await _context.DroneModel.ToListAsync()) :
                          Problem("Entity set 'DroneApplicationContext.DroneModel'  is null.");
        }

        // GET: DroneModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DroneModel == null)
            {
                return NotFound();
            }

            var droneModel = await _context.DroneModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (droneModel == null)
            {
                return NotFound();
            }

            ViewData["DroneName"] = droneModel.Name;
           
            return View(droneModel) ;
        }

        // GET: DroneModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DroneModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,categoryZone")] DroneModel droneModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(droneModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(droneModel);
        }

        // GET: DroneModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DroneModel == null)
            {
                return NotFound();
            }

            var droneModel = await _context.DroneModel.FindAsync(id);
            if (droneModel == null)
            {
                return NotFound();
            }
            return View(droneModel);
        }

        // POST: DroneModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,categoryZone")] DroneModel droneModel)
        {
            if (id != droneModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(droneModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DroneModelExists(droneModel.Id))
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
            return View(droneModel);
        }

        // GET: DroneModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DroneModel == null)
            {
                return NotFound();
            }

            var droneModel = await _context.DroneModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (droneModel == null)
            {
                return NotFound();
            }

            return View(droneModel);
        }

        // POST: DroneModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DroneModel == null)
            {
                return Problem("Entity set 'DroneApplicationContext.DroneModel'  is null.");
            }
            var droneModel = await _context.DroneModel.FindAsync(id);
            if (droneModel != null)
            {
                droneModelService.DeleteDrone(id);
                fileModelService.DeleteFilesByDroneId(id);
                geoCoordsService.DeleteByDroneId(id);
                geoCoordsService.DeleteExifInfoModelByDroneID(id);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DroneModelExists(int id)
        {
          return (_context.DroneModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
