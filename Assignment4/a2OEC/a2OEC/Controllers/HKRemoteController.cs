using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using a2OEC.Models;
using System.Text.RegularExpressions;

namespace a2OEC.Controllers
{
    public class HKRemoteController : Controller
    {
        private readonly OECContext _context;
        public IActionResult Index()
        {
            return View();
        }
        public HKRemoteController(OECContext context)
        {
            _context = context;
        }
        public JsonResult checkProvinceCode(String provinceCode)
        {
            try
            {
               int countProvinceCode = _context.Province.Count(p=>p.Name == provinceCode);

                 if (countProvinceCode == 0)
                    {
                    if (provinceCode.Length != 2 || !Regex.IsMatch(provinceCode, @"^[a-zA-Z]+$"))
                    {
                        return Json("Province Code must  be exactly 2 letters long.");
                    }

                    if (_context.Province.Count(p => p.ProvinceCode == provinceCode) == 0)
                    {
                        return Json("The province code doesn't exist. please check the province code again.");
                    }
                   }
                return Json(true);
           }
            catch (Exception)
            {
                return Json("Error validating province Code.");
                throw;
            }
    }
    public JsonResult CheckDates(DateTime LastContactDate, DateTime DateJoined)
        {
            //ii.	lastContactDate cannot be provided unless dateJoined is also provided, but dateJoined can be provided without lastContactDate.
            if(LastContactDate!=null && (DateJoined == null || DateJoined == default(DateTime)))
            {
                return Json("Last contact date cannot be provided unless date Joined is also provided.");
            }
            //            iii.A farmer cannot be contacted by marketing before they have joined the program.
            if (DateJoined > LastContactDate)
            {
                return Json("Please check the dates of date joined and last contact date.");
            }
            return Json(true);
        }

    }
}