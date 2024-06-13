using Vjezba.Model;

namespace Vjezba.Web.Models
{
    public class MatchViewModel
    {
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public List<PlayerStatsViewModel> Team1PlayerStats { get; set; } = new List<PlayerStatsViewModel>();
        public List<PlayerStatsViewModel> Team2PlayerStats { get; set; } = new List<PlayerStatsViewModel>();
    }
}
