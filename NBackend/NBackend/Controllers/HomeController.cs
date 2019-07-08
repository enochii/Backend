using NBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBackend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            using (NBackendContext dbContext = new NBackendContext())
            {
                if (!dbContext.Database.Exists())
                {
                    ViewBag.Title = "ERROR!";
                    dbContext.Database.Create();
                }
                else
                {
                    ViewBag.Title = "NP";
                }
            }

            return View();
        }
    }
}
