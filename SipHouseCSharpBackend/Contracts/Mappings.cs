using System.Linq.Expressions;
using SipHouseCSharpBackend.Models;

namespace SipHouseCSharpBackend.Contracts;

public class Mappings
{
    public static Expression<Func<Image, ReadImageResponse>> ReadImageResponseProjection => i => new ReadImageResponse
    {
        Description = i.Description,
        Id = i.Id,
        IsMainImage = i.IsMainImage,
        Name = i.Name,
        ProjectId = i.ProjectId,
        Sort = i.Sort,
        FilePath = i.FilePath
    };
    
    public static Expression<Func<Project, ReadProjectResponse>> ReadPojectResponseProjection => p => new ReadProjectResponse()
    {
        Id = p.Id,
        Description = p.Description,
        IsPublic = p.IsPublic,
        Name = p.Name,
        PdfFilePaths = p.PdfFilePaths,
        Price = p.Price,
        PriceDescription = p.PriceDescription,
        Images = p.Images.Select(i => new ReadImageResponse
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