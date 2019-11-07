﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Models;
using ProyectoPI.Controllers;
using System.Threading.Tasks;

namespace ProyectoPI.Controllers
{
    public class EMPLEADOController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();
        private SeguridadController seguridad_controller = new SeguridadController();

        // Despliega la vista de todos los empleados o su propia infomacion, depende de sus permisos
        public async Task<ActionResult> Index()
        {
            string mail = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(mail);
            ViewBag.my_rol = rol;
            if (rol == "Tester" || rol == "Lider")
            {
                var my_info = db.EMPLEADO.Where(x => x.correo == mail);

                return View(my_info.ToList());
            }
            return View(db.EMPLEADO.ToList());

        }

        // Despliega la vista de  detalles de un empleado en especifico
        public ActionResult Details(string id, string rol )
        {
            ViewBag.my_rol = rol;

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

        // Despliega la vista del crear empleado
        public ActionResult Create()
        {
            string[] values = new[] { "Lider", "Jefe", "Tester" };
            ViewBag.rol = new SelectList(values);
            string[] disponible = new[] { "Disponible", "No disponible" };

            ViewBag.disponible = new SelectList(disponible);
            return View();
        }


        // Recibe los datos de la vista del crear empleado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta,horasLaboradas,edad,disponibilidad,rol,fechaNacimiento")] EMPLEADO eMPLEADO)
        {
            EMPLEADO duplicado = db.EMPLEADO.Find(eMPLEADO.cedulaPK);
            if ((duplicado == null) && (!db.EMPLEADO.Any(x => x.tel == eMPLEADO.tel)) && (!db.EMPLEADO.Any(x => x.correo == eMPLEADO.correo)))
            {
                if (ModelState.IsValid)
                {
                    //await seguridad_controller.ChangeRol(eMPLEADO.correo, eMPLEADO.rol);
                    db.EMPLEADO.Add(eMPLEADO);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            else
            {
                if (duplicado != null)
                {
                    this.ModelState.AddModelError("", "YA EXISTE UN EMPLEADO CON LA MISMA CÉDULA");
                }
                else if (db.EMPLEADO.Any(x => x.tel == eMPLEADO.tel))
                {
                    this.ModelState.AddModelError("", "YA EXISTE UN EMPLEADO CON EL MISMO TELÉFONO");
                }
                else if (db.EMPLEADO.Any(x => x.correo == eMPLEADO.correo))
                {
                    this.ModelState.AddModelError("", "YA EXISTE UN EMPLEADO CON EL MISMO CORREO");
                }

                return View(eMPLEADO);
            }
            return View(eMPLEADO);

            return RedirectToAction("../EMPLEADO/Index");
        }

        // Despliega la vista del editar empleado
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

        // Recibe los datos de la vista del editar empleado
        [HttpPost]
        [ValidateAntiForgeryToken]
    
        public async Task<ActionResult> Edit([Bind(Include = "cedulaPK,tel,nombre,primerApellido,segundoApellido,correo,distrito,canton,provincia,direccionExacta,horasLaboradas,edad,disponibilidad,rol,fechaNacimiento")] EMPLEADO eMPLEADO)
        {
            var datosOriginales = db.EMPLEADO.AsNoTracking().Where(x => x.cedulaPK == eMPLEADO.cedulaPK).FirstOrDefault();

            int error = 0;
            if ((datosOriginales.tel != eMPLEADO.tel) && (db.EMPLEADO.Any(x => x.tel == eMPLEADO.tel)))
            {
                error = 1;
                this.ModelState.AddModelError("", "YA EXISTE UN EMPLEADO CON EL MISMO TELÉFONO");
                return View(eMPLEADO);

            }
            
            if ((datosOriginales.correo != eMPLEADO.correo) && (db.EMPLEADO.Any(x => x.correo == eMPLEADO.correo)))
            {
                error = 1;
                this.ModelState.AddModelError("", "YA EXISTE UN EMPLEADO CON EL MISMO CORREO");
                return View(eMPLEADO);
            }
            if (error == 0)
            {
                if (ModelState.IsValid)
                {

                    await seguridad_controller.ChangeRol(eMPLEADO.correo, eMPLEADO.rol);
                    db.Entry(eMPLEADO).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("../EMPLEADO/Index");
                }
            }
                
            
            return RedirectToAction("../EMPLEADO/Index");
        }
        //Despliega vista para casos de elementos duplicados
        public ActionResult NoDuplicados()
        {
            return View();
        }
        // Despliega la vista del eliminar empleado
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

        // Recibe los datos de la vista del eliminar empleado
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {

            var proyectos_involucrados = db.PARTICIPA.AsNoTracking().Where(x => x.cedulaEmpleadoFK == id).FirstOrDefault();

            var proyectos_activos = db.PROYECTO.AsNoTracking().Where(x => x.idPK == proyectos_involucrados.idProyectoFK && x.estado.Equals("En-proceso")).FirstOrDefault();
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(id);

            int error = 0;
            if (proyectos_activos != null)
            {
                error = 1;
                this.ModelState.AddModelError("CustomError", "ESTE EMPLEADO ESTA EN UN PROYECTO ACTIVO");
                return View(eMPLEADO);

            }

            db.EMPLEADO.Remove(eMPLEADO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Redirecciona la al controlador de habilidades
        public ActionResult to_habilidades(string id)
        {
            return RedirectToAction("../HABILIDADES/index", new { id = id });
        }

        //Devuelve una lista con todas los nombres de los empleados
        public SelectList get_nombres(String id)
        {
            return new SelectList(db.EMPLEADO.Where(empleado => empleado.cedulaPK == id), "", "nombre");
        }

        //Devuelve una lista con todas las posibles cedulas de los empleados
        public SelectList get_cedulas()
        {
            return new SelectList(db.EMPLEADO, "", "cedulaPK");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Devuelve una lista con todos los lideres disponibles
        public List<SelectListItem> GetLideresDisponibles()
        { // Retorna los nombres y cédulas de los empleados líderes disponibles
            List<EMPLEADO> empleados = (db.EMPLEADO.Where((e => e.disponibilidad == "Disponible" && e.rol == "Lider"))).ToList();

            List<SelectListItem> informacion = empleados.ConvertAll(e => {
                return new SelectListItem()
                {
                    Text = e.nombre + " " + e.primerApellido,
                    Value = e.cedulaPK,
                    Selected = false
                };
            });
            return informacion;
        }
        //Devuelve una lista con todo los nombres y cedulas de todos los clientes, de manera de conjunto o diccionario
        public List<SelectListItem> getEmpleados()
        { // Retorna los nombres y cédulas de los clientes
            List<EMPLEADO> empleados = db.EMPLEADO.ToList();

            List<SelectListItem> informacion = empleados.ConvertAll(e => {
                return new SelectListItem()
                {
                    Text = e.nombre + " " + e.primerApellido,
                    Value = e.cedulaPK,
                    Selected = false
                };
            });
            
            foreach (EMPLEADO e in empleados)
            {
                System.Diagnostics.Debug.WriteLine(e.nombre);
            }
            return informacion;
        }
    }
}
