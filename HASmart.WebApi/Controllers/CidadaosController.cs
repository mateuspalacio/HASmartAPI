using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Core.Exceptions;
using HASmart.Core.Repositories;
using HASmart.Core.Services;
using HASmart.Infrastructure.EFDataAccess;
using HASmart.Infrastructure.EFDataAccess.Repositories;
using HASmart.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using System;

namespace HASmart.WebApi.Controllers {
    [Route("hasmart/api/[controller]")]
    [ApiController]
    public class CidadaosController : ControllerBase {
        private readonly CidadaoService service;

        public CidadaosController(CidadaoService service) {
            this.service = service;
        }
        [HttpGet("health")]
        public async Task<ActionResult<string>> Health()
        {
            return "Ok";
        }

        /// <summary>
        /// Consulta os dados de um cidadão por meio do CPF ou RG do cidadão.
        /// </summary>
        /// <param name="cpf">do cidadão.</param>
        /// <param name="rg">do cidadão.</param>
        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles="admin")]
        public async Task<ActionResult<IEnumerable<Cidadao>>> GetCidadaos(string cpf, string rg) {
            bool hasCpf = !string.IsNullOrEmpty(cpf);
            bool hasRg = !string.IsNullOrEmpty(rg);
            try {
                if (hasCpf && hasRg) {
                    return this.HandleError("Query", "Informe somente o CPF ou somente o RG para buscar o cidadão");
                } else if (hasCpf) {
                    return new[] { await service.BuscarViaCpf(cpf) };
                } else if (hasRg) {
                    return new[] { await service.BuscarViaRg(rg) };
                } else {
                    return this.HandleError("Query", "Informe somente o CPF ou somente o RG para buscar o cidadão");
                    //return (await service.BuscarTodos(0, 100)).ToList();
                    //return await GetAllCidadaos();
                }
            } catch (EntityNotFoundException e) {
                return this.HandleError(e);
            } catch (EntityValidationException e) {
                return this.HandleError(e);
            }
        }
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Cidadao>>> GetAllCidadaos(){
            return (await service.BuscarTodos(0, 100)).ToList();
        }

        /// <summary>
        /// Consulta os dados de um cidadão por meio do ID.
        /// </summary>
        /// <param name="id">do cidadão.</param>
        // GET: api/Cidadaos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cidadao>> GetCidadao(Guid id) {
            try {
                return await service.BuscarViaId(id);
            } catch(EntityNotFoundException e) {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Retorna todos os cidadãos que tem um nome.
        /// </summary>
        /// <param nome="nome">do cidadão.</param>
        // GET: api/Cidadaos/nome
        [HttpGet("nome/{nome}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Cidadao>>> GetCidadaoByName(string nome)
        {
            return (await service.BuscarCidadaosPorNome(nome)).ToList();
        }

        /// <summary>
        /// Retorna todos os cidadãos que tem um nome anônimo.
        /// </summary>
        /// <param nome="nome"> anônimo do cidadão.</param>
        // GET: api/Cidadaos/nome
        [HttpGet("nome/{nome}/anonimo")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Cidadao>>> GetCidadaoByNameAnonimo(string nome)
        {
            return (await service.BuscarCidadaosPorNomeAnonimo(nome)).ToList();
        }


        /// <summary>
        /// Cadastra um cidadão.
        /// </summary>
        // POST: api/Cidadaos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<Cidadao>> PostCidadao([FromBody] CidadaoPostDTO dto) {
            try {
                Cidadao c = await this.service.CadastrarCidadao(dto);
                return CreatedAtAction("GetCidadao", new { id = c.Id }, c);
            } catch (EntityValidationException e) {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Cadastra um cidadão,se ele ja não exitir, e faz uma medição.
        /// </summary>
        // POST: api/Cidadaos/Registro
        [HttpPost("Registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<ActionResult<Cidadao>> PostRegistro([FromBody] RegistroPostDTO dto) {
            try {
                Cidadao c = await this.service.CadastrarCidadaoEMedicao(dto);
                return CreatedAtAction("GetCidadao", new { id = c.Id }, c);
            } catch (EntityValidationException e) {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Atualiza os dados do cidadão por meio do ID.
        /// </summary>
        /// <param name="id">do cidadão.</param>
        /// <param name="dto"></param>
        // PUT: api/Cidadaos/5
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutCidadao(Guid id, CidadaoPutDTO dto) {
            try {
                Cidadao cidadao = await service.AtualizarCidadao(id, dto);
                return this.Ok(cidadao);
            } catch (EntityValidationException e) {
                return this.HandleError(e);
            } catch (EntityNotFoundException e) {
                return this.HandleError(e);
            } catch (EntityConcurrencyException e) {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Deleta os dados do cidadão por meio do ID.
        /// </summary>
        /// <param name="id">do cidadão.</param>
        // DELETE: api/Cidadaos/5
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCidadao(Guid id)
        {
            try
            {
                Cidadao cidadao = await service.ApagarCidadao(id);
                return this.Ok(cidadao);
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
            catch (EntityNotFoundException e)
            {
                return this.HandleError(e);
            }
            catch (EntityConcurrencyException e)
            {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Atualiza os dados do cidadão por meio do ID.
        /// </summary>
        /// <param name="id">do cidadão.</param>
        /// <param name="dto"></param>
        // POST: api/Cidadaos/5
        [HttpPost("{id}/anonimizar")]
        [AllowAnonymous]
        public async Task<IActionResult> PostCidadaoAnonimo(Guid id)
        {
            try
            {
                Cidadao cidadao = await service.AnonimizarNome(id);
                return this.Ok(cidadao);
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
            catch (EntityNotFoundException e)
            {
                return this.HandleError(e);
            }
            catch (EntityConcurrencyException e)
            {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Receber CSV, Cadastra cidadãos e adicionar medições
        /// </summary>
        // POST: api/Cidadaos/CSV
        [HttpPost("CSV")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<IEnumerable<Cidadao>>> PostCSV([FromForm] FilePostDTO file)
        {
            try
            {
                IEnumerable<Cidadao> c = await service.RegistroComArquivo(file.File);
                return Ok(c);
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
        }

        /// <summary>
        /// Receber CSV, Cadastra cidadãos e adicionar medições
        /// </summary>
        // POST: api/Cidadaos/CSV
        [HttpGet("medicoes/total")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostCSV([FromQuery] Guid id)
        {
            try
            {
                var res = await service.TotalMedicoes(id);
                return Ok(res);
            }
            catch (EntityValidationException e)
            {
                return this.HandleError(e);
            }
        }
    }
}
