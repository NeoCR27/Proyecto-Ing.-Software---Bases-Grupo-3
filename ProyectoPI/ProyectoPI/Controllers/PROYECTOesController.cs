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
    public class PROYECTOesController : Controller
    {

        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        private EMPLEADOController empleadoController = new EMPLEADOController();

        private PARTICIPAController participaController = new PARTICIPAController();

        private SeguridadController seguridadController = new SeguridadController();

        // GET: PROYECTOes
        //public async Task<ActionResult> Index()
        public async Task<ActionResult> Index()
        {
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user);
            System.Diagnostics.Debug.WriteLine(rol);
            var pROYECTO = db.PROYECTO.Include(p => p.CLIENTE);
            ViewBag.rol = rol;
            return View(pROYECTO.ToList());
        }

        // GET: PROYECTOes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user);
            ViewBag.rol = rol;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROYECTO pROYECTO = db.PROYECTO.Find(id);
            if (pROYECTO == null)
            {
                return HttpNotFound();
            }
            var nombreLiderActual = (from proy in db.PROYECTO
                                     join participa in db.PARTICIPA on proy.idPK equals participa.idProyectoFK
                                     join empleado in db.EMPLEADO on participa.cedulaEmpleadoFK equals empleado.cedulaPK
                                     where participa.rol == "Lider"
                                     select new
                                     {
                                         nombre = empleado.nombre + " " + empleado.primerApellido
                                     }).ToList();
            string lideractual = nombreLiderActual.First().ToString();
            int tamano = lideractual.Length - 2;
            lideractual = lideractual.Substring(10);
            lideractual = lideractual.Substring(0, lideractual.Length - 2);
            ViewBag.liderActual = lideractual;
            return View(pROYECTO);
        }

        // GET: PROYECTOes/Create
        public async Task<ActionResult> Create()
        {

            ViewBag.idPK = "0";
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user);
            ViewBag.rol = rol;
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "", "nombre");
            List<SelectListItem> lideres = this.empleadoController.getLideresDisponibles();
            ViewBag.lideres = lideres;
            return View();
        }

        // POST: PROYECTOes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPK,nombre,objetivo,duracionReal,duracionEstimada,fechaInicio,fechaFinalizacion,estado,cedulaClienteFK")] PROYECTO pROYECTO)
        {
            pROYECTO.idPK = this.getIdAsignar();
            if (ModelState.IsValid)
            {
                db.PROYECTO.Add(pROYECTO);
                db.SaveChanges();

                string cedulaLiderEscogido = Request.Form["Lideres"].ToString(); // Agarra el valor seleccionado en el dropdown de la vista con los lideres disponibles
                // Guardar en la base de datos que el lider escogido para ese proyecto 
                // pasar el ID del proyecto y cedula del lider, junto con el rol de "Lider"
                participaController.agregar(pROYECTO.idPK, cedulaLiderEscogido, "Lider");
                // Cambiar la disponibilidad del lider escogido
                List<EMPLEADO> empleado = (db.EMPLEADO.Where(e => e.cedulaPK == cedulaLiderEscogido)).ToList();
                empleado.First().disponibilidad = false;
                return RedirectToAction("Index");
            }
            return View(pROYECTO);
        }

        // GET: PROYECTOes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user);
            ViewBag.rol = rol;
            // Sacar empleado con rol Lider de participa en el id del proyecto
            var nombreLiderActual = (from proy in db.PROYECTO
                                      join participa in db.PARTICIPA on proy.idPK equals participa.idProyectoFK
                                      join empleado in db.EMPLEADO on participa.cedulaEmpleadoFK equals empleado.cedulaPK
                                      where participa.rol == "Lider"
                                      select new
                                      {
                                          nombre = empleado.nombre + " " + empleado.primerApellido
                                      }).ToList();
            string lideractual = nombreLiderActual.First().ToString();
            int tamano = lideractual.Length - 2;
            lideractual = lideractual.Substring(10);
            lideractual = lideractual.Substring(0, lideractual.Length - 2);
            ViewBag.liderActual = lideractual;
            //ViewBag.rol = this.rol;
            ViewBag.rol = "Jefe";
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "", "cedulaPK");
            List<SelectListItem> lideres = this.empleadoController.getLideresDisponibles();
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
            if (ModelState.IsValid)
            {
                db.Entry(pROYECTO).State = EntityState.Modified;
                db.SaveChanges();

                string cedulaLiderEscogido = Request.Form["Lideres"].ToString(); // Agarra el valor seleccionado en el dropdown de la vista con los lideres disponibles
                // Guardar en la base de datos que el lider escogido para ese proyecto 
                // pasar el ID del proyecto y cedula del lider, junto con el rol de "Lider"
                participaController.agregar(pROYECTO.idPK, cedulaLiderEscogido, "Lider");
                return RedirectToAction("Index");
            }
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "", "cedulaPK", pROYECTO.cedulaClienteFK);
            return View(pROYECTO);
        }

        // GET: PROYECTOes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user);
            ViewBag.rol = rol;
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

        public string getIdAsignar() // Metodo que retorna el mayor idPK que se encuentra en la base de datos en las instancias de PROYECTOS,
                                     // esto para comenzar a asignar este id automaticamente
        {
            List<PROYECTO> ids = db.PROYECTO.Where(p => p.idPK != null).ToList();
            List<int> idsInts = new List<int>();
            foreach (PROYECTO proy in ids)
            {
                idsInts.Add(Int32.Parse(proy.idPK));
            }
            int ultimoIdAsignado = idsInts.Max() + 1;
            return ultimoIdAsignado.ToString();
        }
    }
}
