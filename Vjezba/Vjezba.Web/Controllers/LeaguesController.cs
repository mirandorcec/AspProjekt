using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Web.Controllers
{
    public class LeaguesController : Controller
    {
        private readonly FootballContext db;

        public LeaguesController(FootballContext context)
        {
            db = context;
        }

        // GET: Leagues
        public IActionResult Index()
        {
            var leagues = db.Leagues.Include(l => l.Teams);
            return View(leagues.ToList());
        }

        // GET: Leagues/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var league = db.Leagues.Include(l => l.Teams)
                                   .FirstOrDefault(l => l.LeagueId == id);
            if (league == null)
            {
                return NotFound();
            }
            return View(league);
        }

        // GET: Leagues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leagues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Country")] League league)
        {
            if (ModelState.IsValid)
            {
                db.Leagues.Add(league);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(league);
        }

        // GET: Leagues/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var league = db.Leagues.Find(id);
            if (league == null)
            {
                return NotFound();
            }
            return View(league);
        }

        // POST: Leagues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("LeagueId,Name,Country")] League league)
        {
            if (id != league.LeagueId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(league).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeagueExists(league.LeagueId))
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
            return View(league);
        }

        // GET: Leagues/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var league = db.Leagues.Find(id);
            if (league == null)
            {
                return NotFound();
            }
            return View(league);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var league = db.Leagues.Include(l => l.Teams).FirstOrDefault(l => l.LeagueId == id);
            if (league == null)
            {
                return NotFound();
            }

            if (league.Teams.Any())
            {
                ModelState.AddModelError("", "Cannot delete this league because it has teams associated with it.");
                return View(league);
            }

            try
            {
                db.Leagues.Remove(league);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to delete league. Please try again, and if the problem persists, see your system administrator.");
                return View(league);
            }
        }

        private bool LeagueExists(int id)
        {
            return db.Leagues.Any(e => e.LeagueId == id);
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
