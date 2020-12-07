using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using intento2ado.Models;

namespace intento2ado.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor

        MINIMARKETEntities _db;
        public ProveedorController()
        {
            _db = new MINIMARKETEntities();
        }

        public ActionResult Index()
        {

            List<categ> categs = _db.categ.ToList();
            
            //categs = (from m in _db.categ select m).ToList();
            return View(categs);
        }
        public ActionResult NuevoProv()
        {
            ViewBag.cats = new SelectList(_db.categ, "id", "nom", "1");//dice datavaluefield :v es el nombre del campo en el db

            return View();
        }
        [HttpPost]
        public ActionResult NuevoProv(prov newProv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MINIMARKETEntities db = new MINIMARKETEntities())
                    {

                        db.prov.Add(newProv);
                        
                        db.SaveChanges();
                    }
                    return Redirect("/Proveedor");

                }
                return View();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }




        public ActionResult DeleteProv(int id)
        {
            var oProv = _db.prov.Find(id);
            _db.prov.Remove(oProv);
            _db.SaveChanges();

            return Redirect("/Proveedor");
        }

        public ActionResult EditProv(int id)
        {

            var oProv = _db.prov.Find(id);

            ViewBag.cats = new SelectList(_db.categ, "id", "nom", oProv.cat.ToString());//dice datavaluefield :v es el nombre del campo en el db 
            return View(oProv);
        }
        [HttpPost]
        public ActionResult EditProv(prov newProv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (MINIMARKETEntities db = new MINIMARKETEntities())
                    {

                        _db.Entry(newProv).State = System.Data.Entity.EntityState.Modified; ;
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



    }
}
