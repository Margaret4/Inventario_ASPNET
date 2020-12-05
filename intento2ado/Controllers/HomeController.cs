using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using intento2ado.Models;

namespace intento2ado.Controllers
{
    public class HomeController : Controller
    {
        
        MINIMARKETEntities _db;
        public HomeController()
        {
            _db = new MINIMARKETEntities();
        }

        public ActionResult Index()
        {
            ViewData.Model = _db.prod.ToList();
            ViewData.Model = (from m in _db.prod select m).ToList();
            //var categs = _db.categ.ToList();
            //categs = (from m in _db.categ select m).ToList();
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}