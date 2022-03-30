using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPersonalBlog.Models;

public class Blog
{
    public int BlogId { get; set; }
    public string? AuthorId { get; set; }
    [Required]
    [StringLength(150, ErrorMessage = "The {0} must be at least {2} and have a maximum of {1} characters", MinimumLength = 5)]
    public string? Name { get; set; }

    [StringLength(500, ErrorMessage = "The {0} must be at least {2} and have a maximum of {1} characters", MinimumLength = 5)]
    public string? Description { get; set; }
    [DataType(DataType.Date)]
    [Display(Name = "Created Date")]
    public DateTime Created { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Updated Date")]
    public DateTime? Updated { get; set; }

    [Display(Name = "Blog Image")]
    public byte[]? ImageData { get; set; }

    [Display(Name = "Image Type")]
    public string? ContentType { get; set; }

    [NotMapped]
    public IFormFile? Image { get; set; }

    public virtual BlogUser? Author { get; set; }
    public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

}
