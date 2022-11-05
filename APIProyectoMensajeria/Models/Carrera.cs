using System;
using System.Collections.Generic;

namespace APIProyectoMensajeria.Models
{
    public partial class Carrera
    {
        public Carrera()
        {
            Grupos = new HashSet<Grupo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Grupo> Grupos { get; set; }
    }
}
