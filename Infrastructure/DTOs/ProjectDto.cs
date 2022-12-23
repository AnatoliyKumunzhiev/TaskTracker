using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs
{
    public class ProjectDto
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public ProjectStatusDto Status { get; set; }

        public int Priority { get; set; }
        
        public List<TaskDto> Tasks { get; set; }
    }
}
