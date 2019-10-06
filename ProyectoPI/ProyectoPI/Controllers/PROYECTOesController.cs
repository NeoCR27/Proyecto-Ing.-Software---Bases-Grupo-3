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
    public class PROYECTOesController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        private EMPLEADOController empleadoController = new EMPLEADOController();

        //private SeguridadController seguridadController = new SeguridadController();

        //private string user = User.identity.name();

        //private string rol = seguridadController.getRol(user);
        private string rol = "jefe";

        // GET: PROYECTOes
        public ActionResult Index()
        {
            var pROYECTO = db.PROYECTO.Include(p => p.CLIENTE);
            ViewBag.rol = this.rol;
            return View(pROYECTO.ToList());
        }

        // GET: PROYECTOes/Details/5
        public ActionResult Details(string id)
        {
            ViewBag.rol = this.rol;
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

        // GET: PROYECTOes/Create
        public ActionResult Create()
        {
            ViewBag.rol = this.rol;
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "", "cedulaPK");
            SelectList lideres = this.empleadoController.getLideres();
            ViewBag.lideres = lideres;
            return View();
        }

        // POST: PROYECTOes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string liderEscogido, [Bind(Include = "idPK,nombre,objetivo,duracionReal,duracionEstimada,fechaInicio,fechaFinalizacion,estado,cedulaClienteFK")] PROYECTO pROYECTO)
        {
            //string liderEscogido = ViewBag.lideres.SelectValue; // Sacar valor del lider escogido en el view
            System.Diagnostics.Debug.WriteLine(liderEscogido + " fue el lider escogido");
            if (ModelState.IsValid)
            {
                
                db.PROYECTO.Add(pROYECTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pROYECTO);
        }

        // GET: PROYECTOes/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.rol = this.rol;
            SelectList lideres = this.empleadoController.getLideres();
            ViewBag.lideres = lideres;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROYECTO pROYECTO = db.PROYECTO.Find(id);
            if (pROYECTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "", "cedulaPK", pROYECTO.cedulaClienteFK);
            return View(pROYECTO);
        }

        // POST: PROYECTOes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPK,nombre,objetivo,duracionReal,duracionEstimada,fechaInicio,fechaFinalizacion,estado,cedulaClienteFK")] PROYECTO pROYECTO)
        {
            //string liderEscogido = ViewBag.lideres.SelectValue; // Sacar valor del lider escogido en el view
            //System.Diagnostics.Debug.WriteLine(liderEscogido + " fue el lider escogido");
            if (ModelState.IsValid)
            {
                db.Entry(pROYECTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "cedulaPK", "tel", pROYECTO.cedulaClienteFK);
            return View(pROYECTO);
        }

        // GET: PROYECTOes/Delete/5
        public ActionResult Delete(string id)
        {
            ViewBag.rol = this.rol;
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

        // POST: PROYECTOes/Delete/5
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
