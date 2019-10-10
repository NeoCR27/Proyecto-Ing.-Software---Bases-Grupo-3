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

    public class HABILIDADESController : Controller
    {

        private EMPLEADOController empleadosController = new EMPLEADOController();

        private Gr03Proy4Entities db = new Gr03Proy4Entities();

        private SeguridadController seguridad_controller = new SeguridadController();

        private static string valorViejo = null;

        private static string tipoViejo = null;

        private static string idViejo = null;

        // Despliega las habilidades del respectivo empleado con la cedula  igual al id
        public async Task<ActionResult> Index(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string correo = User.Identity.Name;
            string rol = await this.seguridad_controller.GetRol(correo);
            ViewBag.miRol = rol;
            
            EMPLEADO empleado = db.EMPLEADO.Find(id);
            string nombre = empleado.nombre + " " + empleado.primerApellido;
            ViewBag.nombre = nombre;
            System.Diagnostics.Debug.WriteLine(nombre);

            var habilidades = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id);
            return View(habilidades.ToList());
        }

        // Despliega los detalles, no se usa 
        public ActionResult Details(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id), "", "valorPK");

            SelectList nombre = empleadosController.get_nombres(id);
            ViewBag.nombre = nombre;

            return View();
        }



        // Despliega la vista para crear habilidades
        public ActionResult Create()
        {
            List<SelectListItem> empleados = this.empleadosController.getEmpleados();

            ViewBag.cedulaEmpleadoFK = empleadosController.get_cedulas();
            string[] values = new[] { "Tecnica", "Blanda" };
            ViewBag.tipoPK = new SelectList(values);

            //return View(hABILIDADES.ToList());
            return View();

        }

        // Recibe los valores de la habilidad y la agrega al respectivo empleado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "valorPK,tipoPK,cedulaEmpleadoFK")] HABILIDADES hABILIDADES)
        {
            HABILIDADES duplicate = db.HABILIDADES.Find(hABILIDADES.valorPK, hABILIDADES.tipoPK, hABILIDADES.cedulaEmpleadoFK);
            //HABILIDADES habilidad_viejas = await db.HABILIDADES.FindAsync(valor_viejo, tipo_viejo, id_viejo);
            if (duplicate == null)
            {
                if (ModelState.IsValid)
                {
                    db.HABILIDADES.Add(hABILIDADES);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = hABILIDADES.cedulaEmpleadoFK });
                }
            }
            else
            {
                return RedirectToAction("NoDuplicados");
            }

            return RedirectToAction("Index", new { id = hABILIDADES.cedulaEmpleadoFK });
        }

        // Despliega las vista para editar una habilidad
        //Primero guarda los valores viejos para luego borrarlos
        public ActionResult Edit(string id, string valor, string tipo)
        {
            string[] values = new[] { "Tecnica", "Blanda" };
            ViewBag.tipoPK = new SelectList(values);

            idViejo = string.Copy(id);
            valorViejo = string.Copy(valor);
            tipoViejo = string.Copy(tipo);

            if (id == null && valor == null && tipo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor), "cedulaEmpleadoFK", "tipoPK", "valorPK");
            ViewBag.ced = id;

            var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor);

            return View(hABILIDADES);
        }

        // Recibe los valores del editar
        //Primero borra los valores viejos 
        //Luego agrega los nuevos valores de la habilidad
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editconfirm(string id, string valor, string tipo)
        {

            HABILIDADES habilidad_viejas = await db.HABILIDADES.FindAsync(valorViejo, tipoViejo, idViejo);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.HABILIDADES.Remove(habilidad_viejas);
            await db.SaveChangesAsync();

            HABILIDADES habilidadesActuales = db.HABILIDADES.Create();

            habilidadesActuales.cedulaEmpleadoFK = id;
            habilidadesActuales.tipoPK = tipo;
            habilidadesActuales.valorPK = valor;
            HABILIDADES duplicada = db.HABILIDADES.Find(habilidadesActuales.valorPK, habilidadesActuales.tipoPK, habilidadesActuales.cedulaEmpleadoFK);

            if (duplicada == null)
            {
                db.HABILIDADES.Add(habilidadesActuales);
                await db.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction("NoDuplicados");
            }

            return RedirectToAction("Index", new { id = habilidadesActuales.cedulaEmpleadoFK });
        }
        //Despliega vista para casos de elementos duplicados
        public ActionResult NoDuplicados()
        {
            return View();
        }
        // Despliega la vista de eliminar habilidad
        public ActionResult Delete(string id, string valor, string tipo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.habilidades = new SelectList(db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor), "cedulaEmpleadoFK", "tipoPK", "valorPK");
            ViewBag.ced = id;
            var hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id && x.tipoPK == tipo && x.valorPK == valor);

            return View(hABILIDADES);
        }

        // Recibe los valores del eliminar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string valor, string tipo)
        {

            HABILIDADES habilidade = db.HABILIDADES.Find(valor, tipo, id);
            db.HABILIDADES.Remove(habilidade);
            db.SaveChanges();

            return RedirectToAction("Index", new { id = id });
        }

        //Redirecciona al controlador de empleado
        public ActionResult to_empleados()
        {
            return RedirectToAction("../EMPLEADO/Index", new { });
        }

        /*public async Task<ActionResult> DeleteConfirmed(string id, string valor, string tipo)
        {

            HABILIDADES habilidad = await db.HABILIDADES.FindAsync(id, valor, tipo);
            //HABILIDADES hABILIDADES = db.HABILIDADES.Where(x => x.cedulaEmpleadoFK == id);
            db.HABILIDADES.Remove(habilidad);
            await db.SaveChangesAsync();
            return RedirectToAction("../EMPLEADO/Index", new { id =id });
        }*/
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
