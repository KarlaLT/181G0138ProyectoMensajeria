using System;
using System.Collections.Generic;

namespace APIProyectoMensajeria.Models
{
    public partial class Mensaje
    {
        public int Id { get; set; }
        public string Mensaje1 { get; set; } = null!;
        public int? IdEmisor { get; set; }
        public int? IdRemitente { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Usuario? IdEmisorNavigation { get; set; }
        public virtual Usuario? IdRemitenteNavigation { get; set; }
    }
}
