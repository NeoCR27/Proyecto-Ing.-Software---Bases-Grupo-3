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
    public class ParticipaController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        // GET: Participa
        public ActionResult Index()
        {
            var pARTICIPAs = db.PARTICIPAs.Include(p => p.EMPLEADO).Include(p => p.PROYECTO);
            return View(pARTICIPAs.ToList());
        }

        // GET: Participa/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PARTICIPA pARTICIPA = db.PARTICIPAs.Find(id);
            if (pARTICIPA == null)
            {
                return HttpNotFound();
            }
            return View(pARTICIPA);
        }

        // GET: Participa/Create
        public ActionResult Create()
        {
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADOes, "cedulaPK", "tel");
            ViewBag.idProyectoFK = new SelectList(db.PROYECTOes, "idPK", "nombre");
            return View();
        }

        // POST: Participa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rol,cedulaEmpleadoFK,idProyectoFK")] PARTICIPA pARTICIPA)
        {
            if (ModelState.IsValid)
            {
                db.PARTICIPAs.Add(pARTICIPA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADOes, "cedulaPK", "tel", pARTICIPA.cedulaEmpleadoFK);
            ViewBag.idProyectoFK = new SelectList(db.PROYECTOes, "idPK", "nombre", pARTICIPA.idProyectoFK);
            return View(pARTICIPA);
        }

        // GET: Participa/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PARTICIPA pARTICIPA = db.PARTICIPAs.Find(id);
            if (pARTICIPA == null)
            {
                return HttpNotFound();
            }
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADOes, "cedulaPK", "tel", pARTICIPA.cedulaEmpleadoFK);
            ViewBag.idProyectoFK = new SelectList(db.PROYECTOes, "idPK", "nombre", pARTICIPA.idProyectoFK);
            return View(pARTICIPA);
        }

        // POST: Participa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "rol,cedulaEmpleadoFK,idProyectoFK")] PARTICIPA pARTICIPA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pARTICIPA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADOes, "cedulaPK", "tel", pARTICIPA.cedulaEmpleadoFK);
            ViewBag.idProyectoFK = new SelectList(db.PROYECTOes, "idPK", "nombre", pARTICIPA.idProyectoFK);
            return View(pARTICIPA);
        }

        // GET: Participa/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PARTICIPA pARTICIPA = db.PARTICIPAs.Find(id);
            if (pARTICIPA == null)
            {
                return HttpNotFound();
            }
            return View(pARTICIPA);
        }

        // POST: Participa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PARTICIPA pARTICIPA = db.PARTICIPAs.Find(id);
            db.PARTICIPAs.Remove(pARTICIPA);
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
