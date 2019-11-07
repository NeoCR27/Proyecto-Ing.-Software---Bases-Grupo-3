using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Controllers;
using System.Threading.Tasks;

namespace ProyectoPI.Models
{
    public class PRUEBASController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        private SeguridadController seguridad_controller = new SeguridadController();

        // GET: PRUEBAS
        public async Task<ActionResult> Index(string id, string nombre, string nombreProyecto)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(mail);
            ViewBag.my_rol = rol;

            ViewBag.nomProy = nombreProyecto;
            ViewBag.idProy = id;
            ViewBag.nomReq = nombre;

            var pruebas = db.PRUEBAS.Where(x => x.idProyFK == id && x.nombreReqFK == nombre);

            return View(pruebas.ToList());
        }

        // GET: PRUEBAS/Details/5
        public ActionResult Details(string id, string nombreReq, string nombrePK, string nombreProyecto, string rol )
        {
            ViewBag.my_rol = rol;
            ViewBag.nomProy = nombreProyecto;
            ViewBag.idProy = id;
            ViewBag.nomReq = nombreReq;
            ViewBag.nomPK = nombrePK;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRUEBAS pRUEBAS = db.PRUEBAS.Find(id, nombreReq, nombrePK);
            if (pRUEBAS == null)
            {
                return HttpNotFound();
            }
            return View(pRUEBAS);
        }

        // GET: PRUEBAS/Create
        public async Task<ActionResult> Create(string id, string nombre, string nombreProyecto)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(mail);
            ViewBag.my_rol = rol;
            ViewBag.nomProy = nombreProyecto;
            ViewBag.idProy = id;
            ViewBag.nomReq = nombre;

            return View();
        }

        // POST: PRUEBAS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProyFK,nombreReqFK,nombrePK,EstadoFinal,resultadoDetalles")] PRUEBAS pRUEBAS)
        {

            PROYECTO proyecto = db.PROYECTO.Find(pRUEBAS.idProyFK);
            string nombre = proyecto.nombre;
            PRUEBAS duplicate = db.PRUEBAS.Find(pRUEBAS.idProyFK, pRUEBAS.nombreReqFK, pRUEBAS.nombrePK);
            if (duplicate == null)
            {
                if (ModelState.IsValid)
                {
                    db.PRUEBAS.Add(pRUEBAS);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = pRUEBAS.idProyFK, nombre = pRUEBAS.nombreReqFK, nombreProyecto = nombre });
                }
            }
            else
            {
                this.ModelState.AddModelError("", "YA EXISTE UNA PRUEBA CON EL MISMO NOMBRE EN ESTE REQUERIMIENTO");
                return View(pRUEBAS);
            }
            return RedirectToAction("Index", new { id = pRUEBAS.idProyFK, nombre = pRUEBAS.nombreReqFK, nombreProyecto = nombre });
        }

        // GET: PRUEBAS/Edit/5
        public async Task<ActionResult> Edit(string id, string nombreReq, string nombrePK, string nombreProyecto)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(mail);
            ViewBag.my_rol = rol;

            ViewBag.nomProy = nombreProyecto;
            ViewBag.idProy = id;
            ViewBag.nomReq = nombreReq;
            ViewBag.nomPK= nombrePK;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var pruebas = db.PRUEBAS.Where(x => x.idProyFK == id && x.nombreReqFK == nombreReq && x.nombrePK == nombrePK);

            PRUEBAS pRUEBAS = db.PRUEBAS.Find(id, nombreReq, nombrePK);
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
            PROYECTO proyecto = db.PROYECTO.Find(pRUEBAS.idProyFK);
            string nombre = proyecto.nombre;
            if (ModelState.IsValid)
            {
                db.Entry(pRUEBAS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = pRUEBAS.idProyFK, nombre = pRUEBAS.nombreReqFK, nombreProyecto = nombre });
            }
            ViewBag.idProyFK = new SelectList(db.REQUERIMIENTOS, "idFK", "dificultad", pRUEBAS.idProyFK);
            return View(pRUEBAS);
        }

        // GET: PRUEBAS/Delete/5
        public async Task<ActionResult> Delete(string id, string nombreReq, string nombrePK, string nombreProyecto)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(mail);
            ViewBag.my_rol = rol;

            ViewBag.nomProy = nombreProyecto;
            ViewBag.idProy = id;
            ViewBag.nomReq = nombreReq;
            ViewBag.nomPK = nombrePK;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRUEBAS pRUEBAS = db.PRUEBAS.Find(id, nombreReq, nombrePK);
            if (pRUEBAS == null)
            {
                return HttpNotFound();
            }
            return View(pRUEBAS);
        }

        // POST: PRUEBAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id,string nombreReq, string nombrePK)
        {
            PROYECTO proyecto = db.PROYECTO.Find(id);
            string nombre = proyecto.nombre;
            PRUEBAS pRUEBAS = db.PRUEBAS.Find(id, nombreReq, nombrePK);
            db.PRUEBAS.Remove(pRUEBAS);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = pRUEBAS.idProyFK, nombre = pRUEBAS.nombreReqFK, nombreProyecto = nombre });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //Redirecciona al controlador de requerimientos
        public ActionResult RetornarRequerimientos(string id)
        {
            return RedirectToAction("../REQUERIMIENTOS/index", new { id = id});
        }

    }
}
