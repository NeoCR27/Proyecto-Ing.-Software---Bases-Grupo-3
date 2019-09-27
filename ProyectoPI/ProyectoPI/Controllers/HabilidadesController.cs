using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Models;

namespace ProyectoPI.Controllers
{
    public class HabilidadesController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        // GET: Habilidades
        public ActionResult Index()
        {
            var hABILIDADES = db.HABILIDADES.Include(h => h.EMPLEADO);
            return View(hABILIDADES.ToList());
        }

        // GET: Habilidades/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HABILIDADE hABILIDADE = db.HABILIDADES.Find(id);
            if (hABILIDADE == null)
            {
                return HttpNotFound();
            }
            return View(hABILIDADE);
        }

        // GET: Habilidades/Create
        public ActionResult Create()
        {
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADOes, "cedulaPK", "tel");
            return View();
        }

        // POST: Habilidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADE hABILIDADE)
        {
            if (ModelState.IsValid)
            {
                db.HABILIDADES.Add(hABILIDADE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADOes, "cedulaPK", "tel", hABILIDADE.cedulaEmpleadoFK);
            return View(hABILIDADE);
        }

        // GET: Habilidades/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HABILIDADE hABILIDADE = db.HABILIDADES.Find(id);
            if (hABILIDADE == null)
            {
                return HttpNotFound();
            }
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADOes, "cedulaPK", "tel", hABILIDADE.cedulaEmpleadoFK);
            return View(hABILIDADE);
        }

        // POST: Habilidades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADE hABILIDADE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hABILIDADE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADOes, "cedulaPK", "tel", hABILIDADE.cedulaEmpleadoFK);
            return View(hABILIDADE);
        }

        // GET: Habilidades/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HABILIDADE hABILIDADE = db.HABILIDADES.Find(id);
            if (hABILIDADE == null)
            {
                return HttpNotFound();
            }
            return View(hABILIDADE);
        }

        // POST: Habilidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HABILIDADE hABILIDADE = db.HABILIDADES.Find(id);
            db.HABILIDADES.Remove(hABILIDADE);
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
