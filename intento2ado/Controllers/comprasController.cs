using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using intento2ado.Models;

namespace intento2ado.Controllers
{
    public class comprasController : Controller
    {
        private MINIMARKETEntities db = new MINIMARKETEntities();

        // GET: compras
        public ActionResult Index()
        {
            var compra = db.compra.Include(c => c.prov1);
            return View(compra.ToList());
        }

        // GET: compras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            compra compra = db.compra.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        // GET: compras/Create
        public ActionResult Create()
        {
            ViewBag.prov = new SelectList(db.prov, "id", "nom");
            return View();
        }

        // POST: compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( compra compra)
        {
            int tmp = 1;
            try { 
                tmp += db.detalle_c.Max(d => d.id); }
            catch (Exception e)
            {
                tmp = 1;
                string msj = e.ToString();
            }
            finally
            {
                compra.id = tmp;
                compra.fecha = DateTime.Now;
                compra.tot = 0;
                db.compra.Add(compra);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
            //ViewBag.prov = new SelectList(db.prov, "id", "nom", compra.prov);
            //return View(compra);
        }

        // GET: compras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            compra compra = db.compra.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            ViewBag.prov = new SelectList(db.prov, "id", "nom", compra.prov);
            return View(compra);
        }

        // POST: compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(compra compra)
        {

            db.Entry(compra).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }

        // GET: compras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            compra compra = db.compra.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        // POST: compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            compra compra = db.compra.Find(id);
            db.compra.Remove(compra);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
