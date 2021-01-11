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
    public class ventasController : Controller
    {
        private MINIMARKETEntities db = new MINIMARKETEntities();

        // GET: ventas
        public ActionResult Index()
        {
            var ventas = db.venta.Include(v => v.vend);
            return View(ventas.ToList());
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
        public ActionResult Create(venta venta)
        {
            string tiemp = DateTime.Now.ToString();
            if (venta.dnivend!=null)
                db.Database.ExecuteSqlCommand("insert into venta values ("+venta.id+",0,'"+tiemp + "','" + venta.dnivend + "');");
            ViewBag.dnivend = new SelectList(db.vend, "dni", "nom", venta.dnivend);
            int id=db.Database.ExecuteSqlCommand("select id from venta where fecha like'" + tiemp +"';");
            return RedirectToAction("Edit", new { id = id, flag = true });
        }

        // GET: ventas/Edit/5
        public ActionResult Edit(int? id)
        {
            return RedirectToAction("Edit", new { id = id, flag=false });
        }
        public ActionResult Edit(int? id, bool flag)
        {
            if (flag) ViewBag.title = "Nueva Venta - Añadir detalle";
            else ViewBag.title = "Editar Venta";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            venta venta = db.venta.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.vend = db.vend.Find(venta.dnivend);
            ViewBag.dnivends = new SelectList(db.vend.ToList(), "dni", "nom", ViewBag.vend.dni);
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
        
        public ActionResult MostrarDetalle(int id)
        {
            venta venta = db.venta.Find(id);
            MINIMARKETEntities dbContext = new MINIMARKETEntities(); 

            dbContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            var prods1 =
                from p in db.prod
                join u in db.detalle_v.Where(detalle_v => detalle_v.venta == id)
                on new { detalle_v = p.id } equals new { detalle_v = u.produc } into lj
                from x in lj.DefaultIfEmpty()
                where x.produc == null
                select new
                {
                    id = p.id,
                    nom = p.nom
                };
            List<prod> prods= new List<prod>();
            foreach (var i in prods1) {
                prods.Add(db.prod.Find(i.id));
            }
            ViewBag.prods = prods.ToList();

            ViewBag.dets = venta.detalle_v.ToList();
            ViewBag.venta = venta;
            
            return PartialView("MostrarDetalle");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDet(int id, detalle_v FormData)
        {
            detalle_v tmp = db.detalle_v.Find(FormData.id);
            if (FormData.produc != null)
                tmp.produc = FormData.produc;
            if (FormData.canti != null)
                tmp.canti= FormData.canti;
            db.Entry(tmp).State = EntityState.Modified;
            db.SaveChanges();
            if (tmp.venta.HasValue)
            {
                return Redirect("/ventas/Edit/" + tmp.venta.Value.ToString());  
            }
            return Redirect("/ventas/");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoDet(detalle_v FormData,int venta)
        {
            db.Database.ExecuteSqlCommand("insert into detalle_v values ('"+FormData.produc+"',"+venta+","+FormData.canti+",0);");
            //no respeta el trigger, por eso despues de insertar debo modificarlo, porque en el update si cumple, puede ser que instead of no cumpla pero si for?
            int intIdt = db.detalle_v.Max(u => u.id);
            if (FormData!=null)
            {
                detalle_v tmp = db.detalle_v.Find(intIdt);
                if (FormData.produc != null)
                    tmp.produc = FormData.produc;
                if (FormData.canti != null)
                    tmp.canti = FormData.canti;
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("/ventas/Edit/" + venta.ToString());
            }
            return Redirect("/ventas/Edit/"+ FormData.venta.ToString());
        }
        public ActionResult DeleteDet(int id)
        {            
            detalle_v det_venta = db.detalle_v.Find(id);
            string venta = det_venta.venta.ToString();
            db.detalle_v.Remove(det_venta);
            db.SaveChanges();
            
            return Redirect("/ventas/Edit/" + venta);
            
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
