using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Web.Controllers
{
    public class PlayersController : Controller
    {
        private readonly FootballContext db;

        public PlayersController(FootballContext context)
        {
            db = context;
        }

        // GET: Players
        public IActionResult Index()
        {
            var players = db.Players.Include(p => p.Team);
            return View(players.ToList());
        }

        // GET: Players/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var player = db.Players.Include(p => p.Team)
                                   .FirstOrDefault(p => p.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.Position = new SelectList(Enum.GetValues(typeof(Position)).Cast<Position>());
            return View();
        }

        // POST: Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,Age,Position,TeamId")] Player player)
        {

            db.Players.Add(player);
            db.SaveChanges();


            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", player.TeamId);
            ViewBag.Position = new SelectList(Enum.GetValues(typeof(Position)).Cast<Position>());

            return RedirectToAction(nameof(Index));
        }

        // GET: Players/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var player = db.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", player.TeamId);
            ViewBag.Position = new SelectList(Enum.GetValues(typeof(Position)).Cast<Position>(), player.Position);

            return View(player);
        }

        // POST: Players/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PlayerId,FirstName,LastName,Age,Position,TeamId")] Player player)
        {
            if (id != player.PlayerId)
            {
                return BadRequest();
            }


            try
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(player.PlayerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", player.TeamId);
            ViewBag.Position = new SelectList(Enum.GetValues(typeof(Position)).Cast<Position>(), player.Position);

            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return db.Players.Any(e => e.PlayerId == id);
        }


        // GET: Players/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var player = db.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteAjax(int id)
        {
            var player = db.Players.Find(id);
            if (player == null)
            {
                return Json(new { success = false, message = "Player not found." });
            }

            db.Players.Remove(player);
            db.SaveChanges();
            return Json(new { success = true, message = "Player deleted successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Json(new List<object>());
            }

            var players = await db.Players
                .Include(p => p.Team)
                .Where(p => p.FirstName.Contains(query) || p.LastName.Contains(query))
                .Select(p => new
                {
                    p.FirstName,
                    p.LastName,
                    p.Age,
                    p.Position,
                    TeamName = p.Team != null ? p.Team.Name : "No Team"
                })
                .ToListAsync();

            return Json(players);
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
