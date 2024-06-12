using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class League
    {
        [Key]
        public int LeagueId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "League name must be at least 3 characters long.")]
        public string Name { get; set; } = "";

        [Required]
        [MinLength(2, ErrorMessage = "Country name must be at least 2 characters long.")]
        public string Country { get; set; } = "";

        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
