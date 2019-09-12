using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manual_ASP_NET_WEB.ViewModels
{
    public class InformacionGeneralViewModel
    {
        // Cantidad Pacientes
        public int CantidadPacientes { get; set; }
        // Cantidad Medicos
        public int CantidadMedicos { get; set; }
        // Cantidad Consultas
        public int CantidadConsultas { get; set; }
        public int CantidadPacientes60 { get; set; }
    }
}