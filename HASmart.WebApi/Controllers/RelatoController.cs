using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Core.Exceptions;
using HASmart.Core.Services;
using HASmart.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HASmart.WebApi.Controllers
{
    [Route("hasmart/api/[controller]")]
    [ApiController]
    public class RelatoController : Controller
    {
        private readonly RelatoService _service;

        public RelatoController(RelatoService relatoService)
        {
            _service = relatoService;
        }

        /// <summary>
        /// Cadastra um relato de um cidadão, recebendo relato e cidadao id.
        /// </summary>
        // POST: api/Relato/{id}
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<Relatorio>> PostRelato(Guid id, [FromBody] RelatorioPostDTO dto)
        {
            try
            {
                Relatorio r = await _service.CadastrarRelato(id, dto);
                return r;
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Retorna um relato de um cidadão, recebendo relato id e cidadao id.
        /// </summary>
        // GET: api/Relato/{cidadaoId}/{id}
        [HttpGet("{cidadaoId}/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<Relatorio>> GetRelato(Guid id, Guid cidadaoId)
        {
            try
            {
                Relatorio r = await _service.LerRelato(id, cidadaoId);
                return r;
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Retorna um relato de um cidadão, recebendo relato id e cidadao id.
        /// </summary>
        // GET: api/Relato/All
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<List<Relatorio>>> GetAllRelatos()
        {
            try
            {
                var r = await _service.LerAllRelatos();
                return r;
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Retorna os relatos de um cidadão, recebe cidadao id.
        /// </summary>
        // GET: api/Relato/{cidadaoId}
        [HttpGet("{cidadaoId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<List<Relatorio>>> GetRelatos(Guid cidadaoId)
        {
            try
            {
                var r = await _service.LerRelatos(cidadaoId);
                return r;
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
        }
        /// <summary>
        /// Retorna os relatos de um cidadão, recebe cidadao id.
        /// </summary>
        // GET: api/Relato/{nomeAnonimo}/anonimo
        [HttpGet("{nomeAnonimo}/anonimo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<List<Relatorio>>> GetRelatosAnonimo(string nomeAnonimo)
        {
            try
            {
                var r = await _service.LerRelatosParaAnonimo(nomeAnonimo);
                return r;
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Deleta um relato de um cidadão, recebendo id e cidadao id.
        /// </summary>
        // DELETE: api/Relato/{cidadaoId}
        [HttpDelete("{cidadaoId}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<Relatorio>> DeleteRelato(Guid id, Guid cidadaoId)
        {
            try
            {
                var r = await _service.ApagarRelato(id, cidadaoId);
                return r;
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Deleta um relato de um cidadão, recebendo id e cidadao id.
        /// </summary>
        // PUT: api/Relato/{cidadaoId}
        [HttpPut("{cidadaoId}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<Relatorio>> UpdateRelato(Guid id, Guid cidadaoId, RelatorioPostDTO dto)
        {
            try
            {
                var r = await _service.AtualizarRelato(id, cidadaoId, dto);
                return r;
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
        }
    }
}
