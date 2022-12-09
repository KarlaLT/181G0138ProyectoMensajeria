using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual ICollection<Grupo> Grupos { get; set; }
    }
}
