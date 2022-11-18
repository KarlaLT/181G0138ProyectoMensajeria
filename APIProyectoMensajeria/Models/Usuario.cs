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
            UsuariosClases = new HashSet<UsuariosClase>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string NoControl { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Mensaje> MensajeIdEmisorNavigations { get; set; }
        public virtual ICollection<Mensaje> MensajeIdRemitenteNavigations { get; set; }
        public virtual ICollection<UsuariosClase> UsuariosClases { get; set; }
    }
}
