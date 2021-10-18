using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HASmart.Core.Architecture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HASmart.Core.Entities;
using HASmart.Core.Entities.DTOs;
using HASmart.Core.Exceptions;
using HASmart.Core.Repositories;
using HASmart.Core.Services;
using HASmart.Infrastructure.EFDataAccess;
using HASmart.Infrastructure.EFDataAccess.Repositories;
using HASmart.WebApi.Extensions;

namespace HASmart.WebApi.Controllers {
    [ApiController]
    [Route("/hasmart/api/[controller]")]
    public class FarmaciaController : ControllerBase {

        private readonly FarmaciaService service;
        //private readonly UserManager<IdentityUser> _userManager;

        public FarmaciaController(FarmaciaService service) {
            this.service = service;
        }

        /// <summary>
        /// Cadastra as medições de um cidadão por meio do ID.
        /// </summary>
        /// <param name="cidadaoId">do cidadão.</param>
        /// <param name="dto"></param>
        [HttpPost("medicoes/")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Cidadao>> PostMedicao(Guid cidadaoId, [FromBody] MedicaoPostDTO dto) {


            try {
                Cidadao c = await this.service.RegistrarMedicao(cidadaoId, dto);
                return CreatedAtAction("GetCidadao", "Cidadaos", new { id = c.Id }, c);
            } catch (EntityValidationException e) {
                return this.HandleError(e);
            } catch (EntityNotFoundException e) {
                return this.HandleError(e);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Farmacia>> PostFarmacia([FromBody] FarmaciaPostDTO dto) {
            var user = new IdentityUser { UserName = "Farmacias1" };
            //var result = await _userManager.CreateAsync(user, "novaFarmacia1");

            try {
                Farmacia c = await this.service.CadastrarFarmacia(dto);
                return CreatedAtAction("GetFarmacia", new { id = c.Id }, c);
            } catch (EntityValidationException e) {
                return this.HandleError(e);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Farmacia>> GetFarmacia(Guid id) {
            try {
                return await service.BuscarViaId(id);
            } catch (EntityNotFoundException e) {
                return this.HandleError(e);
            }
        }

        

        /*
        /// <summary>
        /// Dispensação de medicamentos para um cidadão por meio do ID.
        /// </summary>
        /// <param name="cidadaoId">do cidadão.</param>
        /// <param name="dto"></param>
        [HttpPost("dispencacoes/")]
        public async Task<ActionResult<Medicao>> PostDispencacao(long cidadaoId, [FromBody] DispencacaoPostDTO dto) {
            if (cidadaoId <= 0) {
                return this.HandleError("CidadaoId", "O ID enviado pela URL nao e valido.");
            }

            try {
                Cidadao c = await this.service.RegistrarDispencacao(cidadaoId, dto);
                return CreatedAtAction("GetCidadao", "Cidadaos", new { id = c.Id }, c);
            } catch (EntityValidationException e) {
                return this.HandleError(e);
            } catch (EntityNotFoundException e) {
                return this.HandleError(e);
            } 
        }*/
    }
}
