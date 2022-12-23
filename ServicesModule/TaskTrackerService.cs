using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using DataAccessModule;
using DataAccessModule.Interfaces;
using DataAccessModule.UnitOfWork;
using Infrastructure.DTOs;
using Infrastructure.Filters;
using ServicesModule.Extensions.DTOs;
using ServicesModule.Extensions.Entities;

namespace ServicesModule
{
    public class TaskTrackerService
    {
        #region Project

        /// <summary>
        /// Get project with task list by project key
        /// </summary>
        ///<param name="id">project key<see cref="Project"/></param>
        /// <returns>project dto<see cref="ProjectDto"/></returns>
        public ProjectDto GetProject(Guid id)
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                return QueryProjectWithInfo(uow).FirstOrDefault(e => e.Id == id)?.ToInternal();
            }
        }

        /// <summary>
        /// Get all projects with task list
        /// </summary>
        /// <returns>project dto list <see cref="ProjectDto"/></returns>
        public List<ProjectDto> GetAllProjects()
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                return QueryProjectWithInfo(uow).ToList().Select(e => e.ToInternal()).ToList();
            }
        }

        /// <summary>
        /// Get all projects by filer params <see cref="ProjectFilter"/>
        /// </summary>
        /// <param name="filter">Filter<see cref="ProjectFilter"/></param>
        /// <returns>project dto list<see cref="ProjectDto"/></returns>
        public List<ProjectDto> GetProjectsByFilter(ProjectFilter filter)
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                var queryResult = QueryProjectWithInfo(uow);

                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Name))
                        queryResult = queryResult.Where(e => e.Name.Contains(filter.Name));
                    
                    if (filter.StatusId != null)
                        queryResult = queryResult.Where(e => e.StatusId == filter.StatusId);
                    
                    if (filter.Priority != null)
                        queryResult = queryResult.Where(e => e.Priority == filter.Priority);

                    if(filter.StartDateRange != null)
                    {
                        var startDateRange = filter.StartDateRange;

                        if(startDateRange.StartDate != null)
                            queryResult = queryResult.Where(e => e.StartDate >= startDateRange.StartDate);

                        if(startDateRange.EndDate != null)
                            queryResult = queryResult.Where(e => e.StartDate <= startDateRange.EndDate);
                    }

                    if(filter.CompletionDateRange != null)
                    {
                        var completionDateRange = filter.CompletionDateRange;

                        if(completionDateRange.StartDate != null)
                            queryResult = queryResult.Where(e => e.CompletionDate >= completionDateRange.StartDate);

                        if(completionDateRange.EndDate != null)
                            queryResult = queryResult.Where(e => e.CompletionDate <= completionDateRange.EndDate);
                    }

                    if (filter.OrderByStartDate && filter.OrderByPriority)
                    {
                        queryResult = queryResult.OrderBy(e => e.StartDate).ThenBy(e => e.Priority);
                    }
                    else
                    {
                        if (filter.OrderByStartDate)
                            queryResult = queryResult.OrderBy(e => e.StartDate);

                        if (filter.OrderByPriority)
                            queryResult = queryResult.OrderBy(e => e.Priority);
                    }
                }

                return queryResult.ToList().Select(e => e.ToInternal()).ToList();
            }
        }

        /// <summary>
        /// Delete project and child tasks by key
        /// </summary>
        /// <param name="id">Project key<see cref="Project"/></param>
        public void DeleteProject(Guid id)
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                if (uow.Repository<Project>().Any(e => e.Id == id))
                {
                    uow.Repository<Task>().RemoveRange(uow.Repository<Task>().Where(e => e.ProjectId == id));
                    uow.Repository<Project>().Remove(uow.Repository<Project>().First(e => e.Id == id));
                }
                else
                {
                    throw new Exception($"Project with key = {id} not found");
                }

                uow.SaveChanges();
            }
        }

        /// <summary>
        /// Add project and child task list in database
        /// </summary>
        /// <param name="dto">project dto<see cref="ProjectDto"/></param>
        public void InsertProject(ProjectDto dto)
        {
            InsertOrUpdateProject(dto);
        }

        
        /// <summary>
        /// Update project and child task list in database
        /// </summary>
        /// <param name="dto">project dto<see cref="ProjectDto"/></param>
        public void UpdateProject(ProjectDto dto)
        {
            InsertOrUpdateProject(dto);
        }

        /// <summary>
        /// Return query of projects with all additional information (tasks, dictionaries) <see cref="ProjectFilter"/>
        /// </summary>
        /// <param name="uow">UnitOfWork<see cref="IUnitOfWork"/></param>
        /// <returns>project query<see cref="Project"/></returns>
        private IQueryable<Project> QueryProjectWithInfo(IUnitOfWork uow)
        {
            return uow.Repository<Project>().Include(nameof(Project.ProjectStatus))
                .Include($"{nameof(Project.Tasks)}.{nameof(Task.TaskStatus)}");
        }

        /// <summary>
        /// Insert or uprate project with task list<see cref="ProjectFilter"/>
        /// </summary>
        /// <param name="dto">project dto<see cref="ProjectDto"/></param>
        private void InsertOrUpdateProject(ProjectDto dto)
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                var project = dto.ToExternal();
                uow.Repository<Project>().AddOrUpdate(project);

                if (dto.Tasks?.Count > 0)
                    dto.Tasks.Select(e => e.ToExternal()).ToList().ForEach(e => uow.Repository<Task>().AddOrUpdate(e));
                
                uow.SaveChanges();
            }
        }

        #endregion

        #region Task

        /// <summary>
        /// Get task with status info by key
        /// </summary>
        ///<param name="id">Task key<see cref="Task"/></param>
        /// <returns>Task dto<see cref="TaskDto"/></returns>
        public TaskDto GetTask(Guid id)
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                return QueryTaskWithInfo(uow).FirstOrDefault(e => e.Id == id)?.ToInternal();
            }
        }

        /// <summary>
        /// Get task list with status info by project key
        /// </summary>
        ///<param name="projectId">Project key<see cref="Project"/></param>
        /// <returns>Task dto list<see cref="TaskDto"/></returns>
        public List<TaskDto> GetAllTasksByProjectId(Guid projectId)
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                return QueryTaskWithInfo(uow).Where(e => e.ProjectId == projectId).ToList().Select(e => e.ToInternal()).ToList();
            }
        }

        /// <summary>
        /// Delete task by key
        /// </summary>
        ///<param name="id">Task key<see cref="Task"/></param>
        public void DeleteTask(Guid id)
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                if (uow.Repository<Task>().Any(e => e.Id == id))
                {
                    uow.Repository<Task>().Remove(uow.Repository<Task>().First(e => e.Id == id));
                }
                else
                {
                    throw new Exception($"Task with key = {id} not found");
                }

                uow.SaveChanges();
            }
        }

        /// <summary>
        /// Insert task in database
        /// </summary>
        ///<param name="dto">Task dto<see cref="TaskDto"/></param>
        public void InsertTask(TaskDto dto)
        {
            InsertOrUpdateTask(dto);
        }

        /// <summary>
        /// Update task in database
        /// </summary>
        ///<param name="dto">Task dto<see cref="TaskDto"/></param>
        public void UpdateTask(TaskDto dto)
        {
            InsertOrUpdateTask(dto);
        }

        /// <summary>
        /// Return task query with included status info
        /// </summary>
        ///<param name="uow">UnitOfWork<see cref="IUnitOfWork"/></param>
        private IQueryable<Task> QueryTaskWithInfo(IUnitOfWork uow)
        {
            return uow.Repository<Task>().Include(nameof(Task.Project))
                .Include(nameof(Task.TaskStatus));
        }

        
        /// <summary>
        /// Update or insert tasks in database
        /// </summary>
        ///<param name="dto">UnitOfWork<see cref="TaskDto"/></param>
        private void InsertOrUpdateTask(TaskDto dto)
        {
            using (var uow = new UnitOfWork(new AppDbContext()))
            {
                var task = dto.ToExternal();
                uow.Repository<Task>().AddOrUpdate(task);
                uow.SaveChanges();
            }
        }

        #endregion

    }


}
