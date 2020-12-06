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
            
            List<categ> categs = _db.categ.ToList();

            //categs = (from m in _db.categ select m).ToList();
            return View(categs);
        }
        public ActionResult NuevoProd(){
            ViewBag.cats = new SelectList(_db.categ, "id", "nom","1");//dice datavaluefield :v es el nombre del campo en el db

            return View();
        }
        [HttpPost]
        public ActionResult NuevoProd(prod newProd)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MINIMARKETEntities db = new MINIMARKETEntities())
                    {
                        
                        _db.prod.Add(newProd);
                        _db.SaveChanges();
                    }
                    return Redirect("/Home");

                }
                return View();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
        }




        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}