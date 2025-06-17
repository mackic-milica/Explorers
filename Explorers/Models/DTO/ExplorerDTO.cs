using System;

namespace SredaZadatak.Models.DTO
{
    public class ExplorerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        public decimal Salary { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ExplorerDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   LastName == dTO.LastName &&
                   BirthYear == dTO.BirthYear &&
                   Salary == dTO.Salary &&
                   ProjectId == dTO.ProjectId &&
                   ProjectName == dTO.ProjectName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, LastName, BirthYear, Salary, ProjectId, ProjectName);
        }
    }
}
