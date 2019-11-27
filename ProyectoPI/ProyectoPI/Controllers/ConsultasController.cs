﻿using System;
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
        
        public ActionResult CantReq()
        {
            ViewBag.proy = new SelectList(db.PROYECTO,"idPK","nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CantReq(string proyecto)
        {
            return RedirectToAction("MostrarTotalReq", new { proy = Request.Form["proy"].ToString() });
            


        }

        public ActionResult MostrarTotalReq(string proy)
        {
            ViewBag.idproy = proy;
            
            string queryCantReq = "Exec Consulta_Cantidad_Req_Tester" + "'" + proy + "'";
            //Se hace el query a la base de datos
            var tempCantReq = (db.Database.SqlQuery<CantReq>(queryCantReq)).ToList();
      
            string queryGetReq = "Exec Get_Req" + "'" + proy + "'";
            //Se hace el query a la base de datos
            var tempGetReq = (db.Database.SqlQuery<GetReq>(queryGetReq)).ToList();
            ViewBag.Req = tempGetReq;


            return View(tempCantReq);
        }

        public ActionResult GraficoTotalReq(string proy)
        {
            ViewBag.idproy = proy;
            System.Diagnostics.Debug.WriteLine("entro");
            string queryCantReq = "Exec Consulta_Cantidad_Req_Tester" + "'" + proy + "'";
            //Se hace el query a la base de datos
            var tempCantReq = (db.Database.SqlQuery<CantReq>(queryCantReq)).ToList();
            string[] nombres = tempCantReq.Select(l => l.nombre.ToString()).ToArray();
            string[] cantidad = tempCantReq.Select(l => l.Cantidad.ToString()).ToArray();

            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
    .       AddSeries(name:"Testers",
                    xValue: nombres,
                    yValues: cantidad)
            .AddLegend()
            .AddTitle("Cantidad de Requerimientos por Tester")
            .SetYAxis("Cantidad de Requerimientos")
            .SetXAxis("Nombre")
            .GetBytes("png");
            return File(chart, "image/bytes");
        }


        public ActionResult EstadoTesterReq()
        {
            ViewBag.proy = new SelectList(db.PROYECTO, "idPK", "nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstadoTesterReq(string proyecto)
        {
            return RedirectToAction("MostrarEstadoTesterReq", new { proy = Request.Form["proy"].ToString() });



        }

        public ActionResult MostrarEstadoTesterReq(string proy)
        {

            string queryCantReq = "Exec Consulta_Estado_Tester_Req" + "'" + proy + "'";
            //Se hace el query a la base de datos
            var tempCantReq = (db.Database.SqlQuery<EstadoAsigReq>(queryCantReq)).ToList();


            return View(tempCantReq);
        }


        public ActionResult EstadoReq()
        {
            ViewBag.proy = new SelectList(db.PROYECTO, "idPK", "nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstadoReq(string proyecto)
        {

            return RedirectToAction("MostrarEstadoReq", new { proy = Request.Form["proy"].ToString() });



        }

        public ActionResult MostrarEstadoReq(string proy)
        {
            ViewBag.idproy = proy;
            string queryEstadoReq = "Exec Consulta_Cant_Req_Estado" + "'" + proy + "'";
            //Se hace el query a la base de datos
            var tempEstadoReq = (db.Database.SqlQuery<EstadoReq>(queryEstadoReq)).ToList();
            ViewBag.final = 0;
            ViewBag.proceso = 0;
            foreach (var item in tempEstadoReq)
            {
                if (item.estado_actual=="Finalizado")
                {
                    ViewBag.final = item.Cantidad;

                }else
                {
                    ViewBag.proceso = item.Cantidad;

                }
            }

            string queryGetReq = "Exec Get_Req" + "'" + proy + "'";
            //Se hace el query a la base de datos
            var tempGetReq = (db.Database.SqlQuery<GetReq>(queryGetReq)).ToList();
            ViewBag.Req = tempGetReq;

            return View();
        }

        public ActionResult GraficoEstadoReq(string proy)
        {
            ViewBag.idproy = proy;
            System.Diagnostics.Debug.WriteLine("entro");
            string queryEstadoReq = "Exec Consulta_Cant_Req_Estado" + "'" + proy + "'";
            //Se hace el query a la base de datos
            var tempEstadoReq = (db.Database.SqlQuery<EstadoReq>(queryEstadoReq)).ToList();
            string[] estados = tempEstadoReq.Select(l => l.estado_actual.ToString()).ToArray();
            string[] cantidad = tempEstadoReq.Select(l => l.Cantidad.ToString()).ToArray();

            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
            .AddSeries(name: "Requerimientos",
                    xValue: estados,
                    yValues: cantidad)
            .AddLegend()
            .AddTitle("Estado de los Requerimientos")
            .SetYAxis("Cantidad de Requerimientos")
            .SetXAxis("Estado Actual")
            .GetBytes("png");
            return File(chart, "image/bytes");
        }


     /*Consultas Julian*/
        public ActionResult HistorialReq()
        {
            List<SelectListItem> empleados = empleadosController.getEmpleados();
            ViewBag.emp = empleados;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HistorialReq(string idEmp)
        {
            return RedirectToAction("MostrarHistorialReq", new { idEmp = Request.Form["emp"].ToString() });
        }

        public ActionResult MostrarHistorialReq(string idEmp)
        {
            ViewBag.idEmp = idEmp;
            return View();
        }

        public ActionResult MostrarHabilidadesEmpleados()
        {
            return View();
        }


        public ActionResult HabilidadesEquipo()
        {
            ViewBag.proy = new SelectList(db.PROYECTO, "idPK", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HabilidadesEquipo(string proy)
        {
            return RedirectToAction("MostrarHabilidadesEquipo", new { proy = Request.Form["proy"].ToString() });
        }

        public ActionResult MostrarHabilidadesEquipo(string proy)
        {
            ViewBag.idProy = proy;
            return View();
        }

        public ActionResult GraficoHistorialReq(string idEmp)
        {
            ViewBag.idEmp = idEmp;

            string consulHistorial = "Exec Consultar_Historial_Req_Tester '" + idEmp + "'"; //'24'

            var tempEstadoReq = (db.Database.SqlQuery<HistorialReq>(consulHistorial)).ToList();
            string[] estados = tempEstadoReq.Select(l => l.Estado.ToString()).ToArray();
            string[] totales = tempEstadoReq.Select(l => l.Total.ToString()).ToArray();

            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
            .AddSeries(name: "Requerimientos",
                    xValue: estados,
                    yValues: totales)
            .AddLegend()
            .AddTitle("Historial de los Requerimientos")
            .SetYAxis("Cantidad de Requerimientos")
            .SetXAxis("Estado Final")
            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult GraficoHabEmp(string tipoHab)
        {
            string consulHab = "Consultar_Num_Habilidades_Empleados '" + tipoHab + "'";

            var tempEstadoReq = (db.Database.SqlQuery<NumHab>(consulHab)).ToList();
            string[] habilidades = tempEstadoReq.Select(l => l.Habilidad.ToString()).ToArray();
            int[] totales = tempEstadoReq.Select(l => l.Total).ToArray();
            int totalObtenido = 0;
            foreach(int i in totales)
            {
                totalObtenido += i; 
            }
            for(int i=0; i<habilidades.Length;i++)
            {
               habilidades[i]=habilidades[i]+" ("+ ((double)totales[i]/(double)totalObtenido*100.00).ToString("#.##") +"%)";
            }
            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
            .AddSeries(name: "HabilidaddesEmp " + tipoHab, chartType: "Pie",
                    xValue: habilidades,
                    yValues: totales)
            .AddLegend()
            .AddTitle("Habilidades de los Empleados")
            .SetYAxis("Cantidad de Habilidades")
            .GetBytes("png");
      
            return File(chart, "image/bytes");
        }
        public ActionResult GraficoHabEquipo(string tipoHab, string idProy)
        {
            string consulHab = "Consultar_Num_Habilidades_Equipo'" + tipoHab + "' ,'"+idProy+"'";
            var tempEstadoReq = (db.Database.SqlQuery<NumHab>(consulHab)).ToList();
            string[] habilidades = tempEstadoReq.Select(l => l.Habilidad.ToString()).ToArray();
            int[] totales = tempEstadoReq.Select(l => l.Total).ToArray();
            int totalObtenido = 0;

            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
            .AddSeries(name: "HabilidaddesEmp " + tipoHab,
                    xValue: habilidades,
                    yValues: totales)
            .AddLegend()
            .AddTitle("Habilidades de los Empleados")
            .SetYAxis("Cantidad de Habilidades")
            .GetBytes("png");

            return File(chart, "image/bytes");
        }

        /*Consultas Julián*/

        /*Consultas Pablo*/





        public ActionResult MostrarLiderReq()
        {

            string reqBaja = "Exec Consulta_lideres_req_totales 'Baja'";
            string reqIntermedia = "Exec Consulta_lideres_req_totales 'Intermedia'";
            string reqAlta = "Exec Consulta_lideres_req_totales 'Alta'";

            var baja = (db.Database.SqlQuery<getLiderReqDificultad>(reqBaja)).ToList();
            var intermedia = (db.Database.SqlQuery<getLiderReqDificultad>(reqIntermedia)).ToList();
            var alta = (db.Database.SqlQuery<getLiderReqDificultad>(reqAlta)).ToList();

            ViewBag.getBaja = baja;
            ViewBag.getIntermedia = intermedia;
            ViewBag.getAlta = alta;

            string queryTotalReq = "Exec Consulta_lideres_totales ";
            var total = (db.Database.SqlQuery<getLiderReq>(queryTotalReq)).ToList();

            ViewBag.getLiderReq = total;


            return View();
        }

        public ActionResult GraficoLideresReq()
        {
            string reqBaja = "Exec Consulta_lideres_req_totales 'Baja'";
            string reqIntermedia = "Exec Consulta_lideres_req_totales 'Intermedia'";
            string reqAlta = "Exec Consulta_lideres_req_totales 'Alta'";

            var baja = (db.Database.SqlQuery<getLiderReqDificultad>(reqBaja)).ToList();
            var intermedia = (db.Database.SqlQuery<getLiderReqDificultad>(reqIntermedia)).ToList();
            var alta = (db.Database.SqlQuery<getLiderReqDificultad>(reqAlta)).ToList();
            //Se hace el query a la base de datos

            string[] cantidadBaja = baja.Select(l => l.req.ToString()).ToArray();
            string[] cantidadIntermedia = intermedia.Select(l => l.req.ToString()).ToArray();
            string[] cantidadAlta = alta.Select(l => l.req.ToString()).ToArray();

            string[] nombreBaja = baja.Select(l => l.nombre.ToString()).ToArray();
            string[] nombreIntermedia = intermedia.Select(l => l.nombre.ToString()).ToArray();
            string[] nombreAlta = alta.Select(l => l.nombre.ToString()).ToArray();


            int xValue = 0;
            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
            .AddSeries(name: "Baja",
                    xValue: nombreBaja,
                    yValues: cantidadBaja)
            .AddSeries(name: "Intermedio",
                    yValues: cantidadIntermedia)
            .AddSeries(name: "Alta",
                    yValues: cantidadAlta)
            .AddLegend()

            .AddTitle("Desempeño de lideres")
            .SetYAxis("Cantidad de Requerimientos")
            .SetXAxis("Lider")
            //.DataBindTable(dataSource: nombreBaja, xField: "Name")
            .GetBytes("png");
            return File(chart, "image/bytes");
        }
        //-------
        public ActionResult TesterReq()
        {
            List<SelectListItem> empleados = empleadosController.getEmpleados();
            ViewBag.emp = empleados;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TesterReq(string idEmp)
        {
            return RedirectToAction("MostrarTesterReq", new { idEmp = Request.Form["emp"].ToString() });
        }

        public ActionResult MostrarTesterReq(string idEmp)
        {
            ViewBag.idEmp = idEmp;
            return View();
        }

        public ActionResult GraficoTesterReq(string testerId)
        {
            
            string consulHab = "Consultar_Num_Habilidades_Equipo '" + testerId + "'";
            var tempEstadoReq = (db.Database.SqlQuery<NumHab>(consulHab)).ToList();
            string[] habilidades = tempEstadoReq.Select(l => l.Habilidad.ToString()).ToArray();
            int[] totales = tempEstadoReq.Select(l => l.Total).ToArray();
            int totalObtenido = 0;

            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
            .AddSeries(name: "HabilidaddesEmp " + testerId,
                    xValue: habilidades,
                    yValues: totales)
            .AddLegend()
            .AddTitle("Habilidades de los Empleados")
            .SetYAxis("Cantidad de Habilidades")
            .GetBytes("png");

            return File(chart, "image/bytes");
        }
        /*Consultas Pablo*/




        /* Consultas Esteban*/

        public ActionResult ProyRequerimientos()
        {
            // Envio todos los proyectos
            ViewBag.proy = new SelectList(db.PROYECTO, "idPK", "nombre");
            // Envio todos los requerimientos
            List<SelectListItem> requerimientos = new List<SelectListItem>(from req in db.REQUERIMIENTOS select new SelectListItem { Value = req.idFK, Text = req.nombrePK });
            foreach (var item in requerimientos)
            {
                string value = item.Value;
                string text = item.Text;
            }

            ViewBag.req = requerimientos;
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
