using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonIgnore]

        public virtual Carrera? IdCarreraNavigation { get; set; }
        [JsonIgnore]

        public virtual ICollection<Clase> Clases { get; set; }
    }
}
