using Vjezba.Model;

namespace Vjezba.Web.Models
{
    public class PlayerStatsViewModel
    {
        public string PlayerName { get; set; }
        public Position Position { get; set; }
        public int DEF { get; set; }
        public int SKILL { get; set; }
        public int SHOOTING { get; set; }
        public int PASSING { get; set; }
        public int SCORING { get; set; }
    }
}
