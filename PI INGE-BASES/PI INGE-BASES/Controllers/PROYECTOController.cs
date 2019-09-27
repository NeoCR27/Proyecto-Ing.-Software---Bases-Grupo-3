using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PI_INGE_BASES.Models;

namespace PI_INGE_BASES.Controllers
{
    public class PROYECTOController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        // GET: PROYECTO
        public ActionResult Index()
        {
            var pROYECTO = db.PROYECTO.Include(p => p.CLIENTE);
            return View(pROYECTO.ToList());
        }

        // GET: PROYECTO/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROYECTO pROYECTO = db.PROYECTO.Find(id);
            if (pROYECTO == null)
            {
                return HttpNotFound();
            }
            return View(pROYECTO);
        }

        // GET: PROYECTO/Create
        public ActionResult Create()
        {
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "cedulaPK", "tel");
            return View();
        }

        // POST: PROYECTO/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPK,nombre,objetivo,duracionReal,duracionEstimada,fechaInicio,fechaFinalizacion,estado,cedulaClienteFK")] PROYECTO pROYECTO)
        {
            if (ModelState.IsValid)
            {
                db.PROYECTO.Add(pROYECTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "cedulaPK", "tel", pROYECTO.cedulaClienteFK);
            return View(pROYECTO);
        }

        // GET: PROYECTO/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROYECTO pROYECTO = db.PROYECTO.Find(id);
            if (pROYECTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "cedulaPK", "tel", pROYECTO.cedulaClienteFK);
            return View(pROYECTO);
        }

        // POST: PROYECTO/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPK,nombre,objetivo,duracionReal,duracionEstimada,fechaInicio,fechaFinalizacion,estado,cedulaClienteFK")] PROYECTO pROYECTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pROYECTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "cedulaPK", "tel", pROYECTO.cedulaClienteFK);
            return View(pROYECTO);
        }

        // GET: PROYECTO/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROYECTO pROYECTO = db.PROYECTO.Find(id);
            if (pROYECTO == null)
            {
                return HttpNotFound();
            }
            return View(pROYECTO);
        }

        // POST: PROYECTO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PROYECTO pROYECTO = db.PROYECTO.Find(id);
            db.PROYECTO.Remove(pROYECTO);
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
