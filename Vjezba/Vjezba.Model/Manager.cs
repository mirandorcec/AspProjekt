using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "First name must be at least 3 characters long.")]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Range(25, 75, ErrorMessage = "Age must be between 25 and 75.")]
        public int Age { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
