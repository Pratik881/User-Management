using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.ViewModels
{
	public class JoinDocumentViewModel
	{
		[Required(ErrorMessage ="Document Id is required")]
		public string DocumentId { get; set; }	
	}
}
