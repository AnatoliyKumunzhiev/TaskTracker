using System;
using System.Linq;
using DataAccessModule;
using DataAccessModule.DictionaryConstants;
using DataAccessModule.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbTest
{
    [TestClass]
    public class DbTests
    {
        [TestMethod]
        public void ClearAndFillTestDb()
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                uow.Repository<Task>().RemoveRange(uow.Repository<Task>());
                uow.Repository<Project>().RemoveRange(uow.Repository<Project>());
                uow.SaveChanges();

                var anyProjects = uow.Repository<Project>().Any();
                var anyTasks = uow.Repository<Task>().Any();

                Assert.IsTrue(!anyProjects && !anyTasks, "Entities delete failed");

                #region First project

                var project1 = new Project
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.Now.AddYears(-1),
                    Name = "Extremely important project",
                    Priority = 0,
                    StatusId = ProjectStatusConstants.ActiveId
                };

                var task11 = new Task
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project1.Id,
                    Name = "First task",
                    Description = "Task number one",
                    StatusId = TaskStatusConstants.InProgressId,
                    Priority = 1
                };

                var task12 = new Task
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project1.Id,
                    Name = "Second task",
                    Description = "Task number two",
                    StatusId = TaskStatusConstants.ToDoId,
                    Priority = 2
                };

                #endregion

                #region Second project

                var project2 = new Project
                {
                    Id = Guid.NewGuid(),
                    StartDate = DateTime.Now.AddYears(-1),
                    Name = "Not an important project",
                    Priority = 1,
                    StatusId = ProjectStatusConstants.NotStartedId
                };

                var task21 = new Task
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project2.Id,
                    Name = "First not important task",
                    Description = "Task number one",
                    StatusId = TaskStatusConstants.ToDoId,
                    Priority = 2
                };

                var task22 = new Task
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project2.Id,
                    Name = "Second not important task",
                    Description = "Task number two",
                    StatusId = TaskStatusConstants.ToDoId,
                    Priority = 3
                };

                #endregion

                uow.Repository<Project>().Add(project1);
                uow.Repository<Project>().Add(project2);
                uow.SaveChanges();

                uow.Repository<Task>().Add(task11);
                uow.Repository<Task>().Add(task12);
                uow.Repository<Task>().Add(task21);
                uow.Repository<Task>().Add(task22);
                uow.SaveChanges();

                Assert.IsTrue(uow.Repository<Project>().Count() == 2, "Wrong number of projects");
                Assert.IsTrue(uow.Repository<Task>().Count() == 4, "Wrong number of tasks");
            }
        }
    }
}
