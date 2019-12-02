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
            ViewBag.myRol = rol;
           
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
            //Se llama a el controlador de empleados para qeu se envíe la lista de empleados
            List<SelectListItem> empleados = empleadosController.getEmpleados();
            ViewBag.emp = empleados;
            //Se retorna la vista
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HistorialReq(string idEmp)
        {
            //Se redirige a mostrar el historial con el ID del empleado escogido
            return RedirectToAction("MostrarHistorialReq", new { idEmp = Request.Form["emp"].ToString() });
        }

        public ActionResult MostrarHistorialReq(string idEmp)
        {
            //Se pasa el id a la vista
            ViewBag.idEmp = idEmp;
            //Se arma la consultay se pasa el resultado a la vista
            string consulHistorial = "Exec Consultar_Historial_Req_Tester '" + idEmp + "'";
            var tempEstadoReq = (db.Database.SqlQuery<HistorialReq>(consulHistorial)).ToList();
            ViewData["Historial"] = tempEstadoReq;
            return View();
        }

        public ActionResult MostrarHabilidadesEmpleados()
        {
            //Se arma las consultas
            string consulHabT = "Consultar_Num_Habilidades_Empleados '" + "Tecnica" + "'";
            string consulHabB = "Consultar_Num_Habilidades_Empleados '" + "Blanda" + "'";
            //Se ejecuta query a base de datos
            var tempHabT = (db.Database.SqlQuery<NumHab>(consulHabT)).ToList();
            var tempHabB = (db.Database.SqlQuery<NumHab>(consulHabB)).ToList();
            //Se pasan a la vista
            ViewData["HabT"] = tempHabT;
            ViewData["HabB"] = tempHabB;
            
            return View();
        }


        public ActionResult HabilidadesEquipo()
        {
            //Se obtiene la lista de proyectos disponibles
            ViewBag.proy = new SelectList(db.PROYECTO, "idPK", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HabilidadesEquipo(string proy)
        {
            //Se pasa el ID del proyecto escogido a la siguiente vista
            return RedirectToAction("MostrarHabilidadesEquipo", new { proy = Request.Form["proy"].ToString() });
        }

        public ActionResult MostrarHabilidadesEquipo(string proy)
        {
            //Se arman las consultas
            ViewBag.idProy = proy;
            string consulHabT = "Consultar_Num_Habilidades_Equipo'" + "Tecnica" + "' ,'" + proy + "'";
            string consulHabB = "Consultar_Num_Habilidades_Equipo'" + "Blanda" + "' ,'"  + proy + "'";
            //Se ejecuta los querys
            var tempHabT = (db.Database.SqlQuery<NumHab>(consulHabT)).ToList();
            var tempHabB = (db.Database.SqlQuery<NumHab>(consulHabB)).ToList();
            //Se pasa los resultados a la vista
            ViewData["HabT"] = tempHabT;
            ViewData["HabB"] = tempHabB;

            return View();
        }

        public ActionResult GraficoHistorialReq(string idEmp)
        {
            ViewBag.idEmp = idEmp;
            //Se arma y ejecuta el query dependiendo
            string consulHistorial = "Exec Consultar_Historial_Req_Tester '" + idEmp + "'"; 
            var tempEstadoReq = (db.Database.SqlQuery<HistorialReq>(consulHistorial)).ToList();
            //Se arman los ejes del gráfico
            string[] estados = tempEstadoReq.Select(l => l.Estado.ToString()).ToArray();
            string[] totales = tempEstadoReq.Select(l => l.Total.ToString()).ToArray();
            //Se crea el gráfico y se retorna como una imagen
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
            //Se arma y se ejecuta el query
            string consulHab = "Consultar_Num_Habilidades_Empleados '" + tipoHab + "'";
            var tempNumHabEmp = (db.Database.SqlQuery<NumHab>(consulHab)).ToList();
            //Se arman los ejes del gráfico
            string[] habilidades = tempNumHabEmp.Select(l => l.Habilidad.ToString()).ToArray();
            int[] totales = tempNumHabEmp.Select(l => l.Total).ToArray();
            //Se obtiene el porcentaje para poder mostrarlo
            int totalObtenido = 0;
            foreach(int i in totales)
            {
                totalObtenido += i; 
            }
            for(int i=0; i<habilidades.Length;i++)
            {
               habilidades[i]=habilidades[i]+" ("+ ((double)totales[i]/(double)totalObtenido*100.00).ToString("#.##") +"%)";
            }
            //Se construye el gráfico con los resultados y porcentajes obtenidos
            var chart = new System.Web.Helpers.Chart(width: 513, height: 400)
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
            //Se arma la consulta y se ejecuta
            string consulHab = "Consultar_Num_Habilidades_Equipo'" + tipoHab + "' ,'"+idProy+"'";
            var tempNumHabEmp = (db.Database.SqlQuery<NumHab>(consulHab)).ToList();
            //Se construyen los ejes del gráfico
            string[] habilidades = tempNumHabEmp.Select(l => l.Habilidad.ToString()).ToArray();
            int[] totales = tempNumHabEmp.Select(l => l.Total).ToArray();
            //Se arma el gráfico y se retorna como una imagen.
            var chart = new System.Web.Helpers.Chart(width: 513, height: 400)
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
            string reqBaja = "Exec [Consulta_lider_req_dificultad] 'Baja'";
            string reqIntermedia = "Exec [Consulta_lider_req_dificultad] 'Intermedia'";
            string reqAlta = "Exec [Consulta_lider_req_dificultad] 'Alta'";

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
            string lideres = "Exec Consulta_lideres_id";
            var liderList = (db.Database.SqlQuery<getLideres>(lideres)).ToList();
            string[] lider = liderList.Select(l => l.nombre.ToString()).ToArray();
            string[] liderId = liderList.Select(l => l.cedula.ToString()).ToArray();

            string consulta = "EXEC Consulta_lider_get_proy";


            string reqBaja = "Exec Consulta_lideres_req_totales 'Baja'";
            string reqIntermedia = "Exec Consulta_lideres_req_totales 'Intermedia'";
            string reqAlta = "Exec Consulta_lideres_req_totales 'Alta'";

            var baja = (db.Database.SqlQuery<getLiderReqDificultad>(reqBaja)).ToList();
            var intermedia = (db.Database.SqlQuery<getLiderReqDificultad>(reqIntermedia)).ToList();
            var alta = (db.Database.SqlQuery<getLiderReqDificultad>(reqAlta)).ToList();
            //Se hace el query a la base de datos

            int[] cantidadBaja = baja.Select(l => l.req).ToArray();
            int[] cantidadIntermedia = intermedia.Select(l => l.req).ToArray();
            int[] cantidadAlta = alta.Select(l => l.req).ToArray();

            string[] nombreBaja = baja.Select(l => l.cedula.ToString()).ToArray();
            string[] nombreIntermedia = intermedia.Select(l => l.cedula.ToString()).ToArray();
            string[] nombreAlta = alta.Select(l => l.cedula.ToString()).ToArray();

            int[] bajas = new int[lider.Length];
            int[] intermedias = new int[lider.Length];
            int[] altas = new int[lider.Length];


            for (int i = 0; i < lider.Length; ++i)
            {
                for (int j = 0; j < nombreBaja.Length; ++j)
                {
                    if (nombreBaja[j] == liderId[i])
                    {
                        bajas[i] = cantidadBaja[j];
                        break;
                    }
                    else
                        bajas[i] = 0;
                }
                for (int j = 0; j < nombreIntermedia.Length; ++j)
                {
                    if (nombreIntermedia[j] == liderId[i])
                    {
                        intermedias[i] = cantidadIntermedia[j];
                        break;
                    }
                    else
                        intermedias[i] = 0;
                }
                for (int j = 0; j < nombreAlta.Length; ++j)
                {
                    if (nombreAlta[j] == liderId[i])
                    {
                        altas[i] = cantidadAlta[j];
                        break;
                    }
                    else
                        altas[i] = 0;
                }
            }

            int xValue = 0;
            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
            .AddSeries(name: "Baja",
                    xValue: lider,
                    yValues: bajas)
            .AddSeries(name: "Intermedio",
                    yValues: intermedias)
            .AddSeries(name: "Alta",
                    yValues: altas)
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
            return RedirectToAction("MostrarTesterReq", new { testerId = Request.Form["emp"].ToString() });
        }

        public ActionResult MostrarTesterReq(string testerId)
        {
            ViewBag.idEmp = testerId;
            string proy = "Exec Consulta_Tester_Req_Global '" + testerId + "'";
            var proyecto = (db.Database.SqlQuery<TesterParticipacionGlobalReq>(proy)).ToList();
            ViewBag.proyectoDatos = proyecto;

            string procedure = "Exec Consulta_Tester_Req_Dificultad '" + testerId + "'";
            var data = (db.Database.SqlQuery<TesterParticipacion>(procedure)).ToList();
            ViewBag.data = data;

            string percentage = "Exec Consulta_Tester_Req_Percentage_Total '" + testerId + "'";
            var testerData = (db.Database.SqlQuery<TesterParticipacionGlobal>(percentage)).ToList();
            ViewBag.porcentaje= testerData;

            return View();
        }

        public ActionResult GraficoTesterReq(string testerId)
        {
            ViewBag.idEmp = testerId;
            string procedure = "Exec Consulta_Tester_Req_Dificultad '" + testerId + "'";
            var testerData = (db.Database.SqlQuery<TesterParticipacion>(procedure)).ToList();
            int[] baja = testerData.Select(l => l.baja).ToArray();
            int[] intermedia = testerData.Select(l => l.intermedia).ToArray();
            int[] alta = testerData.Select(l => l.alta).ToArray();

            int[] porcentaje = new int[3];
            int total = baja[0] + intermedia[0] + alta[0];
            porcentaje[0] = (baja[0] * 100) / total;
            porcentaje[1] = (intermedia[0] * 100) / total;
            porcentaje[2] = (alta[0] * 100) / total;

            string[] nombres = new string[3];
            nombres[0] = "Baja";
            nombres[1] = "Intermedia";
            nombres[2] = "Alta";

            var chart = new System.Web.Helpers.Chart(width: 513, height: 400)
             .AddSeries(name: "Requerimientos realizados", chartType: "Pie",
                     xValue: nombres,
                     yValues: porcentaje)
             .AddLegend()
             .AddTitle("Requerimientos realizados")

             .GetBytes("png");

            return File(chart, "image/bytes");
        }
        public ActionResult GraficoTesterReqGlobal(string testerId)
        {
            ViewBag.idEmp = testerId;
            string procedure = "Exec Consulta_Tester_Req_Percentage_Total '" + testerId + "'";
            var testerData = (db.Database.SqlQuery<TesterParticipacionGlobal>(procedure)).ToList();
            int[] total = testerData.Select(l => l.total).ToArray();
            int[] partipacion = testerData.Select(l => l.participacion).ToArray();
            string[] nombre = testerData.Select(l => l.nombre.ToString()).ToArray();

            int[] porcentaje = new int[2];
            porcentaje[0] = (partipacion[0] * 100) / total[0];
            porcentaje[1] = 100 - porcentaje[0] ;

            string[] nombres = new string[2];
            nombres[0] = nombre[0];
            nombres[1] = "Total";

            var chart = new System.Web.Helpers.Chart(width: 513, height: 400)
             .AddSeries(name: "Requerimientos realizados", chartType: "Pie",
                     xValue: nombres,
                     yValues: porcentaje)
             .AddLegend()
             .AddTitle("Requerimientos realizados")

             .GetBytes("png");

            return File(chart, "image/bytes");
        }
        public ActionResult GraficoTesterBarrasReq(string testerId)
        {
            string proy = "Exec Consulta_tester_proyectos '" + testerId + "'";
            var proyecto = (db.Database.SqlQuery<getProyectos>(proy)).ToList();
            string[] proyectos = proyecto.Select(l => l.proyecto.ToString()).ToArray();
            string[] nombreProy = proyecto.Select(l => l.proyectoNombre.ToString()).ToArray();

            string consultaAlta = "Exec Consulta_Tester_Req_Percentage 'Alta', '" + testerId + "', '" + proyectos[0] + "'";
            var porcentajeA = (db.Database.SqlQuery<getPorcentajes>(consultaAlta)).ToList();
            int[] porcentajeAlta = porcentajeA.Select(l => l.parcial).ToArray();

            string consultaIntermedia = "Exec Consulta_Tester_Req_Percentage 'Intermedia', '" + testerId + "', '" + proyectos[0] + "'";
            var porcentajeI = (db.Database.SqlQuery<getPorcentajes>(consultaIntermedia)).ToList();
            int[] porcentajeIntermedia = porcentajeA.Select(l => l.parcial).ToArray();

            string consultaBaja = "Exec Consulta_Tester_Req_Percentage 'Baja', '" + testerId + "', '" + 7 + "'";
            var porcentajeB = (db.Database.SqlQuery<getPorcentajes>(consultaBaja)).ToList();
            int[] porcentajeBaja = porcentajeA.Select(l => l.parcial).ToArray();

            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
              .AddSeries(name: "Baja",

                      yValues: porcentajeBaja)

              .AddLegend()

              .AddTitle("Desempeño de lideres")
              .SetYAxis("Cantidad de Requerimientos")
              .SetXAxis("Lider")
              //.DataBindTable(dataSource: nombreBaja, xField: "Name")
              .GetBytes("png");
            return File(chart, "image/bytes");
            return File(chart, "image/bytes");
        }
        /*Consultas Pablo*/


        /* Consultas Esteban*/
        // Pruebas de un proyecto, agrupadas por estado final y el tester responsable de cada requerimiento
        public async Task<ActionResult> ProyRequerimientos()
        {
            string correo = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(correo);
            ViewBag.rol = rol;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProyRequerimientos(string proyecto)
        {
            string proy = Request.Form["proy"].ToString();
            return RedirectToAction("MostrarTotalPruebasProy", new { proy });
        }

        public async Task<ActionResult> MostrarTotalPruebasProy(string proy)
        {
            string correo = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(correo);
            ViewBag.rol = rol;
            ViewBag.idproy = proy;
            var proyecto = (from proyectos in db.PROYECTO where proyectos.idPK == proy select new { proyectos.nombre }).ToList(); // Jala el nombre del proyecto
            string nombreProyecto = proyecto.First().ToString(); // "{ nombre = nombreProyecto }
            nombreProyecto = nombreProyecto.Substring(10); // "nombreProyecto }
            nombreProyecto = nombreProyecto.Replace("}", "");
            ViewBag.nombreProy = nombreProyecto;
            return View();
        }

        public ActionResult GraficoMostrarTotalPruebasProy(string proy)
         {
             
            string queryPruebasProy = "Exec pruebas_proyecto" + "'" + proy + "'";  // Se hace el query a la base de datos
            var tempPruebasProy = (db.Database.SqlQuery<PruebasProy>(queryPruebasProy)).ToList();
            /*A continuacion, se muestran ejemplos de resultados devueltos por el query y la forma de manejarlos
             para poder obtener los datos correctos y poder armar el grafico*/
            string[] estadosFinales = tempPruebasProy.Select(pruebasProy => pruebasProy.EstadoFinal.ToString()).ToArray();
            // [Exitoso , Fallido, Exitoso, Incompleto] 
            string[] testersResponsables = tempPruebasProy.Select(pruebasProy => pruebasProy.TesterResponsable.ToString()).ToArray();
            // [Andres2 , Andres2, Andres3, Andres3]
            int[] cantidadPruebas = tempPruebasProy.Select(pruebasProy => pruebasProy.CantidadPruebas).ToArray();
            // [1 , 1, 2, 1]
            List<string> testersSinRepetir = new List<string>();
            for(int i = 0; i < testersResponsables.Length; ++i)
            // Añade los DISTINTOS testers que se encuentran en el array de los testers
            {
                if (!(testersSinRepetir.Contains(testersResponsables[i])))
                {
                    testersSinRepetir.Add(testersResponsables[i]);
                }
            }
            /* Se necesitan 3 arreglos con la cantidad de fallidas, la cantidad de exitosas y la cantidad de incompletas 
             por cada tester distinto. Una forma de hacer esto es que cada tester estara indexado en la lista
             con sus respectivas cantidad de pruebas*/ 
            int[] cantidadExitosas = new int[testersSinRepetir.Count];
            int[] cantidadFallidas = new int[testersSinRepetir.Count];
            int[] cantidadIncompletas = new int[testersSinRepetir.Count];
            for(int i = 0; i < estadosFinales.Length; ++i) 
            // Todos los arrays que vienen del proc almacenado tienen la misma indexacion
            {
                int index = testersSinRepetir.IndexOf(testersResponsables[i], 0);
                if(estadosFinales[i] == "Exitoso")
                {
                    cantidadExitosas[index] = cantidadPruebas[i]; // Anade las pruebas exitosas para el tester en la posicion index
                }else if(estadosFinales[i] == "Fallido")
                {
                    cantidadFallidas[index] = cantidadPruebas[i];
                }
                else // Incompletas
                {
                    cantidadIncompletas[index] = cantidadPruebas[i];
                }
            }
            /*Un ejemplo final del resultado podria ser:
             [Esteban, Andres, Roger] // Testers sin repetir
             [0, 0, 0] // Cantidad de pruebas fallidas, donde Esteban tiene 0, Andres 0 y Roger 0
             [1, 2, 3] // Cantidad de pruebas incompletas, donde Esteban tiene 1, Andres 2 y Roger 3
             [4, 2, 1] // Cantidad de pruebas exitosas, donde Esteban tiene 4, Andres 2 y Roger 1 */

            string[] arrayTesterSinRepetir = testersSinRepetir.ToArray(); // Se pasa a un array de string para poder pasarlo al graficador

            var chart = new System.Web.Helpers.Chart(width: 900, height: 450) // Armar el grafico
            .AddTitle("Pruebas del proyecto")
            .AddSeries(name: "Exitosas",
                    chartType: "column",
                    xValue: arrayTesterSinRepetir,
                    yValues: cantidadExitosas)
            .AddSeries(name: "Fallidas",
                    chartType: "column",
                    xValue: arrayTesterSinRepetir,
                    yValues: cantidadFallidas)
            .AddSeries(name: "Incompletas",
                    chartType: "column",
                    xValue: arrayTesterSinRepetir,
                    yValues: cantidadIncompletas)
            .AddLegend()
            .SetYAxis("Cantidad de Pruebas")
            .SetXAxis("Testers Responsables")
            .GetBytes("png");
            return File(chart, "image/bytes");
         }

        
        // Pruebas de un requerimiento, agrupadas por estado final

        public async Task<ActionResult> PruebasReq()
        {
            string correo = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(correo);
            ViewBag.rol = rol;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PruebasReq(string idFKproyecto, string nombreRequerimiento)
        {
            string proy = Request.Form["proy"].ToString(); // Valor seleccionado en el dropdown de proyectos
            string req = Request.Form["requerimientos"].ToString(); // Valor seleccionado en el dropdown de requerimientos
            return RedirectToAction("MostrarPruebasReq", new { idProyFK = proy, nombreRequerimiento = req });
        }

        public async Task<ActionResult> MostrarPruebasReq(string idProyFK, string nombreRequerimiento)
        {
            string correo = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(correo);
            ViewBag.rol = rol;
            ViewBag.idproy = idProyFK;
            ViewBag.nombreReq = nombreRequerimiento;
            return View();
        }
        
        public ActionResult GraficoMostrarMostrarPruebasReq(string idProyFK, string nombreRequerimiento)
        {
            string queryPruebasPorEstado = "EXEC cantidad_pruebas_por_estado " + "'" + nombreRequerimiento + "','" + idProyFK + "';";
            List<PruebasPorEstado> resultado = (db.Database.SqlQuery<PruebasPorEstado>(queryPruebasPorEstado)).ToList();
            if (resultado.Count() == 2) // Solo hay de 2 tipos
            {
                // Averiguar el estado de los 2 tipos de pruebas que hay
                // Algoritmo que encuentra el tipo de pruebas que hay, y rellena el array con el tipo de pruebas que faltan y le pone como cantidad 0
                PruebasPorEstado incompletas = resultado.Find(x => x.estadoFinal == "Incompleto");
                if (incompletas == null)
                {
                    incompletas = new PruebasPorEstado();
                    incompletas.estadoFinal = "Incompleto";
                    incompletas.cantidad = 0;
                    resultado.Add(incompletas);
                }
                PruebasPorEstado exitosas = resultado.Find(x => x.estadoFinal == "Exitoso");
                if (exitosas == null)
                {
                    exitosas = new PruebasPorEstado();
                    exitosas.estadoFinal = "Exitoso";
                    exitosas.cantidad = 0;
                    resultado.Add(exitosas);
                }
                PruebasPorEstado fallidas = resultado.Find(x => x.estadoFinal == "Fallido");
                if (fallidas == null)
                {
                    fallidas = new PruebasPorEstado();
                    fallidas.estadoFinal = "Fallido";
                    fallidas.cantidad = 0;
                    resultado.Add(fallidas);
                }
            }
            else if (resultado.Count() == 1) // Solo hay de un tipo
             // Algoritmo que encuentra el tipo de pruebas que hay, y rellena el array con el tipo de pruebas que faltan y le pone como cantidad 0
            {
                // Averiguar el estado de la unica prueba 
                if (resultado.ElementAt(0).estadoFinal == "Exitoso")
                {
                    PruebasPorEstado fallidas = new PruebasPorEstado();
                    fallidas.cantidad = 0;
                    fallidas.estadoFinal = "Fallido";
                    resultado.Add(fallidas);
                    PruebasPorEstado incompletas = new PruebasPorEstado();
                    incompletas.cantidad = 0;
                    incompletas.estadoFinal = "Incompleto";
                    resultado.Add(incompletas);
                }
                else if (resultado.ElementAt(0).estadoFinal == "Fallido")
                {
                    PruebasPorEstado exitosas = new PruebasPorEstado();
                    exitosas.cantidad = 0;
                    exitosas.estadoFinal = "Exitoso";
                    resultado.Add(exitosas);
                    PruebasPorEstado incompletas = new PruebasPorEstado();
                    incompletas.cantidad = 0;
                    incompletas.estadoFinal = "Incompleto";
                    resultado.Add(incompletas);
                }
                else
                {
                    PruebasPorEstado exitosas = new PruebasPorEstado();
                    exitosas.cantidad = 0;
                    exitosas.estadoFinal = "Exitoso";
                    resultado.Add(exitosas);
                    PruebasPorEstado fallidas = new PruebasPorEstado();
                    fallidas.cantidad = 0;
                    fallidas.estadoFinal = "Fallido";
                    resultado.Add(fallidas);
                }
            }
            string[] estados = resultado.Select(prueba => prueba.estadoFinal.ToString()).ToArray();
            int[] cantidad = resultado.Select(prueba => prueba.cantidad).ToArray();

            var chart = new System.Web.Helpers.Chart(width: 600, height: 400) // Armar el grafico
            .AddSeries(name: "Estado Pruebas",
                    xValue: estados,
                    yValues: cantidad)
            .AddLegend()
            .AddTitle("Estado de las Pruebas")
            .SetYAxis("Cantidad de Pruebas")
            .GetBytes("png");
            return File(chart, "image/bytes");
        }


        /*Consultas Andres*/
        public ActionResult DuracionProy()
        {
            return View(db.PROYECTO.Where(p => p.estado == "Finalizado").ToList());
        }

        public ActionResult MostrarDuracionProy(string proy)
        {
            ViewBag.idproy = proy;
            ViewBag.nombreProy = db.PROYECTO.Where(x => x.idPK == proy).First().nombre;

            string queryDurProy = "Exec Consulta_Duracion_Proy" + "'" + proy + "'";
            //Se hace el query a la base de datos
            var tempDurProy = (db.Database.SqlQuery<DuracionProy>(queryDurProy)).ToList();

            ViewBag.Consulta = tempDurProy;
            return View();
        }

        public ActionResult GraficoDuracionProy(string proy)
        {
            ViewBag.idproy = proy;

            var consultaGrafico = (from proyecto in db.PROYECTO
                                   where proyecto.idPK == proy
                                   select new
                                   {
                                       durEst = proyecto.duracionEstimada,
                                       durReal = proyecto.duracionReal
                                   });
            int duracionEstimada = 0;
            int duracionReal = 0;
            foreach (var item in consultaGrafico.ToList())
            {
                duracionEstimada = item.durEst.Value;
                duracionReal = item.durReal.Value;
            }

            int[] horas = new int[2];
            horas[0] = duracionEstimada;
            horas[1] = duracionReal;
            
            var chart = new System.Web.Helpers.Chart(width: 700, height: 400, themePath: "~/Img/estiloDurProy.xml")
            .AddSeries(name: "Duraciones",
                    xValue: new[] { "Duracion Estimada", "Duracion Real" },
                    yValues: horas)
            .AddTitle("Análisis de Resultados", "TituloDurProy")
            .SetYAxis("Cantidad de Horas")
            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult HorasPorReq()
        {
            return View(db.PROYECTO.Where(p => p.estado == "Finalizado").ToList());
        }

        public ActionResult HorasPorReqTester(string proy)
        {
            ViewBag.proy = proy;
            return View(db.PARTICIPA.Include(p => p.EMPLEADO).Where(p => p.idProyectoFK == proy).Where(p => p.EMPLEADO.rol == "Tester").ToList());
        }

        public ActionResult MostrarHorasPorReq(string proy, string tester)
        {
            ViewBag.idproy = proy;
            ViewBag.tester = tester;
            ViewBag.nombreProy = db.PROYECTO.Where(x => x.idPK == proy).First().nombre;
            ViewBag.nombre = db.EMPLEADO.Where(x => x.cedulaPK == tester).First().nombre;
            ViewBag.apellido = db.EMPLEADO.Where(x => x.cedulaPK == tester).First().primerApellido;

            return View();
        }

        public ActionResult GraficoHorasPorReq(string proy, string tester)
        {
            string queryHorasPorReq = "Exec Consulta_Horas_Por_Req" + "'" + proy + "','" + tester + "'";
            //Se hace el query a la base de datos
            var tempHorasPorReq = (db.Database.SqlQuery<HorasReq>(queryHorasPorReq)).ToList();

            string[] reqs = tempHorasPorReq.Select(l => l.nombreReq.ToString()).ToArray();
            string[] horasR = tempHorasPorReq.Select(l => l.horasEstimadas.ToString()).ToArray();
            string[] horasE = tempHorasPorReq.Select(l => l.horasReales.ToString()).ToArray();

            var chart = new System.Web.Helpers.Chart(width: 900, height: 450, themePath: "~/Img/estiloHorPorReq.xml")
            .AddTitle("Análisis de Resultados", "TituloHorPorReq")
            .AddSeries(name: "Horas Estimadas",
                    chartType: "column",
                    xValue: reqs,
                    yValues: horasE)
            .AddSeries(name: "Horas Reales",
                    chartType: "column",
                    xValue: reqs,
                    yValues: horasR)
            .AddLegend()
            .SetYAxis("Cantidad de Horas")
            .SetXAxis("Requerimientos")
            .GetBytes("png");
            return File(chart, "image/bytes");
        }
        /*Consultas Andres*/

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
