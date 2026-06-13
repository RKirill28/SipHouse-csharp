using SipHouseCSharpBackend.Models;

namespace SipHouseCSharpBackend.Extensions;

// public static class ProjectExtensions
// {
//
//     public static ProjectDTO ToDtoWithInclude(this Project project)
//     {
//         return new ProjectDTO
//         {
//             Description = project.Description,
//             Id = project.Id,
//             Images = project.Images.Select((image => new ImageDTO
//             {
//                 Description = image.Description,
//                 Id = image.Id,
//                 IsMainImage = image.IsMainImage,
//                 Name = image.Name,
//                 ProjectId = image.ProjectId,
//                 Sort = image.Sort,
//                 Url = image.Url
//             })).ToList(),
//             IsPublic = project.IsPublic,
//             Name = project.Name,
//             PdfUrls = project.PdfUrls,
//             Price = project.Price,
//             PriceDescription = project.PriceDescription
//         };
//     }
// }