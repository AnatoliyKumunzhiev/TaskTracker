using System;
using Infrastructure.DTOs;

namespace ServicesModule.Extensions.DTOs
{
    public static class TaskDtoExtension
    {
        /// <summary>
        /// Transform Task from dto to entity
        /// </summary>
        public static DataAccessModule.Task ToExternal(this TaskDto dto)
        {
            var entity = new DataAccessModule.Task
            {
                Id = dto.Id == default ? Guid.NewGuid() : dto.Id, 
                Name = dto.Name, 
                Description = dto.Description,
                Priority = dto.Priority,
                StatusId = dto.Status?.Id ?? 0,
                ProjectId = dto.ProjectId
            };

            return entity;
        }
    }
}
