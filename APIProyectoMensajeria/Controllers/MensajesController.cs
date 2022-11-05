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
                    var messages = reposMensajes.GetAll().Where(x=>x.IdRemitente==idRemitente);

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
            catch(Exception ex)
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

        //ENVIAR MENSAJE
        [HttpPost]
        public IActionResult Post(Mensaje mssage)
        {
            try
            {
                //if (idMessage != 0)
                //{
                //    var message = reposMensajes.GetById(idMessage);

                //    if (message != null)
                //    {
                        return Ok(message);
                //    }
                //    else
                //    {
                //        return NotFound("No se encontró el mensaje.");
                //    }
                //}
                //else
                //{
                //    return BadRequest("Solicitud incorrecta.");
                //}
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //EDITAR MENSAJE
        [HttpPut]
        public IActionResult Put(Mensaje mssage)
        {
            try
            {
                //if (idMessage != 0)
                //{
                //    var message = reposMensajes.GetById(idMessage);

                //    if (message != null)
                //    {
                        return Ok(message);
                //    }
                //    else
                //    {
                //        return NotFound("No se encontró el mensaje.");
                //    }
                //}
                //else
                //{
                //    return BadRequest("Solicitud incorrecta.");
                //}
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
