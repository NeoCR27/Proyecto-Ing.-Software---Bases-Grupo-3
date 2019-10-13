using System;
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
            if (duplicado == null)
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
                return RedirectToAction("NoDuplicados");
            }

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
        public async Task<ActionResult> Edit(EMPLEADO eMPLEADO)
        {
            if (ModelState.IsValid)
            {

                await seguridad_controller.ChangeRol(eMPLEADO.correo, eMPLEADO.rol);
                db.Entry(eMPLEADO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../EMPLEADO/Index");
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
        public ActionResult DeleteConfirmed(string id)
        {
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(id);
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
            List<EMPLEADO> empleados = (db.EMPLEADO.Where((e => e.disponibilidad == true && e.rol == "Lider"))).ToList();

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
