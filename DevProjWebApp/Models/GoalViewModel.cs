using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevProjWebApp.Models
{
    public class GoalViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public DateTime GoalDueBy { get; set; }
    }
}
