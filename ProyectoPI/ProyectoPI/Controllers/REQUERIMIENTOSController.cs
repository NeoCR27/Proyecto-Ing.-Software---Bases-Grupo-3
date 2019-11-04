using System;
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

            var listaRequerimientos = req.ToList();

            List<int> requerimientosSePuedenBorrar = new List<int>();
            foreach (REQUERIMIENTOS requerimiento in listaRequerimientos)
            {
                string queryCantidadPruebasAsociadas = "EXEC cantidad_pruebas_asociadas " + "'" + requerimiento.nombrePK + "','" + requerimiento.idFK + "';";
                var resultado = (db.Database.SqlQuery<int>(queryCantidadPruebasAsociadas)).ToList().First(); // "Count = #"
                requerimientosSePuedenBorrar.Add(resultado);
            }
            ViewBag.requerimientosSePuedenBorrar = requerimientosSePuedenBorrar;
            return View(listaRequerimientos);
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
            List<SelectListItem> testerDisp = new List<SelectListItem>();

            /*Datos para desplegar miembros de equipo disponibles*/
            string queryTesterDisp = "Exec recuperar_tester_disponible" + "'" + id + "'";
            //Se hace el query a la base de datos
            var tempTesterDisp = (db.Database.SqlQuery<testerDisp>(queryTesterDisp)).ToList();
            /*Se pasa a un Select List para hacer dropdown*/
            foreach (testerDisp item in tempTesterDisp)
            {
                testerDisp.Add(new SelectListItem { Text = item.NombreEmpleado, Value = item.cedulaPK });
            }
            ViewBag.testerDisp = testerDisp;

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
                System.Diagnostics.Debug.WriteLine(rEQUERIMIENTOS.cedulaFK);
                db.REQUERIMIENTOS.Add(rEQUERIMIENTOS);
                db.SaveChanges();
                return RedirectToAction("../REQUERIMIENTOS/Index", new { id = rEQUERIMIENTOS.idFK });
            }


            return View(rEQUERIMIENTOS);
        }

        // GET: REQUERIMIENTOS/Edit/5
        public async Task<ActionResult> Edit(string id, string idpro)
        {
            string user = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(user);
            ViewBag.rol = rol;
            List<SelectListItem> testerDisp = new List<SelectListItem>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REQUERIMIENTOS rEQUERIMIENTOS = db.REQUERIMIENTOS.Find(idpro, id);
            if (rEQUERIMIENTOS == null)
            {
                return HttpNotFound();
            }
            //Datos para desplegar miembros de equipo disponibles
            string queryTesterDisp = "Exec recuperar_tester_disponible" + "'" + idpro + "'";
            //Se hace el query a la base de datos
            var tempTesterDisp = (db.Database.SqlQuery<testerDisp>(queryTesterDisp)).ToList();
            //Se pasa a un Select List para hacer dropdown
            foreach (testerDisp item in tempTesterDisp)
            {
                testerDisp.Add(new SelectListItem { Text = item.NombreEmpleado, Value = item.cedulaPK });
            }

            ViewBag.idProy = idpro;
            ViewBag.dificultad = rEQUERIMIENTOS.dificultad;
            ViewBag.estadoActual = rEQUERIMIENTOS.estado_actual;
            ViewBag.tester = rEQUERIMIENTOS.cedulaFK;
            ViewBag.testerDisp = testerDisp;
            ViewBag.nombrePK = rEQUERIMIENTOS.nombrePK; 
            return View(rEQUERIMIENTOS);
        }

        // POST: REQUERIMIENTOS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string nombrePK, string idProy, string dificultadAnterior, string estadoAnterior, string testerAnterior, [Bind(Include = "idFK,nombrePK,Descripcion,fechaInicio,fechaEntrega,horasReales,horasEstimadas,dificultad,cedulaFK")] REQUERIMIENTOS rEQUERIMIENTOS)
        {
            if (ModelState.IsValid)
            {
                rEQUERIMIENTOS.nombrePK = nombrePK;
                rEQUERIMIENTOS.idFK = idProy;
                if(Request.Form["estado"] == null) // No se selecciono ningun estado actual
                {
                    rEQUERIMIENTOS.estado_actual = estadoAnterior;
                }
                else // Se selecciono una opcion
                {
                    rEQUERIMIENTOS.estado_actual = Request.Form["estado"].ToString();
                }

                if (Request.Form["dificultad"] == null) // No se selecciono ninguna dificultad
                {
                    rEQUERIMIENTOS.dificultad = dificultadAnterior;
                }
                else // Se selecciono una opcion
                {
                    rEQUERIMIENTOS.estado_actual = Request.Form["dificultad"].ToString();
                }

                 
                if (Request.Form["Testers"] == "") // No se selecciono ningun tester
                {
                    rEQUERIMIENTOS.cedulaFK = testerAnterior;
                }
                else // Se selecciono un nuevo tester
                {
                    rEQUERIMIENTOS.cedulaFK = Request.Form["Testers"].ToString();
                }
                
                db.Entry(rEQUERIMIENTOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../REQUERIMIENTOS/Index", new { id = rEQUERIMIENTOS.idFK });
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
            return RedirectToAction("../REQUERIMIENTOS/Index", new { id = rEQUERIMIENTOS.idFK });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Redirecciona la al controlador de pruebas
        public ActionResult to_pruebas(string id, string nombre, string nombreProyecto)
        {
            System.Diagnostics.Debug.WriteLine(id);
            System.Diagnostics.Debug.WriteLine(nombre);
            System.Diagnostics.Debug.WriteLine("hola2");
            return RedirectToAction("../PRUEBAS/index", new { id = id, nombre = nombre, nombreProyecto = nombreProyecto});
        }

    }
}
