using System;
using System.Linq;
using Infrastructure.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicesModule;

namespace DbTest
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void ProjectCrudTest()
        {
            var service = new TaskTrackerService();

            var projects = service.GetAllProjects();
            var allNumberBeforeTest = projects.Count;

            #region Insert

            var dto = new ProjectDto()
            {
                Id = Guid.NewGuid(),
                Name = "Test project"
            };

            service.InsertProject(dto);

            Assert.IsTrue(service.GetAllProjects().Count == allNumberBeforeTest + 1, "Insert method error");

            #endregion

            #region Update and GetById

            dto.StartDate = DateTime.Now;

            service.UpdateProject(dto);

            var updatedDto = service.GetProject(dto.Id);

            Assert.IsTrue(updatedDto.StartDate == DateTime.Now.Date, "Update or Get method error");

            #endregion

            #region Delete

            service.DeleteProject(dto.Id);

            Assert.IsTrue(service.GetAllProjects().Count == allNumberBeforeTest, "Delete method error");

            #endregion
        }

        [TestMethod]
        public void TaskCrudTest()
        {
            var service = new TaskTrackerService();
            var projectWasCreated = false;

            var dtoProject = service.GetAllProjects().FirstOrDefault();

            if (dtoProject == null)
            {
                dtoProject = new ProjectDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test project"
                };
                service.InsertProject(dtoProject);
                projectWasCreated = true;
            }

            var tasksInProjectBeforeTest = service.GetAllTasksByProjectId(dtoProject.Id).Count;

            #region Insert

            var dto = new TaskDto()
            {
                Id = Guid.NewGuid(),
                Name = "Test task",
                Description = "Test task description",
                ProjectId = dtoProject.Id
            };
            service.InsertTask(dto);

            Assert.IsTrue(service.GetAllTasksByProjectId(dtoProject.Id).Count == tasksInProjectBeforeTest + 1, "Insert method error");

            #endregion

            #region Update and GetById

            dto.Priority = 3;

            service.UpdateTask(dto);

            var updatedDto = service.GetTask(dto.Id);

            Assert.IsTrue(updatedDto.Priority == 3, "Update or Get method error");

            #endregion

            #region Delete

            service.DeleteTask(dto.Id);

            Assert.IsTrue(service.GetAllTasksByProjectId(dtoProject.Id).Count == tasksInProjectBeforeTest, "Delete method error");

            #endregion

            if (projectWasCreated)
                service.DeleteProject(dtoProject.Id);
        }
    }
}
