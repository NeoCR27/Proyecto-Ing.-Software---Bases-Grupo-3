using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProyectoPI.Models
{
    public class PRUEBASController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        // GET: PRUEBAS
        public ActionResult Index(string id, string nombre)
        {
            System.Diagnostics.Debug.WriteLine(id);
            System.Diagnostics.Debug.WriteLine("hola");
            ViewBag.idProy = id;
            ViewBag.nomReq = nombre;

            var pruebas = db.PRUEBAS.Where(x => x.idProyFK == id && x.nombrePK == nombre);

            return View(pruebas.ToList());
        }

        // GET: PRUEBAS/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRUEBAS pRUEBAS = db.PRUEBAS.Find(id);
            if (pRUEBAS == null)
            {
                return HttpNotFound();
            }
            return View(pRUEBAS);
        }

        // GET: PRUEBAS/Create
        public ActionResult Create(string id, string nombre)
        {
            ViewBag.idProy = id;
            ViewBag.nomReq = nombre;
            ViewBag.idProyFK = new SelectList(db.REQUERIMIENTOS, "idFK", "dificultad");
            return View();
        }

        // POST: PRUEBAS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProyFK,nombreReqFK,nombrePK,EstadoFinal,resultadoDetalles")] PRUEBAS pRUEBAS)
        {
            if (ModelState.IsValid)
            {
                db.PRUEBAS.Add(pRUEBAS);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = pRUEBAS.idProyFK, nombre = pRUEBAS.nombreReqFK });
            }

            ViewBag.idProyFK = new SelectList(db.REQUERIMIENTOS, "idFK", "dificultad", pRUEBAS.idProyFK);
            return View(pRUEBAS);
        }

        // GET: PRUEBAS/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRUEBAS pRUEBAS = db.PRUEBAS.Find(id);
            if (pRUEBAS == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProyFK = new SelectList(db.REQUERIMIENTOS, "idFK", "dificultad", pRUEBAS.idProyFK);
            return View(pRUEBAS);
        }

        // POST: PRUEBAS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProyFK,nombreReqFK,nombrePK,EstadoFinal,resultadoDetalles")] PRUEBAS pRUEBAS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pRUEBAS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = pRUEBAS.idProyFK, nombre = pRUEBAS.nombreReqFK });
            }
            ViewBag.idProyFK = new SelectList(db.REQUERIMIENTOS, "idFK", "dificultad", pRUEBAS.idProyFK);
            return View(pRUEBAS);
        }

        // GET: PRUEBAS/Delete/5
        public ActionResult Delete(string id, string nombre, string nombrePrueba)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRUEBAS pRUEBAS = db.PRUEBAS.Find(id,nombre,nombrePrueba);
            if (pRUEBAS == null)
            {
                return HttpNotFound();
            }
            return View(pRUEBAS);
        }

        // POST: PRUEBAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id,string nombre,string nombrePrueba)
        {
            PRUEBAS pRUEBAS = db.PRUEBAS.Find(id, nombre, nombrePrueba);
            db.PRUEBAS.Remove(pRUEBAS);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = id, nombre = nombre });
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
