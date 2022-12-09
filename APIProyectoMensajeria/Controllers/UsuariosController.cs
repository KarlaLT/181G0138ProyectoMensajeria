using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIProyectoMensajeria.Models;
using APIProyectoMensajeria.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace APIProyectoMensajeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        static itesrcne_mensajeriakarlaContext Context = new();
        static Repositories.Repository<Usuario> reposUsuarios = new(Context);

        //Para traernos lo que tenemos en la configuracion del Jwt
        public IConfiguration Configuration { get; }

        public UsuariosController(IConfiguration config)
        {
            Configuration = config;
        }

        //LOGIN
        [HttpPost("login")]
        public IActionResult GetUserLogin(LoginViewModel usuario)
        {
            if (string.IsNullOrEmpty(usuario.Correo))
            {
                ModelState.AddModelError("", "Ingrese su usuario.");
            }
            if (string.IsNullOrEmpty(usuario.Password))
            {
                ModelState.AddModelError("", "Ingrese su contraseña.");
            }

            if (ModelState.IsValid)
            {
                var user = reposUsuarios.GetAll().Where(x => x.Correo == usuario.Correo && x.Password == usuario.Password).FirstOrDefault();

                if (user != null)
                {
                    //Crear la identidad del usuario.
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.Nombre));
                    claims.Add(new Claim(ClaimTypes.Role, user.Rol));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"));

                    //Crear un token.
                    var handler = new JwtSecurityTokenHandler();
                    var descriptor = new SecurityTokenDescriptor();

                    descriptor.Issuer = Configuration["Jwt:Issuer"];
                    descriptor.Audience = Configuration["Jwt:Audience"];

                    //La identidad a quien se emite el token.
                    descriptor.Subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                    //En cuanto tiempo se expira el token que mandamos
                    descriptor.Expires = DateTime.UtcNow.AddDays(10);
                    descriptor.IssuedAt = DateTime.UtcNow;
                    descriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])), SecurityAlgorithms.HmacSha256);

                    var token = handler.CreateToken(descriptor);
                    var tokenSerializado = handler.WriteToken(token);

                    return Ok(tokenSerializado);
                }
                else
                {
                    return Unauthorized("Usuario o contraseña incorrectos.");
                }
            }
            else
            {
                return BadRequest("Solicitud incorrecta");
            }
        }

        //TRAER TODOS LOS USUARIOS REGISTRADOS
        //[Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(reposUsuarios.GetAll());
        }

        //MENSAJE A UN SÓLO USUARIO
        //[Authorize]
        [HttpGet("{idUsuario}")]
        public IActionResult Get(int idUsuario)
        {
            try
            {
                if (idUsuario != 0)
                {
                    var user = reposUsuarios.GetById(idUsuario);
                    if (user != null)
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

        //MENSAJE POR GRUPO
        //[Authorize]
        [HttpGet("group/{idGrupo}")]
        public IActionResult GetUsersByGroup(int idGrupo)
        {
            try
            {
                if (idGrupo != 0)
                {
                    List<Usuario> usuarios = new List<Usuario>();

                    var usersByGroup = Context.Set<UsuariosClase>().Include(x => x.IdClaseNavigation).
                        Where(x => x.IdClaseNavigation.IdGrupo == idGrupo).ToList();

                    if (usersByGroup != null)
                    {
                        foreach (var user in usersByGroup)
                        {
                            var usuario = reposUsuarios.GetById(user.IdEstudiante);

                            if (usuario != null)
                            {
                                if (!usuarios.Any(x => x.Id == usuario.Id))
                                    usuarios.Add(usuario);
                            }
                        }

                        if (usuarios.Count > 0)
                        {
                            return Ok(usuarios);
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

        //MENSAJE POR CLASE
        //[Authorize]
        [HttpGet("class/{idClase}")]
        public IActionResult GetUsersByClass(int idClase)
        {
            try
            {
                if (idClase != 0)
                {
                    List<Usuario> usuarios = new List<Usuario>();

                    var usersByClass = Context.Set<UsuariosClase>().Where(x => x.IdClase == idClase).ToList();

                    if (usersByClass != null)
                    {
                        foreach (var user in usersByClass)
                        {
                            var usuario = reposUsuarios.GetById(user.IdEstudiante);

                            if (usuario != null)
                            {
                                if (!usuarios.Any(x => x.Id == usuario.Id))
                                    usuarios.Add(usuario);
                            }
                        }

                        if (usuarios.Count > 0)
                        {
                            return Ok(usuarios);
                        }
                        else
                        {
                            return NotFound("No se encontraron usuarios pertenecientes a la clase.");
                        }
                    }
                    else
                    {
                        return NotFound("No se encontraron usuarios pertenecientes a la clase.");
                    }
                }
                else
                {
                    return NotFound("No se encontraron usuarios pertenecientes a la clase.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //MENSAJE POR CARRERA
        //[Authorize]
        [HttpGet("career/{idCarrera}")]
        public IActionResult GetUsersByCareer(int idCarrera)
        {
            try
            {
                if (idCarrera != 0)
                {
                    List<Usuario> usuarios = new List<Usuario>();

                    var usersByCareer = Context.Set<UsuariosClase>().Include(x => x.IdClaseNavigation).ThenInclude(x => x.IdGrupoNavigation)
                        .Where(x => x.IdClaseNavigation.IdGrupoNavigation.IdCarrera == idCarrera).ToList();

                    if (usersByCareer != null)
                    {
                        foreach (var user in usersByCareer)
                        {
                            var usuario = reposUsuarios.GetById(user.IdEstudiante);

                            if (usuario != null)
                            {
                                if (!usuarios.Any(x => x.Id == usuario.Id))
                                    usuarios.Add(usuario);
                            }
                        }

                        if (usuarios.Count > 0)
                        {
                            return Ok(usuarios);
                        }
                        else
                        {
                            return NotFound("No se encontraron usuarios pertenecientes a la carrera.");
                        }
                    }
                    else
                    {
                        return NotFound("No se encontraron usuarios pertenecientes a la carrera.");
                    }
                }
                else
                {
                    return NotFound("No se encontraron usuarios pertenecientes a la carrera.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GRUPOS
        [HttpGet("grupos")]
        public IActionResult GetGrupos()
        {
            var grupos = Context.Set<Grupo>().ToList();
            return Ok(grupos);
        }

        //CLASES
        [HttpGet("clases")]
        public IActionResult GetClases()
        {
            var clases = Context.Set<Clase>().ToList();
            return Ok(clases);
        }

        //CARRERAS
        [HttpGet("carreras")]
        public IActionResult GetCarreras()
        {
            var carreras = Context.Set<Carrera>().ToList();
            return Ok(carreras);
        }
    }
}