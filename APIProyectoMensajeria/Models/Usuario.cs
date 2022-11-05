using System;
using System.Collections.Generic;

namespace APIProyectoMensajeria.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            MensajeIdEmisorNavigations = new HashSet<Mensaje>();
            MensajeIdRemitenteNavigations = new HashSet<Mensaje>();
        }

        public int Id { get; set; }
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? IdUsuario { get; set; }

        public virtual Dato? IdUsuarioNavigation { get; set; }
        public virtual ICollection<Mensaje> MensajeIdEmisorNavigations { get; set; }
        public virtual ICollection<Mensaje> MensajeIdRemitenteNavigations { get; set; }
    }
}
