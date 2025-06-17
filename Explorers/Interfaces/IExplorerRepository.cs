using SredaZadatak.Models;
using SredaZadatak.Models.DTO;
using System.Collections.Generic;

namespace SredaZadatak.Interfaces
{
    public interface IExplorerRepository
    {
        List<Explorer> GetAll();
        Explorer GetById(int id);
        List<Explorer> GetByName(string name);
        void Create(Explorer explorer);
        void Delete(Explorer explorer);
        void Update(Explorer explorer);
        List<Explorer> GetBySalary(ExplorerSalaryFilterDTO filter);
    }
}
