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
    public class REQUERIMIENTOSController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        // GET: REQUERIMIENTOS
        public ActionResult Index()
        {
            var rEQUERIMIENTOS = db.REQUERIMIENTOS.Include(r => r.EMPLEADO).Include(r => r.PROYECTO);
            return View(rEQUERIMIENTOS.ToList());
        }

        // GET: REQUERIMIENTOS/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REQUERIMIENTOS rEQUERIMIENTOS = db.REQUERIMIENTOS.Find(id);
            if (rEQUERIMIENTOS == null)
            {
                return HttpNotFound();
            }
            return View(rEQUERIMIENTOS);
        }

        // GET: REQUERIMIENTOS/Create
        public ActionResult Create()
        {
            ViewBag.cedulaFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel");
            ViewBag.idFK = new SelectList(db.PROYECTO, "idPK", "nombre");
            return View();
        }

        // POST: REQUERIMIENTOS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idFK,nombrePK,fechaInicio,fechaEntrega,horasReales,horasEstimadas,dificultad,cedulaFK")] REQUERIMIENTOS rEQUERIMIENTOS)
        {
            if (ModelState.IsValid)
            {
                db.REQUERIMIENTOS.Add(rEQUERIMIENTOS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cedulaFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", rEQUERIMIENTOS.cedulaFK);
            ViewBag.idFK = new SelectList(db.PROYECTO, "idPK", "nombre", rEQUERIMIENTOS.idFK);
            return View(rEQUERIMIENTOS);
        }

        // GET: REQUERIMIENTOS/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REQUERIMIENTOS rEQUERIMIENTOS = db.REQUERIMIENTOS.Find(id);
            if (rEQUERIMIENTOS == null)
            {
                return HttpNotFound();
            }
            ViewBag.cedulaFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", rEQUERIMIENTOS.cedulaFK);
            ViewBag.idFK = new SelectList(db.PROYECTO, "idPK", "nombre", rEQUERIMIENTOS.idFK);
            return View(rEQUERIMIENTOS);
        }

        // POST: REQUERIMIENTOS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idFK,nombrePK,fechaInicio,fechaEntrega,horasReales,horasEstimadas,dificultad,cedulaFK")] REQUERIMIENTOS rEQUERIMIENTOS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rEQUERIMIENTOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", rEQUERIMIENTOS.cedulaFK);
            ViewBag.idFK = new SelectList(db.PROYECTO, "idPK", "nombre", rEQUERIMIENTOS.idFK);
            return View(rEQUERIMIENTOS);
        }

        // GET: REQUERIMIENTOS/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REQUERIMIENTOS rEQUERIMIENTOS = db.REQUERIMIENTOS.Find(id);
            if (rEQUERIMIENTOS == null)
            {
                return HttpNotFound();
            }
            return View(rEQUERIMIENTOS);
        }

        // POST: REQUERIMIENTOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            REQUERIMIENTOS rEQUERIMIENTOS = db.REQUERIMIENTOS.Find(id);
            db.REQUERIMIENTOS.Remove(rEQUERIMIENTOS);
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
