using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs
{
    public class TaskDto
    {
        private ProjectDto _project; 

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public TaskStatusDto Status { get; set; }

        public Guid ProjectId { get; set; }

        public ProjectDto Project
        {
            get => _project;
            set
            {
                if (Equals(_project, Project))
                    return;

                _project = value;
                ProjectId = _project?.Id ?? default;
            }
        }
    }
}
