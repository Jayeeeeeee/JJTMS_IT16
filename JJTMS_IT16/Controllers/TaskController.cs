using JJTMS_IT16.Data;
using JJTMS_IT16.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JJTMS_IT16.Controllers
{
    // Accessible by Admin and Team Leader for CRUD
    [Authorize(Roles = "Admin,Team Leader")]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;

        public TaskController(ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Task
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.Tasks.Include(t => t.Project).Include(t => t.AssignedToUser).ToListAsync();
            return View(tasks);
        }

        // GET: Task/Details/5
        [AllowAnonymous] // Allow all roles to view details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Task/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Projects"] = await _context.Projects.ToListAsync();
            ViewData["Users"] = await _userManager.GetUsersInRoleAsync("Team Member");
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("TaskName,Description,DueDate,AssignedToUserId,ProjectId")] TaskModel task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Projects"] = await _context.Projects.ToListAsync();
            ViewData["Users"] = await _userManager.GetUsersInRoleAsync("Team Member");
            return View(task);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            ViewData["Projects"] = await _context.Projects.ToListAsync();
            ViewData["Users"] = await _userManager.GetUsersInRoleAsync("Team Member");
            return View(task);
        }

        // POST: Task/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskName,Description,DueDate,AssignedToUserId,ProjectId")] TaskModel task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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

            ViewData["Projects"] = await _context.Projects.ToListAsync();
            ViewData["Users"] = await _userManager.GetUsersInRoleAsync("Team Member");
            return View(task);
        }

        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
