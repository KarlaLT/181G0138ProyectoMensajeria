using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonIgnore]

        public virtual ICollection<Mensaje> MensajeIdEmisorNavigations { get; set; }
        [JsonIgnore]

        public virtual ICollection<Mensaje> MensajeIdRemitenteNavigations { get; set; }
        [JsonIgnore]

        public virtual ICollection<UsuariosClase> UsuariosClases { get; set; }
    }
}
