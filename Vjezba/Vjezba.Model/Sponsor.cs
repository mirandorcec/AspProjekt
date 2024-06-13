using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Sponsor
    {
        [Key]
        public int SponsorId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Sponsor name must be at least 3 characters long.")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Sponsor type must be at least 3 characters long.")]
        public string Type { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
