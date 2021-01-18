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
        public ActionResult Index(int? venta)
        {
            var detalle_v = db.detalle_v.Include(d => d.prod).Include(d => d.venta1).Where(d=> d.venta==venta);
            ViewBag.tot = db.venta.Find(venta).tot;
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
        public ActionResult Create(int venta)
        {
            var prods1 =
            from p in db.prod
            join u in db.detalle_v.Where(detalle_v => detalle_v.venta == venta)
            on new { detalle_c = p.id } equals new { detalle_c = u.produc } into lj
            from x in lj.DefaultIfEmpty()
            where x.produc == null
            select new
            {
                id = p.id,
                nom = p.nom
            };
            List<prod> prods = new List<prod>();
            foreach (var i in prods1)
            {
                prods.Add(db.prod.Find(i.id));
            }
            ViewBag.produc = new SelectList(prods, "id", "nom");
            ViewBag.venta = venta;
            return View();
        }

        // POST: detalle_v/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,produc,venta,canti,monto")] detalle_v detalle_v)
        {
            if (ModelState.IsValid)
            {
                db.detalle_v.Add(detalle_v);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var prods1 =
            from p in db.prod
            join u in db.detalle_v.Where(deta_v => deta_v.venta == detalle_v.venta)
            on new { detalle_c = p.id } equals new { detalle_c = u.produc } into lj
            from x in lj.DefaultIfEmpty()
            where x.produc == null
            select new
            {
                id = p.id,
                nom = p.nom
            };
            List<prod> prods = new List<prod>();
            foreach (var i in prods1)
            {
                prods.Add(db.prod.Find(i.id));
            }
            ViewBag.produc = new SelectList(prods, "id", "nom");
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
            var prods1 =
            from p in db.prod
            join u in db.detalle_v.Where(deta_v => deta_v.venta == detalle_v.venta)
            on new { detalle_c = p.id } equals new { detalle_c = u.produc } into lj
            from x in lj.DefaultIfEmpty()
            where x.produc == null
            select new
            {
                id = p.id,
                nom = p.nom
            };
            List<prod> prods = new List<prod>();
            foreach (var i in prods1)
            {
                prods.Add(db.prod.Find(i.id));
            }
            ViewBag.produc = new SelectList(prods, "id", "nom");
            ViewBag.venta = new SelectList(db.venta, "id", "dnivend", detalle_v.venta);
            return View(detalle_v);
        }

        // POST: detalle_v/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,produc,venta,canti,monto")] detalle_v detalle_v)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalle_v).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Ventas", new { id=detalle_v.venta});
            }
            var prods1 =
            from p in db.prod
            join u in db.detalle_v.Where(deta_v => deta_v.venta == detalle_v.venta)
            on new { detalle_c = p.id } equals new { detalle_c = u.produc } into lj
            from x in lj.DefaultIfEmpty()
            where x.produc == null
            select new
            {
                id = p.id,
                nom = p.nom
            };
            List<prod> prods = new List<prod>();
            foreach (var i in prods1)
            {
                prods.Add(db.prod.Find(i.id));
            }
            ViewBag.produc = new SelectList(prods, "id", "nom");
            ViewBag.venta = new SelectList(db.venta, "id", "dnivend", detalle_v.venta);
            return View(detalle_v);
        }

        // GET: detalle_v/Delete/5
        public ActionResult Delete(int? id)
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
            return RedirectToAction("Edit", "Venta", new { id = detalle_v.venta });
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
