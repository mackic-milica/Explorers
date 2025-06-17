using SredaZadatak.Models;
using SredaZadatak.Models.DTO;
using System.Collections.Generic;

namespace SredaZadatak.Interfaces
{
    public interface IProjectRepository
    {
        List<Project> GetAll();
        Project GetById(int id);
        List<ProjectFilterReportDTO> GetProjectReport(int limit);
        List<ProjectFilterStateDTO> GetProjectState();
        List<Project> GetByName(string name);
    }
}


/*c) GET api/izvestaj?granica={vrednost} - preuzimanje svih Projekata sa njihovim
nazivom, brojem godina koliko je planirano da Projekat traje i prosečnom starošću svih
Istraživača na tom Projektu, pri čemu je planirani broj godina trajanja Projekta veći od
vrednosti granica, a sortiranih prema nazivu Projekta rastuće;

d) GET api/stanje – preuzimanje svih Projekata gde za svaki Projekat postoji podatak koji
mu je naziv, broj Istraživača koji rade na tom Projektu, najveća zarada nekog Istraživača
na tom Projektu i ukupna zarada za sve Istraživače na tom Projektu, sortirano po ukupnoj
zaradi opadajuće;

e) GET api/projekti/nadji?ime={vrednost} - preuzimanje svih Projekata čiji naziv je jednak
prosleđenoj vrednosti ime, sortirano prema godini početka rastuće, a u slučaju da su dva
Projekta počela iste godine, sortirati ih tada po godini završetka opadajuće*/