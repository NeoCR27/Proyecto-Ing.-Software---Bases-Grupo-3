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

    public class ConsultasController : Controller
    {

        private EMPLEADOController empleadosController = new EMPLEADOController();

        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        private SeguridadController seguridad_controller = new SeguridadController();


        // Despliega las habilidades del respectivo empleado con la cedula  igual al id
        public async Task<ActionResult> Index()
        {
            
            string correo = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(correo);
            ViewBag.miRol = rol;
            
           
            return View();
        }

        public async Task<ActionResult> CantReq()
        {

            string correo = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(correo);
            ViewBag.miRol = rol;


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
    }
}
