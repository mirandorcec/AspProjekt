using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;
using System.Linq;

namespace Vjezba.Web.Controllers
{
    public class ManagersController : Controller
    {
        private readonly FootballContext db;

        public ManagersController(FootballContext context)
        {
            db = context;
        }

        // GET: Managers
        public IActionResult Index()
        {
            var managers = db.Managers.Include(m => m.Team);
            return View(managers.ToList());
        }

        // GET: Managers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var manager = db.Managers.Include(m => m.Team)
                                     .FirstOrDefault(m => m.ManagerId == id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }

        // GET: Managers/Create
        public IActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.ManagerId == null), "TeamId", "Name");
            return View();
        }

        // POST: Managers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,Age,TeamId")] Manager manager)
        {

            db.Managers.Add(manager);
            db.SaveChanges();

            if (manager.TeamId.HasValue)
            {
                var team = db.Teams.Find(manager.TeamId);
                if (team != null)
                {
                    team.ManagerId = manager.ManagerId;
                    db.SaveChanges();
                }
            }

            return RedirectToAction(nameof(Index));

        }

        // GET: Managers/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var manager = db.Managers.Find(id);
            if (manager == null)
            {
                return NotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.ManagerId == null || t.TeamId == manager.TeamId), "TeamId", "Name", manager.TeamId);
            return View(manager);
        }

        // POST: Managers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ManagerId,FirstName,LastName,Age,TeamId")] Manager manager)
        {
            if (id != manager.ManagerId)
            {
                return BadRequest();
            }


            try
            {
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();

                // If manager's TeamId is set to null, update the corresponding team
                if (!manager.TeamId.HasValue)
                {
                    var team = db.Teams.FirstOrDefault(t => t.ManagerId == manager.ManagerId);
                    if (team != null)
                    {
                        team.ManagerId = null;
                        db.SaveChanges();
                    }
                }
                else
                {
                    var team = db.Teams.Find(manager.TeamId);
                    if (team != null)
                    {
                        team.ManagerId = manager.ManagerId;
                        db.SaveChanges();
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManagerExists(manager.ManagerId))
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

        // GET: Managers/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var manager = db.Managers.Find(id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var manager = db.Managers.Include(m => m.Team).FirstOrDefault(m => m.ManagerId == id);
            if (manager == null)
            {
                return NotFound();
            }

            if (manager.Team != null)
            {
                manager.Team.ManagerId = null;
                db.SaveChanges();
            }

            try
            {
                db.Managers.Remove(manager);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to delete manager. Please try again, and if the problem persists, see your system administrator.");
                return View(manager);
            }
        }

        private bool ManagerExists(int id)
        {
            return db.Managers.Any(e => e.ManagerId == id);
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
