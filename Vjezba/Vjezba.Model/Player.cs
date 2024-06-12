using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "First name must be at least 3 characters long.")]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Range(16, 45, ErrorMessage = "Age must be between 16 and 45.")]
        public int Age { get; set; }

        [Required]
        public Position Position { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
