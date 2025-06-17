using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SredaZadatak.Interfaces;
using SredaZadatak.Models.DTO;
using System.Collections;
using System.Collections.Generic;

namespace SredaZadatak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectRepository projectRepository, IMapper mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository; 
        }

        [HttpGet]
        public IActionResult GetAll() 
            {
            var result = _projectRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<ProjectDTO>>(result));
            }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
            {
            if(id < 0) 
                {
                return BadRequest();
                }
            var result = _projectRepository.GetById(id);
            if(result == null) 
                {
                return NotFound();
                }
            return Ok(_mapper.Map<ProjectDTO>(result));
            }

        [Route("/api/izvestaj")]
        [HttpGet]
        public IActionResult GetProjectReport(int limit) 
            {
            if(limit < 0) 
                {
                return BadRequest();
                }
            var result = _projectRepository.GetProjectReport(limit);
            return Ok(_mapper.Map<IEnumerable<ProjectFilterReportDTO>>(result));
            }


        [Route("/api/stanje")]
        [HttpGet]
        public IActionResult GetProjectState() 
            {
            var result = _projectRepository.GetProjectState();
            return Ok(_mapper.Map<IEnumerable<ProjectFilterStateDTO>>(result));
            }


            

        
    }
}
