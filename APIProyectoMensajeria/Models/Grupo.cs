using System;
using System.Collections.Generic;

namespace APIProyectoMensajeria.Models
{
    public partial class Grupo
    {
        public Grupo()
        {
            Clases = new HashSet<Clase>();
        }

        public int Id { get; set; }
        public string Clave { get; set; } = null!;
        public int? IdCarrera { get; set; }

        public virtual Carrera? IdCarreraNavigation { get; set; }
        public virtual ICollection<Clase> Clases { get; set; }
    }
}
