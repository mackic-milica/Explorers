using Microsoft.EntityFrameworkCore;
using SredaZadatak.Interfaces;
using SredaZadatak.Models;
using SredaZadatak.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SredaZadatak.Repository
{


    public class ExplorerRepository : IExplorerRepository
    {

        private readonly AppDbContext _context;

        public ExplorerRepository(AppDbContext context)
        {
            _context = context;

        }

        public void Create(Explorer explorer)
        {
            _context.Explorers.Add(explorer);
            _context.SaveChanges();
        }

        public void Delete(Explorer explorer)
        {
            _context.Explorers.Remove(explorer);
            _context.SaveChanges();
        }

        public List<Explorer> GetAll()
        {
            return _context.Explorers.Include(x => x.Project).OrderBy(x => x.LastName).ToList();
        }

        public Explorer GetById(int id)
        {
            return _context.Explorers.FirstOrDefault(x => x.Id == id);
        }

        public List<Explorer> GetByName(string name)
        {
            return _context.Explorers.Include(x => x.Project.Name)
                .Where(x => x.Name.Contains(name)
                || x.LastName.Contains(name)
                || x.Project.Name.Contains(name))
                .OrderByDescending(x => x.BirthYear).ToList();

        }

        public List<Explorer> GetBySalary(ExplorerSalaryFilterDTO filter)
        {
            return _context.Explorers.Include(x => x.Project).Where(x => x.Salary < filter.Max && x.Salary > filter.Min)
                .OrderByDescending(x => x.Salary).ToList();

        }

        public void Update(Explorer explorer)
        {

            _context.Entry(explorer).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
