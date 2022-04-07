using CrashUno.Models;
using CrashUno.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CrashUno.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repo;

        public HomeController (IRepository temp)
        {
            repo = temp;
        }
        
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Crash(int crashseverityid = 0, int pageNum = 1)
        {
            int pageSize = 13;

            var x = new CrashViewModel
            {
                Crash = repo.Crash
                .Where(c => c.crash_severity_id == crashseverityid || crashseverityid == 0)
                .OrderBy(c => c.crash_id)
                .Include(x => x.location)
                .Skip ((pageNum - 1) * pageSize)
                .Take(pageSize),


                PageInfo = new PageInfo
                {
                    TotalNumCrashes = 
                        (crashseverityid == 0
                        ? repo.Crash.Count()
                        : repo.Crash.Where(c => c.crash_severity_id == crashseverityid).Count()),
                    CrashesPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };
            
            return View(x);
        }
        public IActionResult Location(string searchString = "", int pageNum = 1)
        {
            int pageSize = 4;

            var y = new LocationViewModel
            {
                Location = repo.Location
                .OrderBy(l => l.loc_id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                LocationPageInfo = new LocationPageInfo
                {
                    TotalNumLocations = repo.Location.Count(),
                    LocationsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };
            if (searchString != "")
            {
                pageSize = 4;
                y.Location = repo.Location.Where(x => x.city == searchString)
                    .Skip((pageNum - 1) * pageSize) //LOCATION SEARCH ONLY RETURNS 1 RECORD per SEARCH PLZ HELP
                    .Take(pageSize);
            };
            
            return View(y);
        }

    }
}
