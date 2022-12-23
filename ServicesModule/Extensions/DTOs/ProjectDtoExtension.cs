using System;
using DataAccessModule;
using Infrastructure.DTOs;

namespace ServicesModule.Extensions.DTOs
{
    public static class ProjectDtoExtension
    {
        /// <summary>
        /// Transform Project from dto to entity
        /// </summary>
        public static Project ToExternal(this ProjectDto dto)
        {
            var entity = new Project
            {
                Id = dto.Id == default ? Guid.NewGuid() : dto.Id, 
                Name = dto.Name, 
                StartDate = dto.StartDate,
                CompletionDate = dto.CompletionDate,
                Priority = dto.Priority,
                StatusId = dto.Status?.Id ?? 0
            };

            return entity;
        }
    }
}
