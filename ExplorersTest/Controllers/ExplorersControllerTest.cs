using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SredaZadatak.Controllers;
using SredaZadatak.Interfaces;
using SredaZadatak.Models;
using SredaZadatak.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SredaZadatakTest.Controllers
{
    public class ExplorersControllerTest
    {

        [Fact]
        public void GetByID_ValidId_ReturnsOkObjectResult()
        {
            Explorer explorer = new Explorer()
            {
                Id = 1,
                Name = "Aleksandra",
                LastName = "Rakic",
                BirthYear = 1996,
                Salary = 45000,
                ProjectId = 1,
                Project = new Project() { Id = 1, Name = "IstrazivanjeTest", StartYear = 2010, EndYear = 2015 }
            };

            ExplorerDTO explorerDto = new ExplorerDTO()
            {
                Id = 1,
                Name = "Aleksandra",
                LastName = "Rakic",
                BirthYear = 1996,
                Salary = 45000,
                ProjectId = 1,
                ProjectName = "IstrazivanjeTest"
            };


            var mockRepository = new Mock<IExplorerRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(explorer);

            var mapConf = new MapperConfiguration(cfg => cfg.AddProfile(new ExplorerProfile()));
            IMapper mapper = new Mapper(mapConf);
            var controller = new ExplorersController(mockRepository.Object, mapper);

            var result = controller.GetById(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);

            // // Pristup 1
            // da bi ovo radilo potrebno je kreirati Equals i GetHashCode metode u DTO klasi -> otvoriti DTO klasu,
            // desni klik na klasu -> Quick actions and refactorings -> Generate Equals and GetHashCode
            ExplorerDTO dtoResult = (ExplorerDTO)result.Value;
            Assert.Equal(explorerDto, dtoResult);
        }


        [Fact]
        public void GetById_InvalidId_ReturnsBadRequest()
        {

            var mockRepo = new Mock<IExplorerRepository>();
            var controller = new ExplorersController(mockRepo.Object, null);
            var result = controller.GetById(-1) as BadRequestResult;
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        public void GetById_InvalidId_ReturnsNotFound()
        {

            var mockRepo = new Mock<IExplorerRepository>();
            mockRepo.Setup(x => x.GetById(5));
            var controller = new ExplorersController(mockRepo.Object, null);
            var result = controller.GetById(5) as NotFoundResult;
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);

        }

        [Theory]
        [InlineData(24, 1, "Invalid Id!")]
        [InlineData(1, 1, null)]
        public void PutExplorer_InvalidId_RetunsBadRequest(int routeId, int explorerId, string expectedMessage)
        {

            Explorer explorer = new Explorer()
            {
                Id = 1,
                Name = "Aleksandra",
                LastName = "Petrovic",
                BirthYear = 1994,
                Salary = 4500000,
                ProjectId = 1,
                Project = new Project() { Id = 1, Name = "Artist", StartYear = 2002, EndYear = 2005 }
            };

            var mockRepository = new Mock<IExplorerRepository>();

            var controller = new ExplorersController(mockRepository.Object, null);

            if (expectedMessage == null)
            {
                controller.ModelState.AddModelError("error", "error");
            }

            var actionResult = controller.Update(routeId, explorer);

            var badReqRes = actionResult as BadRequestObjectResult;

            if (expectedMessage != null)
            {
                Assert.Equal(expectedMessage, badReqRes.Value);
            }
            else
            {
                Assert.IsType<SerializableError>(badReqRes.Value);
            }


        }

        [Fact]

        public void Update_ValidId_ReturnsOkObjectResault()
        {
            Explorer explorer = new Explorer()
            {
                Id = 1,
                Name = "Aleksandra",
                LastName = "Rakic",
                BirthYear = 1996,
                Salary = 45000,
                ProjectId = 1,
                Project = new Project() { Id = 1, Name = "IstrazivanjeTest", StartYear = 2010, EndYear = 2015 }
            };

            var mockRepository = new Mock<IExplorerRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(explorer);
            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new ExplorerProfile()));
            IMapper mapper = new Mapper(mapConfig);

            var controller = new ExplorersController(mockRepository.Object, mapper);
            var actionResult = controller.Update(1, explorer) as OkObjectResult;

            Assert.NotNull(actionResult); //ne koristii u slucajevima kada metoda moze da vrati praznu listu ili prazan objekat znaci ne korisiti za OK
            Assert.NotNull(actionResult.Value);

            // Assert.NotNull(actionResult.Value); koristiti samo kada metoda vraca neki rezultat tj kada ima neku vrednost 

        }


        [Fact]
        public void DeleteExplorer_ValidId_ReturnsNotFound()
        {
            Explorer explorer = new Explorer()
            {
                Id = 1,
                Name = "Aleksandra",
                LastName = "Rakic",
                BirthYear = 1996,
                Salary = 45000,
                ProjectId = 1,
                Project = new Project() { Id = 1, Name = "IstrazivanjeTest", StartYear = 2010, EndYear = 2015 }
            };

            var mockRepo = new Mock<IExplorerRepository>();
            mockRepo.Setup(x => x.GetById(1)).Returns(explorer);

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new ExplorerProfile()));

            IMapper mapper = new Mapper(mapConfig);

            var contoller = new ExplorersController(mockRepo.Object, mapper);

            var actionResult = contoller.Delete(1) as NoContentResult;

            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        public void DeleteExplorer_InvalidId_ReturnsBadRequest()
        {
            var mockRepo = new Mock<IExplorerRepository>();
            var controller = new ExplorersController(mockRepo.Object, null);
            var result = controller.Delete(6) as BadRequestResult;
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetBySalary_ReturnsCollection()
        {
            List<ExplorerDTO> explorers = new List<ExplorerDTO>()
{
                new ExplorerDTO
                {
                    Id = 1,
                    Name = "Test",
                    LastName = "Testovic",
                    BirthYear = 2000,
                    ProjectId = 1,
                    ProjectName = "Test",
                    Salary = 45000
                },
                   new ExplorerDTO
                {
                    Id = 2,
                    Name = "StaGod",
                    LastName = "Gostovic",
                    BirthYear = 2004,
                    ProjectId = 2,
                    ProjectName = "Test2",
                    Salary = 45700
                }

                };


            var mockRepo = new Mock<IExplorerRepository>();

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new ExplorerProfile()));
            IMapper mapper = new Mapper(mapConfig);

            var controller = new ExplorersController(mockRepo.Object, mapper);

            var actionResult = controller.GetBySalary(new ExplorerSalaryFilterDTO() { Min = 100000, Max = 800000 }) as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            List<ExplorerDTO> listResults = (List<ExplorerDTO>)actionResult.Value;

            for (int i = 0; i < listResults.Count; i++)
            {
                Assert.Equal(listResults[i], explorers[i]);
            }

        }



        [Fact]
        public void PostExplorer_ValidObject_SetsLocationHeaders()
        {
            Explorer explorer = new Explorer()
            {
                Id = 1,
                Name = "Aleksandra",
                LastName = "Rakic",
                BirthYear = 1996,
                Salary = 45000,
                ProjectId = 1,
                Project = new Project() { Id = 1, Name = "IstrazivanjeTest", StartYear = 2010, EndYear = 2015 }
            };
            var mockRepo = new Mock<IExplorerRepository>();
            mockRepo.Setup(x => x.Create(explorer));

            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile(new ExplorerProfile()));
            IMapper mapper = new Mapper(mapConfig);

            var controller = new ExplorersController(mockRepo.Object, mapper);
            var result = controller.Create(explorer) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<CreatedAtActionResult>(result);

        }

        [Fact]
        public void PostExplorer_InvalidObject_ReturnsBadRequest()
        {

            Explorer explorer = new Explorer()
            {
                Id = 1,
                Name = "Aleksandra",
                LastName = "Rakic",
                BirthYear = 0,
                Salary = 45000,
                ProjectId = 1,
                Project = new Project() { Id = 1, Name = "IstrazivanjeTest", StartYear = 2010, EndYear = 2015 }
            };
            var mockRepo = new Mock<IExplorerRepository>();
            mockRepo.Setup(x => x.Create(explorer));

            var controller = new ExplorersController(mockRepo.Object, null);

            controller.ModelState.AddModelError(string.Empty, string.Empty);

            var result = controller.Create(explorer) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}
