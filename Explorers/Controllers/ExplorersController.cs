using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SredaZadatak.Interfaces;
using SredaZadatak.Models;
using SredaZadatak.Models.DTO;
using SredaZadatak.Repository;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SredaZadatak.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExplorersController : ControllerBase
    {
        private readonly IExplorerRepository _explorerRepository;
        private readonly IMapper _mapper;

        public ExplorersController(IExplorerRepository explorerRepository, IMapper mapper)
        {
            _explorerRepository = explorerRepository;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _explorerRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<ExplorerDTO>>(result));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var result = _explorerRepository.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ExplorerDTO>(result));
        }

        [Route("/api/istrazivaci")]
        [HttpGet]
        public IActionResult GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            var result = _explorerRepository.GetByName(name);
            return Ok(_mapper.Map<ExplorerDTO>(result));
        }

        [HttpPost]
        public IActionResult Create(Explorer explorer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _explorerRepository.Create(explorer);
            
            return CreatedAtAction("GetById", new { id = explorer.Id }, _mapper.Map<ExplorerDTO>(explorer));

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Explorer explorer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != explorer.Id)
            {
                return BadRequest("Invalid Id!");
            }

            try
            {
                _explorerRepository.Update(explorer);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(_mapper.Map<ExplorerDTO>(explorer));
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var explorer = _explorerRepository.GetById(id);
            if (explorer == null)
            {
                return BadRequest();
            }
            _explorerRepository.Delete(explorer);
            return NoContent();
        }


        [Route("/api/pretraga/")]
        [HttpPost]
        public IActionResult GetBySalary(ExplorerSalaryFilterDTO filter)
        {
            if (filter == null)
            {
                filter = new ExplorerSalaryFilterDTO()
                {
                    Max = decimal.MaxValue,
                    Min = decimal.MinValue
                };
            }
            if (filter.Min < 0 || filter.Max < filter.Min || filter.Max < 0)
            {
                return BadRequest();
            }
            var explorers = _explorerRepository.GetBySalary(filter);
            return Ok(_mapper.Map<IEnumerable<ExplorerDTO>>(explorers));

        }

    }

}



