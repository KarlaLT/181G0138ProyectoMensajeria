using System;
using System.Collections.Generic;

namespace APIProyectoMensajeria.Models
{
    public partial class UsuariosClase
    {
        public int Id { get; set; }
        public int? IdEstudiante { get; set; }
        public int? IdClase { get; set; }

        public virtual Clase? IdClaseNavigation { get; set; }
        public virtual Usuario? IdEstudianteNavigation { get; set; }
    }
}
