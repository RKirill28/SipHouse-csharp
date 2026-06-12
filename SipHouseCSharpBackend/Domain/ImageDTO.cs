using System.Linq.Expressions;
using SipHouseCSharpBackend.Models;

namespace SipHouseCSharpBackend.Domain;

public class CreateImageDTO 
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsMainImage { get; set; } = false;
    public int Sort { get; set; }
    public long ProjectId { get; set; }
    
}

public class UpdateImageDTO
{
    public string? Name { get; set; } = null;
    public string? Description { get; set; } = null;
    public bool? IsMainImage { get; set; } = null;
    public int? Sort { get; set; } = null;
}

public class ReadImageDTO : CreateImageDTO
{
    public long Id { get; set; }
    public string? FilePath { get; set; }

    public static Expression<Func<Image, ReadImageDTO>> Projection => i => new ReadImageDTO
    {
        Description = i.Description,
        Id = i.Id,
        IsMainImage = i.IsMainImage,
        Name = i.Name,
        ProjectId = i.ProjectId,
        Sort = i.Sort,
        FilePath = i.FilePath
    };
}
