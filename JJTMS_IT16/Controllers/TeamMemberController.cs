using JJTMS_IT16.Data;
using JJTMS_IT16.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JJTMS_IT16.Controllers
{
    // Accessible by Team Members
    [Authorize(Roles = "Team Member")]
    public class TeamMemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;

        public TeamMemberController(ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TeamMember/Tasks
        public async Task<IActionResult> Tasks()
        {
            var userId = _userManager.GetUserId(User);
            var tasks = await _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.AssignedToUserId == userId)
                .ToListAsync();
            return View(tasks);
        }

        // GET: TeamMember/TaskDetails/5
        public async Task<IActionResult> TaskDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id && m.AssignedToUserId == _userManager.GetUserId(User));

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: TeamMember/CompleteTask/5
        [HttpPost]
        public async Task<IActionResult> CompleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null || task.AssignedToUserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            task.IsComplete = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Tasks));
        }
    }
}
