using Infrastructure.DTOs;

namespace ServicesModule.Extensions.Entities
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Transform Task from entity to dto
        /// </summary>
        public static TaskDto ToInternal(this DataAccessModule.Task entity)
        {
            var dto = new TaskDto
            {
                Id = entity.Id, 
                Name = entity.Name, 
                Description = entity.Description,
                Priority = entity.Priority,
                ProjectId = entity.ProjectId,
                Status = new TaskStatusDto
                {
                    Id = entity.StatusId,
                    Name = entity.TaskStatus?.Name
                }
            };

            return dto;
        }
    }
}
