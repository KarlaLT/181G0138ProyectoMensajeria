using System;
using System.Collections.Generic;

namespace APIProyectoMensajeria.Models
{
    public partial class Dato
    {
        public Dato()
        {
            DatosClases = new HashSet<DatosClase>();
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string NoControl { get; set; } = null!;

        public virtual ICollection<DatosClase> DatosClases { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
