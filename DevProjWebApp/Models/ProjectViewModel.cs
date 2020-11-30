using System.Collections.Generic;

namespace DevProjWebApp.Models
{
    public class ProjectViewModel
    {
        public string Name { get; set; }

        public List<string> LinkedRepositoriesURLs { get; set; }
        
        public string Description { get; set; }

        public bool isPrivate { get; set; }
    }
}
