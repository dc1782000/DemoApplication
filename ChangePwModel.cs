using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
	public class ChangePwModel
	{
        
        public string Email { get; set; }

        [Required(ErrorMessage = "CurrentPassword is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password must be at least 5 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
        ErrorMessage = "Password must have at least one lowercase letter, one uppercase letter, one digit, and one special character")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        public string ConfirmPassword { get; set; }
	}
}

