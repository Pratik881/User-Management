using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManagementSystem.Data;
using UserManagementSystem.Models;
using UserManagementSystem.ViewModels;

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


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var documents = await _context.Documents
                .Where(d => d.OwnerId == userId || d.DocumentUsers.Any(du => du.UserId == userId && du.Role == "Collaborator"))
                .ToListAsync();
            foreach(var document in documents)
            {
              document.IsOwner=document.OwnerId== userId; 
            }

            return View(documents);
        }


        // Action to handle document creation
        [HttpPost]
        public IActionResult CreateDocument(Document document)
        {
            if (!ModelState.IsValid)
            {
                // Redirect to the form page if validation fails
                return View("Documents", document);
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            document.OwnerId = currentUserId;

            document.DocumentUsers = new List<DocumentUsers>
    {
        new DocumentUsers
        {
            UserId = currentUserId,
            Role = "Owner"
        }
    };
            document.Id = Guid.NewGuid().ToString();
            _context.Documents.Add(document);
            _context.SaveChanges();

            return RedirectToAction("GetDocuments"); // Redirect to documents list
        }

		[HttpPost]
		public async Task<IActionResult> JoinDocument(JoinDocumentViewModel model)
		{
			if (string.IsNullOrEmpty(model.DocumentId)) { 
                ModelState.AddModelError("DocumentId", "Document Id is required");
                return View(model);
            };
			var documentId = model.DocumentId?.Trim(); // To remove any extra spaces
			Console.WriteLine($"Looking for document with ID: {documentId}");
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id ==documentId);

           
            if (document == null)
            {
                ModelState.AddModelError("DocumentId", "Document Not found");
                return View(model);
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingCollaborator = await _context.DocumentUsers.FirstOrDefaultAsync(du => du.DocumentId == document.Id && du.UserId== currentUserId);
            if ( existingCollaborator != null) 
            {
                if (existingCollaborator.Role == "Collaborator")
                {
                    ModelState.AddModelError("DocumentId", "You are already a collaborator here");

                }
                if (existingCollaborator.Role == "Owner")
                {
                    ModelState.AddModelError("DocumentId", "You are the owner of this document");
                    return View(model);
				}
            }
            var newCollaborator = new DocumentUsers
            {
                DocumentId = document.Id,
                UserId = currentUserId,
                Role = "Collaborator"
            };

            _context.DocumentUsers.Add(newCollaborator);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetDocuments");
                 
        }

    public async Task <IActionResult> DeleteDocument([FromQuery] string documentId)
        
        {
            
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == documentId);

            if (document ==null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId!= document.OwnerId) {
                return Unauthorized();


            }
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetDocuments");
        }

        [HttpGet]
        public async Task<IActionResult> EditDocument([FromQuery] string ToEdit)
        {
            var document=await _context.Documents.FirstOrDefaultAsync(d=>d.Id==ToEdit);
            if (document==null)
            {
                return NotFound();
            }
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
             var relatedUser = document.DocumentUsers.FirstOrDefault(d => d.DocumentId == ToEdit && d.UserId == currentUserId);
            
           
             if(relatedUser==null) {
                return Unauthorized();
                    }
            return View(document);
        }

    }
}
