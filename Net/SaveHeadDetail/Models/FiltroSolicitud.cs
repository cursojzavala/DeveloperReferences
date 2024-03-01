using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testColegio.Models
{
    public class FiltroSolicitud
    {
        public string Nombres {get;set;}
        public DateTime FechaSolicitud {get;set;}
        public string CodRegistrante { get;set;}
        public string Carrera {get;set;}
        public string Periodo {get;set;}
        public string Sede {get;set;}
        public string cursoDescripcion  {get;set;}
        public string Profesor {get;set;}
    }
}