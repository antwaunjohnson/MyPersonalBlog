using System.ComponentModel.DataAnnotations;

namespace MyPersonalBlog.ViewModels;

public class ContactMe
{
    [Required]
    [StringLength(150, ErrorMessage = "The {0} must be at least {2} and have a maximum of {1} characters", MinimumLength = 5)]
    public string? Name { get; set; }
    [Required]
    [EmailAddress]
    [StringLength(150, ErrorMessage = "The {0} must be at least {2} and have a maximum of {1} characters", MinimumLength = 5)]
    public string? Email { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and have a maximum of {1} characters", MinimumLength = 5)]

    public string? Subject { get; set; }
   
    [Required]
    [StringLength(500, ErrorMessage = "The {0} must be at least {2} and have a maximum of {1} characters", MinimumLength = 5)]
    public string? Message { get; set; }
}
