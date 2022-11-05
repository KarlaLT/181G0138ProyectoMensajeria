using System;
using System.Collections.Generic;

namespace APIProyectoMensajeria.Models
{
    public partial class DatosClase
    {
        public int Id { get; set; }
        public int? IdEstudiante { get; set; }
        public int? IdClase { get; set; }

        public virtual Clase? IdClaseNavigation { get; set; }
        public virtual Dato? IdEstudianteNavigation { get; set; }
    }
}
