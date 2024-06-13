using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using System.Linq;

namespace Vjezba.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly FootballContext db;

        public TeamsController(FootballContext context)
        {
            db = context;
        }

        // GET: Teams
        public IActionResult Index()
        {
            var teams = db.Teams.Include(t => t.League).Include(t => t.Manager);
            return View(teams.ToList());
        }

        // GET: Teams/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var team = db.Teams.Include(t => t.Manager)
                               .Include(t => t.League)
                               .Include(t => t.Players)
                               .FirstOrDefault(t => t.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name");
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Stadium,LeagueId")] Team team)
        {

            db.Teams.Add(team);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        // GET: Teams/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var team = db.Teams.Include(t => t.Manager).FirstOrDefault(t => t.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
            return View(team);
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("TeamId,Name,Stadium,LeagueId,ManagerId")] Team updatedTeam)
        {
            if (id != updatedTeam.TeamId)
            {
                return BadRequest();
            }


            try
            {
                var existingTeam = db.Teams.Include(t => t.Manager).FirstOrDefault(t => t.TeamId == updatedTeam.TeamId);

                if (existingTeam == null)
                {
                    return NotFound();
                }

                existingTeam.Name = updatedTeam.Name;
                existingTeam.Stadium = updatedTeam.Stadium;
                existingTeam.LeagueId = updatedTeam.LeagueId;

                // Update manager relationships
                if (existingTeam.ManagerId != updatedTeam.ManagerId)
                {
                    if (existingTeam.ManagerId != null)
                    {
                        var oldManager = db.Managers.FirstOrDefault(m => m.ManagerId == existingTeam.ManagerId);
                        if (oldManager != null)
                        {
                            oldManager.TeamId = null;
                        }
                    }

                    if (updatedTeam.ManagerId != null)
                    {
                        var newManager = db.Managers.FirstOrDefault(m => m.ManagerId == updatedTeam.ManagerId);
                        if (newManager != null)
                        {
                            newManager.TeamId = updatedTeam.TeamId;
                        }
                    }

                    existingTeam.ManagerId = updatedTeam.ManagerId;
                }

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(updatedTeam.TeamId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));



        }

        private bool TeamExists(int id)
        {
            return db.Teams.Any(e => e.TeamId == id);
        }

        // GET: Teams/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var team = db.Teams.Include(t => t.League).Include(t => t.Manager).FirstOrDefault(t => t.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var team = db.Teams.Include(t => t.Players).Include(t => t.Manager).FirstOrDefault(t => t.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            try
            {
                if (team.Manager != null)
                {
                    team.Manager.TeamId = null;
                }
                db.Teams.Remove(team);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to delete team. Please try again.");
                return View(team);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
