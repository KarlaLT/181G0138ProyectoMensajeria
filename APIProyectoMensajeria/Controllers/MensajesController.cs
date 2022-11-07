using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIProyectoMensajeria.Models;
using APIProyectoMensajeria.Repositories;

namespace APIProyectoMensajeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajesController : ControllerBase
    {
        static mensajeriatecnmContext Context = new();
        static Repositories.Repository<Mensaje> reposMensajes = new(Context);

        //TRAER LOS MENSAJES QUE UN USUARIO RECIBE (POR ID REMITENTE)
        [HttpGet("receive/{idRemitente}")]
        public IActionResult GetByRemitente(int idRemitente)
        {
            try
            {
                if (idRemitente != 0)
                {
                    var messages = reposMensajes.GetAll().Where(x => x.IdRemitente == idRemitente);

                    if (messages != null)
                    {
                        return Ok(messages);
                    }
                    else
                    {
                        return NotFound("No se encontraron mensajes para el usuario.");
                    }
                }
                else
                {
                    return BadRequest("Solicitud incorrecta.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //TRAER TODOS LOS MENSAJES QUE UN USUARIO ENVIÓ (POR ID EMISOR)
        [HttpGet("sent/{idEmisor}")]
        public IActionResult GetByEmisor(int idEmisor)
        {
            try
            {
                if (idEmisor != 0)
                {
                    var messages = reposMensajes.GetAll().Where(x => x.IdEmisor == idEmisor);

                    if (messages != null)
                    {
                        return Ok(messages);
                    }
                    else
                    {
                        return NotFound("No se encontraron mensajes enviados por el usuario.");
                    }
                }
                else
                {
                    return BadRequest("Solicitud incorrecta.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //TRAER MENSAJE POR ID
        [HttpGet("{idMessage}")]
        public IActionResult GetByID(int idMessage)
        {
            try
            {
                if (idMessage != 0)
                {
                    var message = reposMensajes.GetById(idMessage);

                    if (message != null)
                    {
                        return Ok(message);
                    }
                    else
                    {
                        return NotFound("No se encontró el mensaje.");
                    }
                }
                else
                {
                    return BadRequest("Solicitud incorrecta.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //ENVIAR MENSAJE A 1 USUARIO
        [HttpPost]
        public IActionResult Post(Mensaje message)
        {
            try
            {
                if (message != null)
                {
                    //VERIFICAR QUE SEA DOCENTE O ADMINISTRATIVO
                    var emisor = Context.Set<Usuario>().Where(x => x.Id == message.IdEmisor).FirstOrDefault();

                    if (emisor.Rol != "Estudiante")
                    {
                        if (string.IsNullOrWhiteSpace(message.Mensaje1))
                        {
                            ModelState.AddModelError("", "Ingrese el mensaje que quiere enviar.");
                        }
                        if (message.IdRemitente <= 0)
                        {
                            ModelState.AddModelError("", "Selecione destinatarios válidos.");
                        }

                        if (ModelState.IsValid)
                        {
                            message.Fecha = DateTime.Now;
                            reposMensajes.Insert(message);
                            return Ok();
                        }
                        else
                        {
                            return BadRequest(ModelState);
                        }
                    }
                    else
                    {
                        return BadRequest("Sólo usuarios de tipo administrativo y docentes pueden enviar mensajes.");
                    }
                }
                else
                {
                    return BadRequest("No se puede enviar un mensaje sin datos.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //ENVIAR MENSAJE A +1 USUARIO
        [HttpPost("toUsers")]
        public IActionResult Send(MensajeViewModel messageVM)
        {
            try
            {
                if (messageVM.Mensaje != null)
                {
                    var emisor = Context.Set<Usuario>().Where(x => x.Id == messageVM.Mensaje.IdEmisor).FirstOrDefault();

                    if (emisor.Rol != "Estudiante")
                    {
                        if (string.IsNullOrWhiteSpace(messageVM.Mensaje.Mensaje1))
                        {
                            ModelState.AddModelError("", "Ingrese el mensaje que quiere enviar.");
                        }
                        if (messageVM.Usuarios == null)
                        {
                            ModelState.AddModelError("", "Selecione destinatarios válidos.");
                        }


                        if (ModelState.IsValid)
                        {
                            foreach (var user in messageVM.Usuarios)
                            {
                                Mensaje message = new Mensaje()
                                {
                                    Fecha = DateTime.Now,
                                    IdEmisor = emisor.Id,
                                    IdRemitente = user.Id,
                                    Mensaje1=messageVM.Mensaje.Mensaje1
                                };
                                reposMensajes.Insert(message);
                            }
                            return Ok();
                        }
                        else
                        {
                            return BadRequest(ModelState);
                        }
                    }
                    else
                    {
                        return BadRequest("Sólo usuarios de tipo administrativo y docentes pueden enviar mensajes.");
                    }
                }
                else
                {
                    return BadRequest("No se puede enviar un mensaje sin datos.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //EDITAR MENSAJE
        [HttpPut]
        public IActionResult Put(Mensaje message)
        {
            try
            {
                if (message != null)
                {
                    var msj = reposMensajes.GetById(message.Id);

                    if (msj != null)
                    {
                        if (string.IsNullOrWhiteSpace(message.Mensaje1))
                        {
                            ModelState.AddModelError("", "Ingrese el mensaje que quiere enviar.");
                        }
                        if (message.IdRemitente <= 0)
                        {
                            ModelState.AddModelError("", "Selecione destinatarios válidos.");
                        }

                        if (ModelState.IsValid)
                        {
                            msj.Mensaje1 = message.Mensaje1;
                            msj.IdRemitente = message.IdRemitente;
                            msj.Fecha = DateTime.Now;
                            reposMensajes.Update(msj);
                            return Ok();
                        }
                        else
                        {
                            return BadRequest(ModelState);
                        }
                    }
                    else
                    {
                        return BadRequest("Mensaje no encontrado.");
                    }

                }
                else
                {
                    return BadRequest("No se puede actualizar un mensaje sin datos.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //ELIMINAR MENSAJE
        [HttpDelete("{idMessage}")]
        public IActionResult Delete(int idMessage)
        {
            try
            {
                if (idMessage != 0)
                {
                    var message = reposMensajes.GetById(idMessage);

                    if (message != null)
                    {
                        reposMensajes.Delete(message);
                        return Ok();
                    }
                    else
                    {
                        return NotFound("No se encontró el mensaje.");
                    }
                }
                else
                {
                    return BadRequest("Solicitud incorrecta.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
