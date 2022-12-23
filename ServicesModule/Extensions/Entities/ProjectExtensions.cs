using System.Collections.Generic;
using DataAccessModule;
using Infrastructure.DTOs;
using LinqKit;

namespace ServicesModule.Extensions.Entities
{
    public static class ProjectExtensions
    {
        /// <summary>
        /// Transform Project from entity to dto
        /// </summary>
        public static ProjectDto ToInternal(this Project entity)
        {
            var dto = new ProjectDto
            {
                Id = entity.Id, 
                Name = entity.Name, 
                StartDate = entity.StartDate,
                CompletionDate = entity.CompletionDate,
                Priority = entity.Priority,
                Status = new ProjectStatusDto()
                {
                    Id = entity.StatusId,
                    Name = entity.ProjectStatus?.Name
                },
                Tasks = new List<TaskDto>()
            };

            entity.Tasks.ForEach(e => dto.Tasks.Add(e.ToInternal()));

            return dto;
        }
    }
}
