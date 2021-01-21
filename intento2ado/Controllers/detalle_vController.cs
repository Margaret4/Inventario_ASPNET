using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using intento2ado.Models;
using System.Data.SqlClient;

namespace intento2ado.Controllers
{
    public class detalle_vController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        void connectionString()
        {
            con.ConnectionString = "Data Source = SQL5101.site4now.net; Initial Catalog = DB_A6E0A3_MINIMARKET; User Id = DB_A6E0A3_MINIMARKET_admin; Password = @ABC123@";
            //&quot; data source = ; initial catalog = MINIMARKET; integrated security = True; MultipleActiveResultSets = True; App = EntityFramework & quot

        }
        private MINIMARKETEntities db = new MINIMARKETEntities();

        // GET: detalle_v
        public ActionResult Index(int? venta)
        {
            var detalle_v = db.detalle_v.Include(d => d.prod).Include(d => d.venta1).Where(d=> d.venta==venta);
            ViewBag.tot = db.venta.Find(venta).tot;
            ViewBag.venta = venta;

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
        public ActionResult Create(int? id)
        {
            ViewBag.venta = id;
            var prods1 =
            from p in db.prod
            join u in db.detalle_v.Where(detalle_v => detalle_v.venta == id)
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
               // prods.Add(db.prod.Find(i.id));
            }
            ViewBag.produc = new SelectList(db.prod.ToList(), "id", "nom");
            ViewBag.venta = id;
            return View();
        }

        // POST: detalle_v/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( detalle_v detalle_v)
        {
            
            //a;adir detalle
            prod prod_added = db.prod.Find(detalle_v.produc);
            string query = "insert into detalle_v(produc,canti,venta,monto) values " +
                "( '" + detalle_v.produc + "'," + detalle_v.canti.ToString() + "," + detalle_v.venta.ToString()+"," + (detalle_v.canti * prod_added.precio).ToString() + ");";
            db.Database.ExecuteSqlCommand(query);
            //Actualizar id = compra
            query = "update venta set tot=tot+" + (prod_added.precio * detalle_v.canti).ToString() + "where id=" + detalle_v.venta + ";";
            db.Database.ExecuteSqlCommand(query);
            //actualizar id = produc
            query = "update prod set canti=canti-" + (detalle_v.canti).ToString() + "where id='" + detalle_v.produc + "';";
            db.Database.ExecuteSqlCommand(query);

            return RedirectToAction("Edit","ventas",new { id= detalle_v.venta});
            
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
            //    prods.Add(db.prod.Find(i.id));
            }
            ViewBag.produc = new SelectList(db.prod.ToList(), "id", "nom");
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
            string query;
            prod newProd = db.prod.Find(detalle_v.produc);
            detalle_v antdet = db.detalle_v.Find(detalle_v.id);
            prod antProd = db.prod.Find(antdet.produc);

            //Actulizar detalle
            query = "update detalle_v set monto=" + (detalle_v.canti * newProd.precio).ToString() + ", canti=" + (detalle_v.canti).ToString() +  ", produc='" + detalle_v.produc + "' where id=" + detalle_v.id.ToString() + ";";
            db.Database.ExecuteSqlCommand(query);
            ViewBag.venta = detalle_v.venta;
            //Actualizar id = compra
            query = "update venta set tot=tot+"+ (detalle_v.canti * newProd.precio).ToString() + "-" + antdet.monto.ToString() + " where id=" + detalle_v.venta.ToString() + ";";
            db.Database.ExecuteSqlCommand(query);

            //actualizar id = produc
            query = "update prod set canti=canti+" + antdet.canti.ToString() + "where id='" + antProd.id + "';";
            db.Database.ExecuteSqlCommand(query);
            query = "update prod set canti=canti-" + (detalle_v.canti).ToString() + "where id='" + detalle_v.produc + "';";
            db.Database.ExecuteSqlCommand(query);

            return RedirectToAction("Edit", "ventas", new { id = detalle_v.venta });
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
            float monto = (float)detalle_v.monto.Value;
            int venta = detalle_v.venta.Value;
            db.detalle_v.Remove(detalle_v);
            db.SaveChanges();
            //Actualizar id = compra
            string query = "update venta set tot=tot-" + (monto).ToString() + " where id=" + venta.ToString() + ";";
            db.Database.ExecuteSqlCommand(query);

            //actualizar id = produc
            query = "update prod set canti=canti+" + (detalle_v.canti).ToString() + " where id='" + detalle_v.produc + "';";
            db.Database.ExecuteSqlCommand(query);
            ViewBag.produc = new SelectList(db.prod, "id", "nom", detalle_v.produc);


            return RedirectToAction("Edit", "ventas", new { id = venta });
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
