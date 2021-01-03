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
    public class detalle_vController : Controller
    {
        private MINIMARKETEntities db = new MINIMARKETEntities();

        // GET: detalle_v
        public ActionResult Index()
        {
            var detalle_v = db.detalle_v.Include(d => d.prod).Include(d => d.venta1);
            return View(detalle_v.ToList());
        }

        // GET: detalle_v/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detalle_v detalle_v = db.detalle_v.Find(id);
            if (detalle_v == null)
            {
                return HttpNotFound();
            }
            return View(detalle_v);
        }

        // GET: detalle_v/Create
        public ActionResult Create()
        {
            ViewBag.produc = new SelectList(db.prod, "id", "nom");
            ViewBag.venta = new SelectList(db.venta, "id", "dnivend");
            return View();
        }

        // POST: detalle_v/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,produc,canti,venta,monto")] detalle_v detalle_v)
        {
            if (ModelState.IsValid)
            {
                db.detalle_v.Add(detalle_v);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.produc = new SelectList(db.prod, "id", "nom", detalle_v.produc);
            ViewBag.venta = new SelectList(db.venta, "id", "dnivend", detalle_v.venta);
            return View(detalle_v);
        }

        // GET: detalle_v/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detalle_v detalle_v = db.detalle_v.Find(id);
            if (detalle_v == null)
            {
                return HttpNotFound();
            }
            ViewBag.produc = new SelectList(db.prod, "id", "nom", detalle_v.produc);
            ViewBag.venta = new SelectList(db.venta, "id", "dnivend", detalle_v.venta);
            return View(detalle_v);
        }

        // POST: detalle_v/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,produc,canti,venta,monto")] detalle_v detalle_v)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalle_v).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.produc = new SelectList(db.prod, "id", "nom", detalle_v.produc);
            ViewBag.venta = new SelectList(db.venta, "id", "dnivend", detalle_v.venta);
            return View(detalle_v);
        }

        // GET: detalle_v/Delete/5
        public ActionResult DeleteDet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detalle_v detalle_v = db.detalle_v.Find(id);
            if (detalle_v == null)
            {
                return HttpNotFound();
            }
            return View(detalle_v);
        }

        // POST: detalle_v/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            detalle_v detalle_v = db.detalle_v.Find(id);
            db.detalle_v.Remove(detalle_v);
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
