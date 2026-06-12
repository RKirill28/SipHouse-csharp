using System.Linq.Expressions;
using SipHouseCSharpBackend.Models;

namespace SipHouseCSharpBackend.Domain;

public class CreateProjectDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string PriceDescription { get; set; } 
    public bool IsPublic { get; set; } = false;
}

public class ReadProjectDTO : CreateProjectDTO
{
    public long Id { get; set; }
    public required ICollection<ReadImageDTO> Images { get; set; }
    public required ICollection<string> PdfFilePaths { get; set; }

    public static Expression<Func<Project, ReadProjectDTO>> Projection => p => new ReadProjectDTO()
    {
        Id = p.Id,
        Description = p.Description,
        IsPublic = p.IsPublic,
        Name = p.Name,
        PdfFilePaths = p.PdfFilePaths,
        Price = p.Price,
        PriceDescription = p.PriceDescription,
        Images = p.Images.Select(i => new ReadImageDTO
        {
            Id = i.Id,
            Description = i.Description,
            IsMainImage = i.IsMainImage,
            Name = i.Name,
            ProjectId = i.ProjectId,
            Sort = i.Sort,
            FilePath = i.FilePath
        }).ToList()
    };
}

public class UpdateProjectDTO
{
    public string? Name { get; set; } = null;
    public string? Description { get; set; } = null;
    public decimal? Price { get; set; } = null;
    public string? PriceDescription { get; set; } = null;
    public bool? IsPublic { get; set; } = null;
}