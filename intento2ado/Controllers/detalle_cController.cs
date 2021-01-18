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
    public class detalle_cController : Controller
    {
        private MINIMARKETEntities db = new MINIMARKETEntities();

        // GET: detalle_c
        public ActionResult Index(int id)
        {
            var detalle_c = db.detalle_c.Include(d => d.prod).Where(d=>d.compra==id);
            ViewBag.compra = id;
            ViewBag.tot = db.compra.Find(id).tot;
            return View(detalle_c.ToList());
        }

        // GET: detalle_c/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detalle_c detalle_c = db.detalle_c.Find(id);
            if (detalle_c == null)
            {
                return HttpNotFound();
            }
            return View(detalle_c);
        }

        // GET: detalle_c/Create
        public ActionResult Create(int compra)
        {
            ViewBag.compra = compra;
            var prods1 =
                from p in db.prod
                join u in db.detalle_c.Where(detalle_c => detalle_c.compra== compra)
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
            return View();
        }

        // POST: detalle_c/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(detalle_c detalle_c)
        {
            
            
            int tmp=  1;
            try { tmp += db.detalle_c.Max(d => d.id); }
            catch (Exception e) {
                tmp = 1;
                string msj = e.ToString();
            }
            finally
            {
                detalle_c.id = tmp;
                detalle_c.tot = detalle_c.prec_unit * detalle_c.canti;
                db.detalle_c.Add(detalle_c);
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "compras", new { id = detalle_c.compra });
            
            //ViewBag.produc = new SelectList(db.prod, "id", "nom", detalle_c.produc);
            //return View(detalle_c);
        }

        // GET: detalle_c/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detalle_c detalle_c = db.detalle_c.Find(id);
            if (detalle_c == null)
            {
                return HttpNotFound();
            }
            var prods1 =
                from p in db.prod
                join u in db.detalle_c.Where(deta_c => deta_c.compra == detalle_c.compra)
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
            ViewBag.compra = detalle_c.compra;
            return View(detalle_c);
            
        }

        // POST: detalle_c/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(detalle_c detalle_c)
        {
            int? tmp = db.detalle_c.Find(detalle_c.id).compra;
            detalle_c.compra = tmp;            
            db.Entry(detalle_c).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "compras", new { id = detalle_c.compra });

        }

        // GET: detalle_c/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detalle_c detalle_c = db.detalle_c.Find(id);
            if (detalle_c == null)
            {
                return HttpNotFound();
            }
            return View(detalle_c);
        }

        // POST: detalle_c/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            detalle_c detalle_c = db.detalle_c.Find(id);
            db.detalle_c.Remove(detalle_c);
            db.SaveChanges();
            return RedirectToAction("Edit", "compras", new { id = detalle_c.compra });
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
