using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Team name must be at least 3 characters long.")]
        public string Name { get; set; } = "";

        [Required]
        [MinLength(3, ErrorMessage = "Stadium name must be at least 3 characters long.")]
        public string Stadium { get; set; } = "";

        [ForeignKey(nameof(Manager))]
        public int? ManagerId { get; set; }
        public virtual Manager Manager { get; set; }

        [ForeignKey(nameof(League))]
        public int? LeagueId { get; set; }
        public virtual League League { get; set; }

        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
