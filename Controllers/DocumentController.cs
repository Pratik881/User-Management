using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UserManagementSystem.Data;
using UserManagementSystem.Models;

namespace UserManagementSystem.Controllers
{
    [Authorize] // Ensure only authenticated users can access document actions
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DocumentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Documents()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var userId = _userManager.GetUserId(User);
            var documents = await _context.Documents
                .Where(d => d.OwnerId == userId)
                .ToListAsync();

            return Json(documents);
        }


        // Action to handle document creation
        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] Document model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                return BadRequest("Title is required.");
            }

            var userId = _userManager.GetUserId(User); // Get the current user's ID

            var newDocument = new Document
            {
                Id = Guid.NewGuid().ToString(),
                Title = model.Title,
                Content = "Default content",
                OwnerId = userId
            };

            _context.Documents.Add(newDocument);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Document has been created successfully!" });
        }
    }
}
