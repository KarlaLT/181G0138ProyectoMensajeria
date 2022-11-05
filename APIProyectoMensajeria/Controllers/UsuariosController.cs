using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIProyectoMensajeria.Models;
using APIProyectoMensajeria.Repositories;


namespace APIProyectoMensajeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        static mensajeriatecnmContext Context = new();
        static Repositories.Repository<Usuario> reposUsuarios = new(Context);

        //TRAER TODOS LOS USUARIOS REGISTRADOS
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(reposUsuarios.GetAll());
        }

        [HttpGet("{idUsuario}")] 
        public IActionResult Get(int idUsuario)
        {
            try
            {
                if (idUsuario != 0)
                {
                    var user = reposUsuarios.GetById(idUsuario);
                    if(user != null)
                    {
                        return Ok(user);
                    }
                    else
                    {
                        return NotFound("Usuario no encontrado.");
                    }
                }
                else
                {
                    return BadRequest("Usuario no encontrado.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("group/{idGrupo}")]
        public IActionResult GetUsersByGroup(int idGrupo)
        {
            try
            {
                if (idGrupo != 0)
                {
                    List<Usuario> usuariosGrupo = new List<Usuario>();
                    Repository<Clase> reposClases = new Repository<Clase>(Context);
                    Repository<DatosClase> reposDatosClase = new Repository<DatosClase>(Context);

                    // var usersId = Context.Set<DatosClase>().Where(x => x.IdClaseNavigation.IdGrupo == idGrupo);
                    var classes = reposClases.GetAll().Where(x=>x.IdGrupo==idGrupo).ToList();
                    var datosclase = reposDatosClase.GetAll().Where(x => x.IdClase == 1).ToList();

                    if (datosclase != null)
                    {
                        foreach (var user in datosclase)
                        {
                            var usuario = reposUsuarios.GetById(user.IdEstudiante);

                            if (usuario != null)
                            {
                                usuariosGrupo.Add(usuario);
                            }
                        }

                        if (usuariosGrupo.Count > 0)
                        {
                            return Ok(usuariosGrupo);
                        }
                        else
                        {
                            return NotFound("No se encontraron usuarios pertenecientes al grupo.");
                        }
                    }
                    else
                    {
                        return NotFound("No se encontraron usuarios pertenecientes al grupo.");
                    }
                }
                else
                {
                    return NotFound("No se encontraron usuarios pertenecientes al grupo.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
