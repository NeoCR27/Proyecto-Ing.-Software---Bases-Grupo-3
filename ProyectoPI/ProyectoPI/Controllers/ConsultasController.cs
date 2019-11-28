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
            string lideres = "Exec Consulta_lideres_id";
            var liderList = (db.Database.SqlQuery<getLideres>(lideres)).ToList();
            string[] lider = liderList.Select(l => l.nombre.ToString()).ToArray();
            string[] liderId = liderList.Select(l => l.cedula.ToString()).ToArray();

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

        public ActionResult GraficoTesterBarrasReq(string testerId)
        {
            string proy = "Exec Consulta_tester_proyectos '" + testerId + "'";
            var proyecto = (db.Database.SqlQuery<getProyectos>(proy)).ToList();
            string[] proyectos = proyecto.Select(l => l.proyecto.ToString()).ToArray();
            string[] nombreProy = proyecto.Select(l => l.proyectoNombre.ToString()).ToArray();

            string porcentaje = "Exec Consulta_Tester_Req_Percentage 'Alta', '" + testerId + "', '" + proyectos[0] + "'";
            var porcentajeA = (db.Database.SqlQuery<getPorcentajes>(porcentaje)).ToList();
            int[] porcentajeAlta = porcentajeA.Select(l => l.porcentaje).ToArray();
            
            //Se hace el query a la base de datos

            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
            .AddSeries(name: nombreProy[0],
                    yValues: porcentajeAlta)

            .AddLegend()

            .AddTitle("Desempeño de lideres")
            .SetYAxis("Cantidad de Requerimientos")
            .SetXAxis("Lider")
            //.DataBindTable(dataSource: nombreBaja, xField: "Name")
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProyRequerimientos(string proyecto, string requerimiento)
        {
            string proy = Request.Form["proy"].ToString();
            string req = Request.Form["requerimientos"].ToString();
            return RedirectToAction("MostrarTotalPruebasPorEstado", proy, req);
        }

        public ActionResult MostrarTotalPruebasPorEstado(string proy, string requerimiento)
        {
            /* ViewBag.idproy = proy;
             ViewBag.requerimiento = requerimiento;
             string queryCantReq = "Exec Consulta_Cantidad_Req_Tester" + "'" + proy + "'";
             //Se hace el query a la base de datos
             var tempCantReq = (db.Database.SqlQuery<CantReq>(queryCantReq)).ToList();

             string queryGetReq = "Exec Get_Req" + "'" + proy + "'";
             //Se hace el query a la base de datos
             var tempGetReq = (db.Database.SqlQuery<GetReq>(queryGetReq)).ToList();
             ViewBag.Req = tempGetReq;
             */

            return View();
        }

        /* public ActionResult GraficoPruebasPorEstado(string proy)
         {
             /*ViewBag.idproy = proy;
             System.Diagnostics.Debug.WriteLine("entro");
             string queryCantReq = "Exec Consulta_Cantidad_Req_Tester" + "'" + proy + "'";
             //Se hace el query a la base de datos
             var tempCantReq = (db.Database.SqlQuery<CantReq>(queryCantReq)).ToList();
             string[] nombres = tempCantReq.Select(l => l.nombre.ToString()).ToArray();
             string[] cantidad = tempCantReq.Select(l => l.Cantidad.ToString()).ToArray();

             var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
                     .AddSeries(name: "Testers",
                     xValue: nombres,
                     yValues: cantidad)
             .AddLegend()
             .AddTitle("Cantidad de Requerimientos por Tester")
             .SetYAxis("Cantidad de Requerimientos")
             .SetXAxis("Nombre")
             .GetBytes("png");
             return File(chart, "image/bytes");
         }*/

        /*Consultas Andres*/
        public ActionResult DuracionProy()
        {
            ViewBag.proy = new SelectList(db.PROYECTO, "idPK", "nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DuracionProy(string proyecto)
        {
            return RedirectToAction("MostrarDuracionProy", new { proy = Request.Form["proy"].ToString() });
        }

        public ActionResult MostrarDuracionProy(string proy)
        {
            ViewBag.idproy = proy;

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
                                       durReal = proyecto.duracionReal,
                                       nomProy = proyecto.nombre
                                   });
            string nombreProyecto = "";
            int duracionEstimada = 0;
            int duracionReal = 0;
            foreach (var item in consultaGrafico.ToList())
            {
                nombreProyecto = item.nomProy;
                duracionEstimada = item.durEst.Value;
                duracionReal = item.durReal.Value;
            }

            int[] horas = new int[2];
            horas[0] = duracionEstimada;
            horas[1] = duracionReal;

            //agregar aqui el nombre del proyecto
            var chart = new System.Web.Helpers.Chart(width: 700, height: 400)
            .AddSeries(name: "Duraciones",
                    xValue: new[] { "Duracion Estimada", "Duracion Real" },
                    yValues: horas)
            .AddTitle("Análisis de Duración del Proyecto " + nombreProyecto)
            .SetYAxis("Cantidad de Horas")
            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult HorasPorReq()
        {
            ViewBag.proy = new SelectList(db.PROYECTO, "idPK", "nombre");
            ViewBag.tester = new SelectList(db.EMPLEADO, "cedulaPK", "nombre");
            /*List<SelectListItem> empleados = new List<SelectListItem>(from req in db.REQUERIMIENTOS
                                                                           join proy in db.PROYECTO on req.idFK equals proy.idPK
                                                                           join empl in db.EMPLEADO on req.cedulaFK equals empl.cedulaPK
                                                                           where empl.rol == "Tester"
                                                                           select new SelectListItem { Value = empl.cedulaPK, Text = empl.nombre+" "+empl.primerApellido });

            ViewBag.empl = empleados;*/

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HorasPorReq(string proyecto, string empleado)
        {
            return RedirectToAction("MostrarHorasPorReq", new { proy = Request.Form["proy"].ToString(), tester = Request.Form["tester"].ToString() });
        }

        public ActionResult MostrarHorasPorReq(string proy, string tester)
        {
            ViewBag.idproy = proy;
            ViewBag.tester = tester;

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

            var chart = new System.Web.Helpers.Chart(width: 900, height: 450)
            .AddTitle("Análisis de Horas por Requerimiento")
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
