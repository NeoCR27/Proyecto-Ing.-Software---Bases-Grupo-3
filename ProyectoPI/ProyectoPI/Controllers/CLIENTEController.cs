﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Models;

namespace ProyectoPI.Controllers
{
    public class CLIENTEController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();
        private SeguridadController seguridadController = new SeguridadController();

        // GET: CLIENTE
        public async Task<ActionResult> Index()
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.rol = rol;
            if (rol == "Cliente")
            {
                var miInfo = db.CLIENTE.Where(x => x.correo == mail);
                return View(miInfo.ToList());
            }

            return View(db.CLIENTE.ToList());
        }

        // GET: CLIENTE/Details/5
        public async Task<ActionResult> Details(string id)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.rol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // GET: CLIENTE/Create
        public async Task<ActionResult> Create()
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.rol = rol;
            return View();
        }

        // POST: CLIENTE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta")] CLIENTE cLIENTE)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.rol = rol;

            CLIENTE duplicate = db.CLIENTE.Find(cLIENTE.cedulaPK);
            if ((duplicate == null) && (!db.CLIENTE.Any(x => x.tel == cLIENTE.tel)) && (!db.CLIENTE.Any(x => x.correo == cLIENTE.correo)))
            {
                if (ModelState.IsValid)
                {
                    db.CLIENTE.Add(cLIENTE);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (duplicate != null)
                {
                    this.ModelState.AddModelError("", "YA EXISTE UN CLIENTE CON LA MISMA CÉDULA");
                }
                else if (db.CLIENTE.Any(x => x.tel == cLIENTE.tel))
                {
                    this.ModelState.AddModelError("", "YA EXISTE UN CLIENTE CON EL MISMO TELÉFONO");
                }
                else if (db.CLIENTE.Any(x => x.correo == cLIENTE.correo))
                {
                    this.ModelState.AddModelError("", "YA EXISTE UN CLIENTE CON EL MISMO CORREO");
                }

                return View(cLIENTE);
            }

            return View(cLIENTE);
        }

        // GET: CLIENTE/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.rol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // POST: CLIENTE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta")] CLIENTE cLIENTE)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.rol = rol;

            var datosOriginales = db.CLIENTE.AsNoTracking().Where(x => x.cedulaPK == cLIENTE.cedulaPK).FirstOrDefault();
            if ((datosOriginales.tel != cLIENTE.tel) && (db.CLIENTE.Any(x => x.tel == cLIENTE.tel)))
            {
                this.ModelState.AddModelError("", "YA EXISTE UN CLIENTE CON EL MISMO TELÉFONO");
                return View(cLIENTE);
            }
            if ((datosOriginales.correo != cLIENTE.correo) && (db.CLIENTE.Any(x => x.correo == cLIENTE.correo)))
            {
                this.ModelState.AddModelError("", "YA EXISTE UN CLIENTE CON EL MISMO CORREO");
                return View(cLIENTE);
            }

            if (ModelState.IsValid)
            {
                db.Entry(cLIENTE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cLIENTE);
        }

        // GET: CLIENTE/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(mail);
            ViewBag.rol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTE.Find(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // POST: CLIENTE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CLIENTE cLIENTE = db.CLIENTE.Find(id);

            var proyectos_activos = db.PROYECTO.AsNoTracking().Where(x => x.cedulaClienteFK == id && x.estado.Equals("En-proceso")).FirstOrDefault();
            if (proyectos_activos != null)
            {
                this.ModelState.AddModelError("CustomError", "ESTE CLIENTE ESTA EN UN PROYECTO ACTIVO");
                return View(cLIENTE);
            }
            db.CLIENTE.Remove(cLIENTE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult NoDuplicados()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<SelectListItem> GetClientes()
        { // Retorna los nombres y cédulas de los clientes
            List<CLIENTE> clientes = (db.CLIENTE.ToList());

            List<SelectListItem> informacion = clientes.ConvertAll(e => {
                return new SelectListItem()
                {
                    Text = e.nombre + " " + e.primerApellido,
                    Value = e.cedulaPK,
                    Selected = false
                };
            });

            foreach(CLIENTE e in clientes)
            {
                System.Diagnostics.Debug.WriteLine(e.nombre);
            }
            return informacion;
        }
    }
}
