using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Infrastructure.DTOs;
using ServicesModule;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskTrackerService _taskTrackerService;

        public TaskController()
        {
            _taskTrackerService = new TaskTrackerService();
        }

        /// <summary>
        /// Get task with status info by key
        /// </summary>
        ///<param name="id">Task key</param>
        /// <returns>Task dto<see cref="TaskDto"/></returns>
        [HttpGet("Id")]
        public TaskDto Get(Guid id)
        {
            return _taskTrackerService.GetTask(id);
        }

        /// <summary>
        /// Get task list with status info by project key
        /// </summary>
        ///<param name="projectId">Project key</param>
        /// <returns>Task dto list<see cref="TaskDto"/></returns>
        [HttpGet("ProjectId")]
        public List<TaskDto> GetAllByProjectId(Guid projectId)
        {
            return _taskTrackerService.GetAllTasksByProjectId(projectId);
        }

        /// <summary>
        /// Insert task in database
        /// </summary>
        ///<param name="dto">Task dto<see cref="TaskDto"/></param>
        [HttpPut]
        public void Put(TaskDto dto)
        { 
            _taskTrackerService.InsertTask(dto);
        }

        /// <summary>
        /// Update task in database
        /// </summary>
        ///<param name="dto">Task dto<see cref="TaskDto"/></param>
        [HttpPatch]
        public void Update(TaskDto dto)
        {
            _taskTrackerService.UpdateTask(dto);
        }

        /// <summary>
        /// Delete task by key
        /// </summary>
        ///<param name="id">Task key</param>
        [HttpDelete("Id")]
        public void Delete(Guid id)
        {
            _taskTrackerService.DeleteTask(id);
        }
    }
}
