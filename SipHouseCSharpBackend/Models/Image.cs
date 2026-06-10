using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SipHouseCSharpBackend.Models;

[Table("images")]
public class Image
{
    [Key]
    public long Id { get; set; }

    public Guid? FileId { get; set; } = null;
    [MaxLength(32)] public required string Name { get; set; }
    [MaxLength(256)] public required string Description { get; set; }
    public bool IsMainImage { get; set; } = false;
    public int Sort { get; set; }
    public long ProjectId { get; set; }
    [ForeignKey(nameof(ProjectId))] public Project Project { get; set; } = null!;
}