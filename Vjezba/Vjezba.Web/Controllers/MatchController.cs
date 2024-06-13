using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using Vjezba.Web.Models;
using System.Linq;

namespace Vjezba.Web.Controllers
{
    [Route("CustomMatch")]
    public class MatchController : Controller
    {
        private readonly FootballContext db;

        public MatchController(FootballContext context)
        {
            db = context;
        }

        [HttpGet("versus")]
        public IActionResult Index(int team1Id, int team2Id)
        {
            var team1 = db.Teams.Include(t => t.Players).FirstOrDefault(t => t.TeamId == team1Id);
            var team2 = db.Teams.Include(t => t.Players).FirstOrDefault(t => t.TeamId == team2Id);

            if (team1 == null || team2 == null)
            {
                return NotFound();
            }

            var matchViewModel = new MatchViewModel
            {
                Team1 = team1,
                Team2 = team2
            };

            return View(matchViewModel);
        }
    }
}
