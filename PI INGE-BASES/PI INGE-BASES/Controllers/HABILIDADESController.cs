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
    public class HABILIDADESController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        // GET: HABILIDADES
        public ActionResult Index()
        {
            var hABILIDADES = db.HABILIDADES.Include(h => h.EMPLEADO);
            return View(hABILIDADES.ToList());
        }

        // GET: HABILIDADES/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HABILIDADES hABILIDADES = db.HABILIDADES.Find(id);
            if (hABILIDADES == null)
            {
                return HttpNotFound();
            }
            return View(hABILIDADES);
        }

        // GET: HABILIDADES/Create
        public ActionResult Create()
        {
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel");
            return View();
        }

        // POST: HABILIDADES/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADES hABILIDADES)
        {
            if (ModelState.IsValid)
            {
                db.HABILIDADES.Add(hABILIDADES);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", hABILIDADES.cedulaEmpleadoFK);
            return View(hABILIDADES);
        }

        // GET: HABILIDADES/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HABILIDADES hABILIDADES = db.HABILIDADES.Find(id);
            if (hABILIDADES == null)
            {
                return HttpNotFound();
            }
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", hABILIDADES.cedulaEmpleadoFK);
            return View(hABILIDADES);
        }

        // POST: HABILIDADES/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADES hABILIDADES)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hABILIDADES).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaEmpleadoFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel", hABILIDADES.cedulaEmpleadoFK);
            return View(hABILIDADES);
        }

        // GET: HABILIDADES/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HABILIDADES hABILIDADES = db.HABILIDADES.Find(id);
            if (hABILIDADES == null)
            {
                return HttpNotFound();
            }
            return View(hABILIDADES);
        }

        // POST: HABILIDADES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HABILIDADES hABILIDADES = db.HABILIDADES.Find(id);
            db.HABILIDADES.Remove(hABILIDADES);
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
