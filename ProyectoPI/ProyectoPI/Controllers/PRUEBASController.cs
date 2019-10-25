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
    public class PRUEBASController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        // GET: PRUEBAS
        public ActionResult Index()
        {
            var pRUEBAS = db.PRUEBAS.Include(p => p.REQUERIMIENTOS);
            return View(pRUEBAS.ToList());
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
        public ActionResult Create()
        {
            ViewBag.idProyFK = new SelectList(db.REQUERIMIENTOS, "idFK", "dificultad");
            return View();
        }

        // POST: PRUEBAS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProyFK,nombreReqFK,nombrePK,resultFinal,estado,proposito,entradaDatos,flujo,resultEsperado,prioridad,descripcionErr,imagenErr,estadoErr")] PRUEBAS pRUEBAS)
        {
            if (ModelState.IsValid)
            {
                db.PRUEBAS.Add(pRUEBAS);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include = "idProyFK,nombreReqFK,nombrePK,resultFinal,estado,proposito,entradaDatos,flujo,resultEsperado,prioridad,descripcionErr,imagenErr,estadoErr")] PRUEBAS pRUEBAS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pRUEBAS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProyFK = new SelectList(db.REQUERIMIENTOS, "idFK", "dificultad", pRUEBAS.idProyFK);
            return View(pRUEBAS);
        }

        // GET: PRUEBAS/Delete/5
        public ActionResult Delete(string id)
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

        // POST: PRUEBAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PRUEBAS pRUEBAS = db.PRUEBAS.Find(id);
            db.PRUEBAS.Remove(pRUEBAS);
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
