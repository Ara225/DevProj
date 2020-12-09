using System.Collections.Generic;

namespace DevProjWebApp.Models
{
    public class ProjectViewModel
    {
        public string Name { get; set; }

        public string LinkedRepositoryURL { get; set; }
        
        public string Description { get; set; }

        public bool isPrivate { get; set; }
    }
}
