using SredaZadatak.Interfaces;
using SredaZadatak.Models;
using SredaZadatak.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SredaZadatak.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Project> GetAll()
        {
           return _context.Projects.OrderBy(x => x.Name).ToList();
        }

        public Project GetById(int id)
        {
            return _context.Projects.FirstOrDefault(x => x.Id == id);
        }
  
        public List<Project> GetByName(string name)
        {
            return _context.Projects.Where(x => x.Name == name).OrderBy(x => x.StartYear).ThenByDescending(x => x.EndYear).ToList();
        }
        /*GET api/izvestaj?granica={vrednost} - preuzimanje svih Projekata sa njihovim
            nazivom, brojem godina koliko je planirano da Projekat traje i prosečnom starošću svih
            Istraživača na tom Projektu, pri čemu je planirani broj godina trajanja Projekta veći od
            vrednosti granica, a sortiranih prema nazivu Projekta rastuće;*/
        public List<ProjectFilterReportDTO> GetProjectReport(int limit)
        {
           var result = _context.Projects.Where(x => x.EndYear - x.StartYear > limit)
                .Select(x => new ProjectFilterReportDTO() { 
                    Name = x.Name,
                    Duration = x.EndYear - x.StartYear,
                    AverageAge = (int)(x.Explorers.Any() ? x.Explorers.Average(x => DateTime.Now.Year - x.BirthYear) : 0)
                    }).OrderBy(x => x.Name).ToList();
            return result;
        }

        public List<ProjectFilterStateDTO> GetProjectState()
        {
         var result = _context.Projects.Select(x => new ProjectFilterStateDTO() { 
             Name = x.Name,
             NumberOfExplorers = x.Explorers.Count(),
             MaxSalary = x.Explorers.Max(x => x.Salary),
             TotalSalary = x.Explorers.Sum(x => x.Salary)
             }).OrderByDescending(x => x.TotalSalary).ToList();
            return result;

             
        }
    }
}
/*GET api/stanje – preuzimanje svih Projekata gde za svaki Projekat postoji podatak koji
mu je naziv, broj Istraživača koji rade na tom Projektu, najveća zarada nekog Istraživača
na tom Projektu i ukupna zarada za sve Istraživače na tom Projektu, sortirano po ukupnoj
zaradi opadajuće;*/