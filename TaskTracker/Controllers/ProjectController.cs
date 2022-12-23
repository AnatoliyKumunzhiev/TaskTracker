using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Infrastructure.DTOs;
using Infrastructure.Filters;
using ServicesModule;


namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly TaskTrackerService _taskTrackerService;

        public ProjectController()
        {
            _taskTrackerService = new TaskTrackerService();
        }

        /// <summary>
        /// Get project with task list by project key
        /// </summary>
        ///<param name="id">project key</param>
        /// <returns>project dto<see cref="ProjectDto"/></returns>
        [HttpGet("Id")]
        public ProjectDto Get(Guid id)
        {
            return _taskTrackerService.GetProject(id);
        }

        /// <summary>
        /// Get all projects by filer params <see cref="ProjectFilter"/>
        /// </summary>
        /// <param name="filter">Filter<see cref="ProjectFilter"/></param>
        /// <returns>project dto list<see cref="ProjectDto"/></returns>
        [HttpGet("Filter")]
        public List<ProjectDto> GetProjectsByFilter([FromQuery] ProjectFilter filter)
        {
            return _taskTrackerService.GetProjectsByFilter(filter);
        }

        /// <summary>
        /// Get all projects with task list
        /// </summary>
        /// <returns>project dto list <see cref="ProjectDto"/></returns>
        [HttpGet]
        public List<ProjectDto> GetAll()
        {
            return _taskTrackerService.GetAllProjects();
        }

        /// <summary>
        /// Add project and child task list in database
        /// </summary>
        /// <param name="dto">project dto<see cref="ProjectDto"/></param>
        [HttpPut]
        public void Put(ProjectDto dto)
        { 
            _taskTrackerService.InsertProject(dto);
        }

        /// <summary>
        /// Update project and child task list in database
        /// </summary>
        /// <param name="dto">project dto<see cref="ProjectDto"/></param>
        [HttpPatch]
        public void Update(ProjectDto dto)
        {
            _taskTrackerService.UpdateProject(dto);
        }

        /// <summary>
        /// Delete project and child tasks by key
        /// </summary>
        /// <param name="id">Project key</param>
        [HttpDelete("Id")]
        public void Delete(Guid id)
        {
            _taskTrackerService.DeleteProject(id);
        }
    }
}
