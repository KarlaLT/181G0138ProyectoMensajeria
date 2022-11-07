﻿using System;
using System.Collections.Generic;

namespace APIProyectoMensajeria.Models
{
    public partial class Clase
    {
        public Clase()
        {
            UsuariosClases = new HashSet<UsuariosClase>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int? IdGrupo { get; set; }

        public virtual Grupo? IdGrupoNavigation { get; set; }
        public virtual ICollection<UsuariosClase> UsuariosClases { get; set; }
    }
}
