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

        private CLIENTEController clienteController = new CLIENTEController();

        // GET: PROYECTOes
        //public async Task<ActionResult> Index()
        public async Task<ActionResult> Index()
        {
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user); // Me retorna el rol del usuario logeado
            ViewBag.rol = rol;

            if (rol.Equals("Jefe", StringComparison.InvariantCultureIgnoreCase)) // Retorno todos los pryectos
            {
                ViewBag.proyectos = true;
                return View(db.PROYECTO.Include(p => p.CLIENTE).ToList());
            }
            else // Retorno solo los proyectos en los que estoy
            {
                var cedulaUsuarioLogeado = (from proy in db.PROYECTO
                                            join participa in db.PARTICIPA on proy.idPK equals participa.idProyectoFK
                                            join empleado in db.EMPLEADO on participa.cedulaEmpleadoFK equals empleado.cedulaPK
                                            where empleado.correo == user
                                            select new
                                            {
                                                empleado.cedulaPK
                                            }).ToList();
                if (cedulaUsuarioLogeado.Count != 0) // Esta en la tabla PARTICIPA, por lo tanto ha sido parte de algun equipo
                {
                    string cedulaResultado = cedulaUsuarioLogeado.First().ToString(); // Tiene la fomra "{ cedulaPK = cedula }"
                    cedulaResultado = cedulaResultado.Replace("{", ""); // " cedulaPK = cedula }"
                    cedulaResultado = cedulaResultado.Replace("}", ""); // " cedulaPK = cedula "
                    cedulaResultado = cedulaResultado.Replace("cedulaPK", ""); // "  = cedula }"
                    cedulaResultado = cedulaResultado.Replace(" ", ""); // "=cedula}"
                    cedulaResultado = cedulaResultado.Replace("=", ""); // "cedula"

                    string queryProyecto = "Select * from PROYECTO proy join PARTICIPA participa  on proy.idPK = participa.idProyectoFK join EMPLEADO empleado on participa.cedulaEmpleadoFK = empleado.cedulaPK join CLIENTE cliente   on proy.cedulaClienteFK = cliente.cedulaPK where participa.cedulaEmpleadoFK = " + cedulaResultado;
                    var proyecto = db.Database.SqlQuery<PROYECTO>(queryProyecto).ToList();
                    string queryCliente = "Select cliente.nombre from PROYECTO proy join PARTICIPA participa  on proy.idPK = participa.idProyectoFK join EMPLEADO empleado on participa.cedulaEmpleadoFK = empleado.cedulaPK join CLIENTE cliente   on proy.cedulaClienteFK = cliente.cedulaPK where participa.cedulaEmpleadoFK = " + cedulaResultado;
                    var cliente = db.Database.SqlQuery<string>(queryCliente).ToList();
                    string nombreCliente = cliente.First().ToString();
                    string queryClienteApellido = "Select cliente.primerApellido from PROYECTO proy join PARTICIPA participa  on proy.idPK = participa.idProyectoFK join EMPLEADO empleado on participa.cedulaEmpleadoFK = empleado.cedulaPK join CLIENTE cliente   on proy.cedulaClienteFK = cliente.cedulaPK where participa.cedulaEmpleadoFK = " + cedulaResultado;
                    var clienteApellido = db.Database.SqlQuery<string>(queryCliente).ToList();
                    string apellidoCliente = clienteApellido.First().ToString();
                    string nombreCompletoCliente = nombreCliente + " " + apellidoCliente;
                    List <PROYECTO> model = new List<PROYECTO>();
                   foreach (var item in proyecto) 
                    {
                        PROYECTO proy = new PROYECTO();
                        proy.idPK = item.idPK;
                        proy.nombre = item.nombre;
                        proy.objetivo = item.objetivo;
                        proy.CLIENTE = new CLIENTE();
                        proy.CLIENTE.nombre = nombreCompletoCliente;
                        model.Add(proy);
                    }

                    ViewBag.proyectos = true;
                    return View(model);
                }
                else
                {
                    ViewBag.proyectos = false;
                    return View();
                }
            }
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
            string lideractual = nombreLiderActual.First().ToString(); // Queda con la forma "{ nombre = nombre apellido }"
            lideractual = lideractual.Substring(10); // Queda con la forma "nombre apellido }"
            lideractual = lideractual.Substring(0, lideractual.Length - 2); // Queda con la forma "nombre apellido"
            ViewBag.liderActual = lideractual;
            return View(pROYECTO);
        }

        // GET: PROYECTOes/Create
        public async Task<ActionResult> Create()
        {
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user);
            ViewBag.rol = rol;
            ViewBag.cedulaClienteFK = new SelectList(db.CLIENTE, "", "nombre");
            List<SelectListItem> lideres = this.empleadoController.GetLideresDisponibles(); // Retorna los lideres disponibles
            ViewBag.lideres = lideres;
            List<SelectListItem> clientes = this.clienteController.GetClientes(); // Retorna los clientes
            ViewBag.clientes = clientes;
            return View();
        }

        // POST: PROYECTOes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPK,nombre,objetivo,duracionReal,duracionEstimada,fechaInicio,fechaFinalizacion,estado,cedulaClienteFK")] PROYECTO pROYECTO)
        {

            if (ModelState.IsValid)
            {
                pROYECTO.idPK = this.GetIdAsignar(); // Asigna el id automaticamente al proyecto
                db.PROYECTO.Add(pROYECTO);

                db.SaveChanges();

                string cedulaLiderEscogido = Request.Form["Lideres"].ToString(); // Agarra el valor seleccionado en el dropdown de la vista con los lideres disponibles

                participaController.Agregar(pROYECTO.idPK, cedulaLiderEscogido, "Lider"); // Agrego a PARTICIPA el lider escogido

                EMPLEADO lider = db.EMPLEADO.Find(cedulaLiderEscogido);
                lider.disponibilidad = false; // Cambiar la disponibilidad del lider escogido

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(pROYECTO);
        }

        // GET: PROYECTOes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
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
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user);
            ViewBag.rol = rol;
            ViewBag.idPK = pROYECTO.idPK;
            // Sacar empleado con rol Lider de participa en el id del proyecto
            var cedulaYnombreLiderActual = (from proy in db.PROYECTO
                                     join participa in db.PARTICIPA on proy.idPK equals participa.idProyectoFK
                                     join empleado in db.EMPLEADO on participa.cedulaEmpleadoFK equals empleado.cedulaPK
                                     where participa.rol == "Lider" && proy.idPK == id
                                     select new
                                     {
                                         nombre = empleado.nombre + " " + empleado.primerApellido,
                                         cedula = empleado.cedulaPK
                                     }).ToList();

            string informacionLider = cedulaYnombreLiderActual.First().ToString();
            string[] nombreYCedula = informacionLider.Split(',');
            string nombre = nombreYCedula[0]; // "{ nombre = nombre apellido"
            nombre = nombre.Substring(10); // "nombre apellido"
            string cedula = nombreYCedula[1]; // "cedula = cedula }"
            cedula = cedula.Replace(" ", ""); // "cedula=cedula}"
            cedula = cedula.Replace("}", ""); // "cedula=cedula"
            cedula = cedula.Replace("cedula=", ""); // "cedula"

            ViewBag.liderActual = nombre;
            ViewBag.cedulaLiderActual = cedula;
            List<SelectListItem> lideres = this.empleadoController.GetLideresDisponibles(); // Retorna los lideres disponibles
            List<SelectListItem> clientes = this.clienteController.GetClientes(); // Retorna los clientes
            ViewBag.lideres = lideres;
            ViewBag.clientes = clientes;
            ViewBag.estadoActual = pROYECTO.estado;

            return View(pROYECTO);
        }

        // POST: PROYECTOes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string cedulaLiderActual, string estado, [Bind(Include = "idPK,nombre,objetivo,duracionReal,duracionEstimada,fechaInicio,fechaFinalizacion,estado,cedulaClienteFK")] PROYECTO pROYECTO)
        {
            if (ModelState.IsValid)
            {
                string cedulaLiderEscogido = Request.Form["Lideres"].ToString(); // Agarra el valor seleccionado en el dropdown de la vista con los lideres disponibles
                if (cedulaLiderEscogido.Equals("") == false) // SI se cambio al lider
                {
                    participaController.Eliminar(pROYECTO.idPK, cedulaLiderActual); // Elimina que participa en ese proyecto
                    participaController.Agregar(pROYECTO.idPK, cedulaLiderEscogido, "Lider"); // Agrega al nuevo lider a la relacion participa
                    EMPLEADO antiguoLider = db.EMPLEADO.Find(cedulaLiderActual); // Cambio de la disponibilidad del lider antiguo
                    antiguoLider.disponibilidad = true;
                    EMPLEADO nuevoLider = db.EMPLEADO.Find(cedulaLiderEscogido);
                    nuevoLider.disponibilidad = false; 
                }
                pROYECTO.estado = Request.Form["estado"].ToString();
                db.Entry(pROYECTO).State = EntityState.Modified;
                db.SaveChanges();
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

            ViewBag.idPK = pROYECTO.idPK;
            var informacionLider = (from proy in db.PROYECTO
                                    join participa in db.PARTICIPA on proy.idPK equals participa.idProyectoFK
                                    join empleado in db.EMPLEADO on participa.cedulaEmpleadoFK equals empleado.cedulaPK
                                    where participa.rol == "Lider" && proy.idPK == pROYECTO.idPK
                                    select new
                                    {
                                        cedula = empleado.cedulaPK
                                    }).ToList();

            string cedulaLiderActual = informacionLider.First().ToString();
            cedulaLiderActual = cedulaLiderActual.Substring(10); // " { cedula = cedula }"
            cedulaLiderActual = cedulaLiderActual.Replace(" ", ""); // "cedula} "
            cedulaLiderActual = cedulaLiderActual.Replace("}", ""); // "cedula";

            ViewBag.cedulaLiderActual = cedulaLiderActual;

            return View(pROYECTO);
        }

        // POST: PROYECTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string cedulaLiderActual, string id)
        {
            PROYECTO pROYECTO = db.PROYECTO.Find(id);
            db.PROYECTO.Remove(pROYECTO);
            EMPLEADO lider = db.EMPLEADO.Find(cedulaLiderActual); // Pone la disponibilidad del lider en TRUE
            lider.disponibilidad = true;
            PARTICIPA participa = db.PARTICIPA.Find(cedulaLiderActual, id); 
            db.PARTICIPA.Remove(participa); // Elimina la tupla de PARTICIPA
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

        public string GetIdAsignar() // Metodo que retorna el mayor idPK que se encuentra en la base de datos en las instancias de PROYECTOS,
                                     // esto para comenzar a asignar este id automaticamente
        {
            List<PROYECTO> ids = db.PROYECTO.Where(p => p.idPK != null).ToList();
            List<int> idsInts = new List<int>();
            if (ids.Count != 0)
            {
                foreach (PROYECTO proy in ids)
                {
                    idsInts.Add(Int32.Parse(proy.idPK));
                }
                int ultimoIdAsignado = idsInts.Max() + 1;
                return ultimoIdAsignado.ToString();
            }
            else
            {
                return "1";
            }
        }

        //Redirecciona la al controlador de habilidades
        public ActionResult get_req(string id)
        {
            return RedirectToAction("../REQUERIMIENTOS/index", new { id = id });
        }
    }
}
