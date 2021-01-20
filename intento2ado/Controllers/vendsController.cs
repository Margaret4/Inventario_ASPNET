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
    public class vendsController : Controller
    {
        private MINIMARKETEntities db = new MINIMARKETEntities();

        // GET: vends
        public ActionResult Index()
        {
            return View(db.vend.ToList());
        }

        // GET: vends/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vend vend = db.vend.Find(id);
            if (vend == null)
            {
                return HttpNotFound();
            }
            return View(vend);
        }

        // GET: vends/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: vends/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dni,nom,pwd")] vend vend)
        {
            if (ModelState.IsValid)
            {
                db.vend.Add(vend);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vend);
        }

        // GET: vends/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vend vend = db.vend.Find(id);
            if (vend == null)
            {
                return HttpNotFound();
            }
            return View(vend);
        }

        // POST: vends/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dni,nom,pwd")] vend vend)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vend);
        }

        // GET: vends/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vend vend = db.vend.Find(id);
            if (vend == null)
            {
                return HttpNotFound();
            }
            return View(vend);
        }

        // POST: vends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            vend vend = db.vend.Find(id);
            db.vend.Remove(vend);
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
