using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using testColegio.ORM;

namespace testColegio.Models
{
    public class SolicitudView
    {
        public int IdSolicitud { get; set; }
        public List<Alumno> alumnos { get; set; }


        public int IdAlumno { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public string CodRegistrante { get; set; }


        public string Carrera { get; set; }


        public string Periodo { get; set; }

        public List<SolicitudViewDetail> detalle { get; set; }


    }

    public class SolicitudViewDetail
    {
        public int Idcurso { get; set; }
        public string cursoDesc { get; set; }
        public string profesor { get; set; }
        public string aula { get; set; }
        public string sede { get; set; }
        public string observacion { get; set; }
    }

}