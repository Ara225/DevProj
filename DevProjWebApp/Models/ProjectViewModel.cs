using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevProjWebApp.Models
{
    public class ProjectViewModel
    {
        [Required]
        public string Name { get; set; }

        [DisplayName("URL to Project Repository")]
        public string RepositoryURL { get; set; }

        [Required]
        public string Description { get; set; }

        public bool isPrivate { get; set; }

        public List<GoalViewModel> GoalsList { get; set; }
    }
}
