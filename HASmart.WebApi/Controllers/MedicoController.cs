using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Core.Exceptions;
using HASmart.Core.Services;
using HASmart.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using System;

namespace HASmart.WebApi.Controllers
{
    [Route("hasmart/api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly MedicoService service;
        public MedicoController(MedicoService service) {
            this.service = service;
        }

        /// <summary>
        /// Consulta os dados de um medico por meio do CRM do medico.
        /// </summary>
        /// <param name="crm">do medico.</param>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Medico>> GetMedico(string crm) {
            try {
                return await service.BuscarViaCrm(crm);
            } catch(EntityNotFoundException e) {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Cadastra um medico.
        /// </summary>
        // POST: api/Medico
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<Medico>> PostMedico([FromBody] MedicoPostDTO dto) {
            try {
                Medico m = await this.service.CadastrarMedico(dto);
                return CreatedAtAction("GetMedico", new { id = m.Id }, m);
            } catch (EntityValidationException e) {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Adiciona um ou mais cidadaos no medico.
        /// </summary>
        // POST: api/Medico
        /// <param name="id">do medico.</param>
        [HttpPost("{id}/Cidadaos")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<Medico>> PostCidadaos(Guid id,[FromBody] string[] cpfs) {
            try {
                Medico m = await this.service.AdicionarCidadaos(id,cpfs);
                return CreatedAtAction("GetMedico", new { id = m.Id }, m);
            } catch (EntityValidationException e) {
                return this.HandleError(e);
            }
        }

        [HttpPost("operador/")]
        [AllowAnonymous]
        public async Task<ActionResult<Medico>> VerifyOperador(Medico operador)
        {
            try
            {
                return await service.BuscarOperador(operador);
            }
            catch (EntityNotFoundException e)
            {
                return this.HandleError(e);
            }
        }
    }
}