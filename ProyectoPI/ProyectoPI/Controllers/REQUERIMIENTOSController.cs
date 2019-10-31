﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Models;
using System.Threading.Tasks;
namespace ProyectoPI.Controllers
{
    public class REQUERIMIENTOSController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();
        private SeguridadController seguridad_controller = new SeguridadController();
        private string idProyecto;
        // GET: REQUERIMIENTOS
        public async Task<ActionResult> Index(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string correo = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(correo);
            ViewBag.myRol = rol;
            ViewBag.idProy = id;
            PROYECTO proy = db.PROYECTO.Find(id);
            string nombre = proy.nombre;
            ViewBag.nombre = nombre;

            var req = db.REQUERIMIENTOS.Where(x => x.idFK == id);
            return View(req.ToList());
        }

        // GET: REQUERIMIENTOS/Details/5
        public async Task<ActionResult> Details(string id, string idpro)
        {
            string user = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(user);
            ViewBag.rol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REQUERIMIENTOS rEQUERIMIENTOS = db.REQUERIMIENTOS.Find(idpro, id);
            if (rEQUERIMIENTOS == null)
            {
                return HttpNotFound();
            }
            return View(rEQUERIMIENTOS);
        }

        // GET: REQUERIMIENTOS/Create
        public ActionResult Create(string id)

        {
            ViewBag.cedulaFK = new SelectList(db.EMPLEADO, "cedulaPK", "tel");
            ViewBag.idProy = id;
            idProyecto = id;

            return View();
        }



        // POST: REQUERIMIENTOS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idFK,nombrePK,fechaInicio,fechaEntrega,horasReales,dificultad,cedulaFK,horasEstimadas,Descripcion,estado_actual,estado_final,descripcion_resultado")] REQUERIMIENTOS rEQUERIMIENTOS)
        {
            if (ModelState.IsValid)

            {
                System.Diagnostics.Debug.WriteLine(rEQUERIMIENTOS.idFK);
                db.REQUERIMIENTOS.Add(rEQUERIMIENTOS);
                db.SaveChanges();
                return RedirectToAction("../REQUERIMIENTOS/index", new { id = rEQUERIMIENTOS.idFK });
            }


            return View(rEQUERIMIENTOS);
        }

        // GET: REQUERIMIENTOS/Edit/5
        public async Task<ActionResult> Edit(string id, string idpro)
        {
            string user = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(user);
            ViewBag.rol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REQUERIMIENTOS rEQUERIMIENTOS = db.REQUERIMIENTOS.Find(idpro, id);
            if (rEQUERIMIENTOS == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProy = idpro;
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
                return RedirectToAction("../REQUERIMIENTOS/index", new { id = rEQUERIMIENTOS.idFK });
            }

            return View(rEQUERIMIENTOS);
        }

        // GET: REQUERIMIENTOS/Delete/5
        public async Task<ActionResult> Delete(string id, string idpro)
        {
            string user = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(user);
            ViewBag.rol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REQUERIMIENTOS rEQUERIMIENTOS = db.REQUERIMIENTOS.Find(idpro, id);
            if (rEQUERIMIENTOS == null)
            {
                return HttpNotFound();
            }
            return View(rEQUERIMIENTOS);
        }

        // POST: REQUERIMIENTOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string idpro)
        {
            REQUERIMIENTOS rEQUERIMIENTOS = db.REQUERIMIENTOS.Find(idpro, id);
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
