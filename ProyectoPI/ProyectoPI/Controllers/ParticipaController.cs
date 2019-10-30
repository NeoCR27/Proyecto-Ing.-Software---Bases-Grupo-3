using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoPI.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace ProyectoPI.Controllers
{
    public class PARTICIPAController : Controller
    {
        private Gr03Proy4Entities db = new Gr03Proy4Entities();
       private SeguridadController seguridadController = new SeguridadController();

        // GET: PARTICIPA
        public async Task<ActionResult> Index()
        {
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user); // Me retorna el rol del usuario logeado
            ViewBag.rol = rol;
            if (rol.Equals("Jefe", StringComparison.InvariantCultureIgnoreCase)) // Retorno todos los pryectos
            {
                ViewBag.equipos = true;
                string query = "SELECT Part.idProyectoFK AS 'IDProyecto', Emp.nombre + ' ' + Emp.primerApellido + ' ' + Emp.segundoApellido AS 'empleado', Emp.correo AS 'email', Part.rol AS 'rol', Proy.nombre AS 'proyNom' FROM PARTICIPA Part Join EMPLEADO Emp ON Emp.cedulaPK = Part.cedulaEmpleadoFK Join PROYECTO proy ON proy.idPK = Part.idProyectoFK WHERE Part.idProyectoFK = proy.idPK ORDER BY part.idProyectoFK";
                return View((db.Database.SqlQuery<EquipoModel>(query)).ToList());
            }
            else
            {
                //Obtiene cedula de usuario actualmente navegando
                var cedulaEmpleadoLogeado = (from proy in db.PROYECTO
                                            join participa in db.PARTICIPA on proy.idPK equals participa.idProyectoFK
                                            join empleado in db.EMPLEADO on participa.cedulaEmpleadoFK equals empleado.cedulaPK
                                            where empleado.correo == user
                                            select new
                                            {
                                                empleado.cedulaPK
                                            }).ToList();
                var cedulaClienteLogeado = (from proy in db.PROYECTO
                                            join cliente in db.CLIENTE on proy.cedulaClienteFK equals cliente.cedulaPK
                                            where cliente.correo == user
                                            select new
                                            {
                                                cliente.cedulaPK
                                            }).ToList();
                if (cedulaEmpleadoLogeado.Count != 0) // Esta en la tabla PARTICIPA, por lo tanto ha sido parte de algun equipo
                {
                    ViewBag.equipos = true;
                    string cedulaResultado = cedulaEmpleadoLogeado.First().ToString();
                    cedulaResultado = cedulaResultado.Replace("{", "");
                    cedulaResultado = cedulaResultado.Replace("}", "");
                    cedulaResultado = cedulaResultado.Replace("cedulaPK", "");
                    cedulaResultado = cedulaResultado.Replace(" ", "");
                    cedulaResultado = cedulaResultado.Replace("=", "");
                    ViewBag.miID = cedulaResultado;
                    string query = "SELECT Part.idProyectoFK AS 'IDProyecto', Emp.nombre + ' ' + Emp.primerApellido + ' ' + Emp.segundoApellido AS 'empleado', Emp.correo AS 'email', Part.rol AS 'rol', Proy.nombre AS 'proyNom', Emp.CedulaPK AS 'idEmp', Cli.CedulaPK as 'idCli' FROM PARTICIPA Part Join EMPLEADO Emp ON Emp.cedulaPK = Part.cedulaEmpleadoFK Join PROYECTO proy ON proy.idPK = Part.idProyectoFK Join CLIENTE cli ON cli.cedulaPK = proy.cedulaClienteFK WHERE Part.idProyectoFK = proy.idPK  ORDER BY part.idProyectoFK";
                    return View((db.Database.SqlQuery<EquipoModel>(query)).ToList());
                }
                else
                {
                    if (cedulaClienteLogeado.Count != 0) // Esta en la tabla De empleados, por lo tanto ha sido parte de algun equipo
                    {
                        ViewBag.equipos = true;
                        string cedulaResultado = cedulaClienteLogeado.First().ToString();
                        cedulaResultado = cedulaResultado.Replace("{", "");
                        cedulaResultado = cedulaResultado.Replace("}", "");
                        cedulaResultado = cedulaResultado.Replace("cedulaPK", "");
                        cedulaResultado = cedulaResultado.Replace(" ", "");
                        cedulaResultado = cedulaResultado.Replace("=", "");
                        ViewBag.miID = cedulaResultado;
                        string query = "SELECT Part.idProyectoFK AS 'IDProyecto', Emp.nombre + ' ' + Emp.primerApellido + ' ' + Emp.segundoApellido AS 'empleado', Emp.correo AS 'email', Part.rol AS 'rol', Proy.nombre AS 'proyNom', Emp.CedulaPK AS 'idEmp', Cli.CedulaPK as 'idCli' FROM PARTICIPA Part Join EMPLEADO Emp ON Emp.cedulaPK = Part.cedulaEmpleadoFK Join PROYECTO proy ON proy.idPK = Part.idProyectoFK Join CLIENTE cli ON cli.cedulaPK = proy.cedulaClienteFK WHERE Part.idProyectoFK = proy.idPK  ORDER BY part.idProyectoFK";
                        return View((db.Database.SqlQuery<EquipoModel>(query)).ToList());
                    }
                    ViewBag.equipos = false;
                    return View();
                }
             
            }
        }
        // GET: PARTICIPA/Edit/5
        public async Task<ActionResult> Edit(string id, string buscarPor = "", string filtroBusqueda = "")
        {
            string user = User.Identity.Name;
            string rol = await this.seguridadController.GetRol(user); // Me retorna el rol del usuario logeado
            ViewBag.rol = rol;
            //Se Arma los  string de query
            ViewBag.id = id;
            string queryEquipo = "SELECT Part.idProyectoFK AS 'IDProyecto', Emp.nombre + ' ' + Emp.primerApellido AS 'empleado', Emp.correo AS 'email', Part.rol AS 'rol', Emp.disponibilidad AS 'dispon', Emp.cedulaPK AS 'idEmp', Proy.nombre AS 'proyNom' FROM PARTICIPA Part Join EMPLEADO Emp ON Emp.cedulaPK = Part.cedulaEmpleadoFK JOIN Proyecto Proy ON Proy.idPK = Part.idProyectoFK WHERE Part.idProyectoFK = " + id;
            string queryEmpleado = ("Exec recuperarHabilidad" + "'"+ buscarPor + "','" + filtroBusqueda + "'");
            //Se hace el query a la base de datos
            IList<EquipoModel> resultadoQueryEquipo = (db.Database.SqlQuery<EquipoModel>(queryEquipo)).ToList();
            IList<HabilidadEmpleadoModel> resultadoQueryEmpleado = (db.Database.SqlQuery<HabilidadEmpleadoModel>(queryEmpleado)).ToList();
            ViewBag.nomProyecto = resultadoQueryEquipo.First().proyNom;
            //Se pasa a viewData para llamar desde la vista
            ViewData["equipo"] = resultadoQueryEquipo;
            ViewData["empleados"] = resultadoQueryEmpleado;           
            return View();
        }

        //Agrega un tester recibiendo los datos para crear la relación con el proyecto
        [HttpPost]
        public ActionResult AgregarTester(string idProyecto, string idEmpleado)
        {
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(idEmpleado);
            eMPLEADO.disponibilidad = "No Disponible";
            db.Entry(eMPLEADO).State = EntityState.Modified;
            db.SaveChanges();
            Agregar(idProyecto, idEmpleado, "Tester");
            return RedirectToAction("Edit", new { id = idProyecto });
        }
        //Elimina un tester de la relación con el proyecto.
        [HttpPost]
        public ActionResult RemoverTester(string idProyecto, string idEmpleado)
        {
            EMPLEADO eMPLEADO = db.EMPLEADO.Find(idEmpleado);
            eMPLEADO.disponibilidad = "Disponible";
            db.Entry(eMPLEADO).State = EntityState.Modified;
            db.SaveChanges();
            Eliminar(idProyecto, idEmpleado);
            return RedirectToAction("Edit", new { id = idProyecto });
        }

        //Agrega un empleado a un proyecto con un rol específico haciendo el query en la DB
        public void Agregar(string idProyectoFK, string cedulaEmpleadoFK, string rol)
        {
            PARTICIPA participa = db.PARTICIPA.Create();
            participa.cedulaEmpleadoFK = cedulaEmpleadoFK;
            participa.idProyectoFK = idProyectoFK;
            participa.rol = rol;
            db.PARTICIPA.Add(participa);
            db.SaveChanges();
        }

        // Elimina un empleado de un proyecto
        public void Eliminar(string idProyectoFK, string cedulaEmpleadoFK)
        {
            PARTICIPA pARTICIPA = db.PARTICIPA.Find(cedulaEmpleadoFK, idProyectoFK);
            db.PARTICIPA.Remove(pARTICIPA);
            db.SaveChanges();
        }
    }
}
