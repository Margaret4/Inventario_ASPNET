﻿using System;
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
    public class ventasController : Controller
    {
        private MINIMARKETEntities db = new MINIMARKETEntities();

        // GET: ventas
        public ActionResult Index()
        {
            var venta = db.venta.Include(v => v.vend);
            return View(venta.ToList());
        }

        // GET: ventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // GET: ventas/Create
        public ActionResult Create()
        {
            ViewBag.dnivend = new SelectList(db.vend, "dni", "nom");
            ViewBag.ReturnDate = DateTime.Now;
            return View();
        }

        // POST: ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tot,fecha,dnivend")] venta venta)
        {
            if (ModelState.IsValid)
            {
                db.venta.Add(venta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.dnivend = new SelectList(db.vend, "dni", "nom", venta.dnivend);
            return View(venta);
        }

        // GET: ventas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.dnivend = new SelectList(db.vend, "dni", "nom", venta.dnivend);
            return View(venta);
        }

        // POST: ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tot,fecha,dnivend")] venta venta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.dnivend = new SelectList(db.vend, "dni", "nom", venta.dnivend);
            return View(venta);
        }

        // GET: ventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }

        // POST: ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            venta venta = db.venta.Find(id);
            db.venta.Remove(venta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult MostrarDetalle(int id)
        {
            venta venta = db.venta.Find(id);

            ViewBag.dets= venta.detalle_v.ToList();
            return PartialView("MostrarDetalle");
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