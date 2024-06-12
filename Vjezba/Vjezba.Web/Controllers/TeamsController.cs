using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;

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
            var teams = db.Teams.Include(t => t.Manager).Include(t => t.League);
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
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName");
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name");
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Stadium,ManagerId,LeagueId")] Team team)
        {

            db.Teams.Add(team);
            db.SaveChanges();

            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", team.ManagerId);
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Teams/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", team.ManagerId);
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
            return View(team);
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("TeamId,Name,Stadium,ManagerId,LeagueId")] Team team)
        {
            if (id != team.TeamId)
            {
                return BadRequest();
            }

       
                try
                {
                    db.Entry(team).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TeamId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", team.ManagerId);
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);
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
            var team = db.Teams.Include(t => t.Manager).Include(t => t.League).FirstOrDefault(t => t.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var team = db.Teams.Include(t => t.Players).FirstOrDefault(t => t.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            if (team.Players.Any())
            {
                ModelState.AddModelError("", "Cannot delete this team because it has players associated with it.");
                return View(team);
            }

            try
            {
                db.Teams.Remove(team);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to delete team. ");
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
