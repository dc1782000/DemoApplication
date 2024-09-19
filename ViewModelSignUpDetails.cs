using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
	public class ViewModelSignUpDetails
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password must be at least 5 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
       ErrorMessage = "Password must have at least one lowercase letter, one uppercase letter, one digit, and one special character")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        public string? ConfirmPassword { get; set; }

        public bool IsActive { get; set; }
    }
}

