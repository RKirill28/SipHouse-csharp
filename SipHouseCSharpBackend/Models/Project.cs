using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SipHouseCSharpBackend.Models;

[Table("projects")]
public class Project
{
    [Key]
    public long Id { get; set; }

    [Required] [MaxLength(32)] public required string Name { get; set; }

    [Required] [MaxLength(500)] public required string Description { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required] [MaxLength(255)] public required string PriceDescription { get; set; }

    public ICollection<string> PdfFilePaths { get; set; } = new List<string>();

    public bool IsPublic { get; set; } = false;

    public ICollection<Image> Images { get; set; } = new List<Image>();

}