using Manual_ASP_NET_WEB.Models;
using Manual_ASP_NET_WEB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manual_ASP_NET_WEB.Controllers
{
    public class InformacionGeneralController : Controller
    {
        // Establece conexion con el modelo para ejecutar consultas con linq
        private Manual_ASP_NET_DBEntities db = new Manual_ASP_NET_DBEntities();


        // GET: InformacionGeneral
        public ActionResult Index()
        {
            //Se obtienen los datos para mandarle a la vista
            //Se obtiene la cantidad de médicos registrados
            int cantidadMedicos = db.Medicos.Count();

            //Se obtiene la cantidad de pacientes registrados
            int cantidadPacientes = db.Pacientes.Count();

            //Se obtiene la cantidad de contultas registradas
            int cantidadConsultas = db.Consultas.Count();


            var cantidadPacientes60 = db.Pacientes.Where(paciente => paciente.Peso > 60);

            int cantidadPacientes60int = cantidadPacientes60.Count();

            // Objeto con los datos obetnidos a entregar a la vista
            var informacionObtenidaViewModel = new InformacionGeneralViewModel
            {
                CantidadMedicos = cantidadMedicos,
                CantidadPacientes = cantidadPacientes,
                CantidadConsultas = cantidadConsultas,
                CantidadPacientes60 = cantidadPacientes60int
            };            return View(informacionObtenidaViewModel);


        }
    }
}