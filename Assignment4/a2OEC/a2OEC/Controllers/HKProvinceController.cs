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
    public class HKProvinceController : Controller
    {
        private readonly OECContext _context;

        public HKProvinceController(OECContext context)
        {
            _context = context;
        }

        // GET: HKProvince
        public async Task<IActionResult> Index()
        {
            var oECContext = _context.Province.Include(p => p.CountryCodeNavigation);
            return View(await oECContext.ToListAsync());
        }

        // GET: HKProvince/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .Include(p => p.CountryCodeNavigation)
                .SingleOrDefaultAsync(m => m.ProvinceCode == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // GET: HKProvince/Create
        public IActionResult Create()
        {
          ViewData["CountryName"] = new SelectList(_context.Country.OrderBy(x => x.Name), "Name", "Name");
            
            return View();
        }

        // POST: HKProvince/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProvinceCode,Name,CountryCode")] Province province)
        {
            if (ModelState.IsValid)
            {
               province.CountryCode = _context.Country.SingleOrDefault(x => x.Name == province.CountryCode).CountryCode;
                _context.Add(province);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);

            string name = province.CountryCode = _context.Country.SingleOrDefault(p => p.CountryCode == province.CountryCode.ToString()).Name;
            ViewData["CountryName"] = new SelectList(_context.Country.OrderBy(x => x.Name), "Name", "Name", name);
            return View(province);
        }

        // GET: HKProvince/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province.SingleOrDefaultAsync(m => m.ProvinceCode == id);
            if (province == null)
            {
                return NotFound();
            }
            string name = province.CountryCode = _context.Country.SingleOrDefault(p => p.CountryCode == province.CountryCode.ToString()).Name;
            ViewData["CountryName"] = new SelectList(_context.Country.OrderBy(x => x.Name), "Name", "Name", name);
            return View(province);
        }

        // POST: HKProvince/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProvinceCode,Name,CountryCode")] Province province)
        {
            if (id != province.ProvinceCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    province.CountryCode = _context.Country.SingleOrDefault(x => x.Name == province.CountryCode).CountryCode;
                    _context.Update(province);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvinceExists(province.ProvinceCode))
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
            string name = province.CountryCode = _context.Country.SingleOrDefault(p => p.CountryCode == province.CountryCode.ToString()).Name;
            ViewData["CountryName"] = new SelectList(_context.Country.OrderBy(x => x.Name), "Name", "Name", name);
            return View(province);
        }

        // GET: HKProvince/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .Include(p => p.CountryCodeNavigation)
                .SingleOrDefaultAsync(m => m.ProvinceCode == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // POST: HKProvince/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var province = await _context.Province.SingleOrDefaultAsync(m => m.ProvinceCode == id);
            _context.Province.Remove(province);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvinceExists(string id)
        {
            return _context.Province.Any(e => e.ProvinceCode == id);
        }
    }
}
