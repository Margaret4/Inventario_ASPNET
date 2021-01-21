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
        public ActionResult Index(int? id)
        {
            var detalle_c1 = db.detalle_c.Include(d => d.compra1).Include(d => d.prod).Where(d=>d.compra==id);
            
            ViewBag.compra = id;
            ViewBag.tot=db.compra.Find(id).tot;
            return View(detalle_c1.ToList());
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
        public ActionResult Create(int? compra)
        {
            ViewBag.compra = compra;
            var prods1 =
            from p in db.prod
            join u in db.detalle_c.Where(detalle_c => detalle_c.compra == compra)
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
                //prods.Add(db.prod.Find(i.id));
            }

            ViewBag.produc = new SelectList(db.prod.ToList(), "id", "nom");
            return View();
        }

        // POST: detalle_c/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(detalle_c detalle_c)
        {
            int tmp = 1;
            try
            {
                tmp += db.detalle_c.Max(d => d.id);
            }
            catch (Exception e)
            {
            }
            //a;adir detalle
            string query= "insert into detalle_c(id,produc,canti,compra,prec_unit,tot) values " +
                "( " + tmp.ToString() + ",'" + detalle_c.produc + "'," + detalle_c.canti.ToString() + "," + detalle_c.compra.ToString() + "," + detalle_c.prec_unit.ToString() + "," + (detalle_c.prec_unit * detalle_c.canti).ToString() + ");";
            db.Database.ExecuteSqlCommand(query);
            //Actualizar id = compra
            query = "update compra set tot=tot+"+ (detalle_c.prec_unit * detalle_c.canti).ToString()+"where id="+detalle_c.compra+";";
            db.Database.ExecuteSqlCommand(query);
            //actualizar id = produc
            query = "update prod set canti=canti+" + (detalle_c.canti).ToString() + "where id='" + detalle_c.produc + "';";
            db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Edit","Compras",new { id=detalle_c.compra});
            

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
            ViewBag.compra = detalle_c.compra;
            ViewBag.produc = new SelectList(db.prod, "id", "nom", detalle_c.produc);
            return View(detalle_c);
        }

        // POST: detalle_c/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,produc,canti,compra,prec_unit,tot")] detalle_c detalle_c)
        {
            detalle_c tmp1 = db.detalle_c.Find(detalle_c.id);
            string ant_prod_id = tmp1.produc;
            int prod_canti = tmp1.canti.Value;
            string query;
            //Actulizar detalle
            query = "update detalle_c set tot=" + (detalle_c.canti * detalle_c.prec_unit).ToString() + ", canti=" + (detalle_c.canti).ToString() + ", prec_unit="+ detalle_c.prec_unit.ToString() + ", produc='"+detalle_c.produc+"' where id=" + detalle_c.id.ToString() + ";";
            db.Database.ExecuteSqlCommand(query);
            ViewBag.compra = detalle_c.compra;
            //Actualizar id = compra
            query = "update compra set tot=tot-" + (detalle_c.tot).ToString()+"+"+ (detalle_c.canti*detalle_c.prec_unit).ToString()+ " where id=" + detalle_c.compra.ToString() + ";";
            db.Database.ExecuteSqlCommand(query);
            
            //actualizar id = produc
            query = "update prod set canti=canti-" + (prod_canti).ToString() + "where id='" + ant_prod_id + "';";
            db.Database.ExecuteSqlCommand(query);
            query = "update prod set canti=canti+" + (detalle_c.canti).ToString() + "where id='" + detalle_c.produc+ "';";
            db.Database.ExecuteSqlCommand(query);
            ViewBag.produc = new SelectList(db.prod, "id", "nom", detalle_c.produc);
            return RedirectToAction("Edit", "Compras", new { id = detalle_c.compra });
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
            float monto = (float)detalle_c.tot.Value;
            int compra = detalle_c.compra.Value;
            db.detalle_c.Remove(detalle_c);
            db.SaveChanges();
            //Actualizar id = compra
            string query = "update compra set tot=tot-" + (monto).ToString() + "where compra=" + compra + ";";
            db.Database.ExecuteSqlCommand(query);

            //actualizar id = produc
            query = "update prod set canti=canti-" + (detalle_c.canti).ToString() + "where id='" + detalle_c.produc+ "';";
            db.Database.ExecuteSqlCommand(query);
            ViewBag.produc = new SelectList(db.prod, "id", "nom", detalle_c.produc);


            return RedirectToAction("Edit","Compras", new { id=compra});
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
