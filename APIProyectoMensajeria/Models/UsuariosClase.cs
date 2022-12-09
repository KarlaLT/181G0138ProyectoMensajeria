using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIProyectoMensajeria.Models
{
    public partial class UsuariosClase
    {
        public int Id { get; set; }
        public int? IdEstudiante { get; set; }
        public int? IdClase { get; set; }
        [JsonIgnore]

        public virtual Clase? IdClaseNavigation { get; set; }
        [JsonIgnore]

        public virtual Usuario? IdEstudianteNavigation { get; set; }
    }
}
