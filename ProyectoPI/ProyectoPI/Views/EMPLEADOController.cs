using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Models;


namespace ProyectoPI.Views
{
    public class EMPLEADOController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        public String Habilidades(String id)
        {

            return "Query " + Request.QueryString[""]  ;
        }

        // GET: EMPLEADO
        public ActionResult Index()
        {
            return View(db.EMPLEADO.ToList());
        }

        // GET: EMPLEADO/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(id);
            if (eMPLEADO == null)
            {
                return HttpNotFound();
            }
            return View(eMPLEADO);
        }

        //public ActionResult Habilities(string id)
        //{

       //}

        // GET: EMPLEADO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EMPLEADO/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta,horasLaboradas,edad,disponibilidad,rol,fechaNacimiento")] EMPLEADO eMPLEADO)
        {
            if (ModelState.IsValid)
            {
                db.EMPLEADO.Add(eMPLEADO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eMPLEADO);
        }

        // GET: EMPLEADO/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(id);
            if (eMPLEADO == null)
            {
                return HttpNotFound();
            }
            return View(eMPLEADO);
        }

        // POST: EMPLEADO/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta,horasLaboradas,edad,disponibilidad,rol,fechaNacimiento")] EMPLEADO eMPLEADO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eMPLEADO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eMPLEADO);
        }

        // GET: EMPLEADO/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(id);
            if (eMPLEADO == null)
            {
                return HttpNotFound();
            }
            return View(eMPLEADO);
        }

        // POST: EMPLEADO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(id);
            db.EMPLEADO.Remove(eMPLEADO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult to_habilidades(string id)
        {
            return RedirectToAction("../HABILIDADES/detalles_empleado", new { id = id });
        }

        public SelectList get_nombres(String id)
        {
            return new SelectList(db.EMPLEADO.Where(empleado => empleado.cedulaPK == id), "", "nombre");
        }

        public SelectList get_cedulas()
        {
            return new SelectList(db.EMPLEADO, "","cedulaPK");
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
