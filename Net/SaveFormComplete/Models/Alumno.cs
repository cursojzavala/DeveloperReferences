using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FormComplete.Models
{
    public class Alumno
    {

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        public string direccion { get; set; }

        [Required]
        public string sexo { get; set; }

        [Required(ErrorMessage ="Seleccione un curso")]
        public List<string> cursos { get; set; }

    }

    public class Curso
    {
        public int IdCurso { get; set; }
        public string Nombre { get; set; }
        public bool Select { get; set; } = false;

    }



}