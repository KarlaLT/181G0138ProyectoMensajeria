using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIProyectoMensajeria.Models
{
    public partial class Mensaje
    {
        public int Id { get; set; }
        public string Mensaje1 { get; set; } = null!;
        public int? IdEmisor { get; set; }
        public int? IdRemitente { get; set; }
        public DateTime Fecha { get; set; }

        [JsonIgnore]

        public virtual Usuario? IdEmisorNavigation { get; set; }
        [JsonIgnore]

        public virtual Usuario? IdRemitenteNavigation { get; set; }
    }
}
