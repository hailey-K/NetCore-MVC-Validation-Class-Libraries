using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using a2OEC.Models;

namespace a2OEC.Controllers
{
    public class HKFarmController : Controller
    {
        private readonly OECContext _context;

        public HKFarmController(OECContext context)
        {
            _context = context;
        }

        // GET: HKFarm
        public async Task<IActionResult> Index()
        {
            var oECContext = _context.Farm.Include(f => f.ProvinceCodeNavigation).OrderBy(f => f.Name);
            return View(await oECContext.ToListAsync());
        }

        // GET: HKFarm/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farm = await _context.Farm
                .Include(f => f.ProvinceCodeNavigation)
                .SingleOrDefaultAsync(m => m.FarmId == id);
            if (farm == null)
            {
                return NotFound();
            }

            return View(farm);
        }

        // GET: HKFarm/Create
        public IActionResult Create()
        {
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode");
            return View();
        }

        // POST: HKFarm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FarmId,Name,Address,Town,County,ProvinceCode,PostalCode,HomePhone,CellPhone,Email,Directions,DateJoined,LastContactDate")] Farm farm)
        {
    
            if (ModelState.IsValid)
            {
            try
            {
                    _context.Add(farm);
                    @TempData["SUCCESS"] = "Insert the data successfully !";
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",$"error inserting new order: {ex.GetBaseException().Message}");
            }
               
            }
            return View(farm);
        }

        // GET: HKFarm/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farm = await _context.Farm.SingleOrDefaultAsync(m => m.FarmId == id);
            if (farm == null)
            {
                return NotFound();
            }
           ViewData["ProvinceName"] = new SelectList(_context.Province.OrderBy(p => p.Name), "ProvinceCode", "Name", farm.ProvinceCode);
            return View(farm);
        }

        // POST: HKFarm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FarmId,Name,Address,Town,County,ProvinceCode,PostalCode,HomePhone,CellPhone,Email,Directions,DateJoined,LastContactDate")] Farm farm)
        {
           
                if (id != farm.FarmId)
                {
                    return NotFound();
                }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farm);


                    TempData["SUCCESS"] = "Edit the data successfully !";
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmExists(farm.FarmId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"error inserting new order: {ex.GetBaseException().Message}");
                }
            }
            ViewData["ProvinceName"] = new SelectList(_context.Province, "ProvinceCode", "Name", farm.ProvinceCode);
            return View(farm);
        }

        // GET: HKFarm/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farm = await _context.Farm
                .Include(f => f.ProvinceCodeNavigation)
                .SingleOrDefaultAsync(m => m.FarmId == id);
            if (farm == null)
            {
                return NotFound();
            }

            return View(farm);
        }

        // POST: HKFarm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var farm = await _context.Farm.SingleOrDefaultAsync(m => m.FarmId == id);
                _context.Farm.Remove(farm);
                TempData["SUCCESS"] = "Delete the data successfully !";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorDeleteFarm"] = ex.GetBaseException().Message;
                return RedirectToAction("Delete", "HKFarm");
                throw;
            }
           
        }

        private bool FarmExists(int id)
        {
            return _context.Farm.Any(e => e.FarmId == id);
        }
    }
}
