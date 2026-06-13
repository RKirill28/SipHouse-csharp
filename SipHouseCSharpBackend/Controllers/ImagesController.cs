using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SipHouseCSharpBackend.Contracts;
using SipHouseCSharpBackend.Models;

namespace SipHouseCSharpBackend.Controllers;

[ApiController]
[Route("/api/v1/images")]
public class ImagesController(SipHouseContext _context, IConfiguration configuration) : ControllerBase
{
    private readonly string[] _allowedContentTypes = ["image/png", "image/jpeg", "image/jpg"];
    
    [HttpGet("{project_id}")]
    [ProducesResponseType<List<ReadImageResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetImagesByProject([FromRoute(Name = "project_id")] long projectId)
    {
        return Ok(
            await _context.Images
                .Where(i => i.ProjectId == projectId)
                .Select(Mappings.ReadImageResponseProjection)
                .ToListAsync()
            );
    }

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadImageFile(IFormFile file, [FromForm] long imageId)
    {
        if (!_allowedContentTypes.Contains(file.ContentType)) 
            return Problem("File is not supported", statusCode: StatusCodes.Status502BadGateway);
        
        if (file == null | file.Length == 0) 
            return Problem(detail: "No file", statusCode: StatusCodes.Status502BadGateway);
        
        var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == imageId);
        if (image == null) 
            return Problem(detail: "Not found image by id", statusCode: StatusCodes.Status404NotFound);
        
        // TODO: Get base file path from config
        var fileId = Guid.NewGuid();
        string filesPath = configuration.GetValue<string>("StorageSettings:UploadFolder") ?? "files";
        string filePath = $"{filesPath}/{fileId}.png";
        
        // Uploading large file: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-10.0#upload-large-files-with-streaming
        using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

        image.FilePath = filePath;
        await _context.SaveChangesAsync();
        
        return Ok(new {fileId = fileId});
    }
    
    [HttpPost]
    [ProducesResponseType<ReadImageResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateImage(CreateImageRequest image)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == image.ProjectId);
        if (project == null)
            return Problem(detail: "Project not found", statusCode: StatusCodes.Status404NotFound);
        
        var newImage = new Image
        {
            Description = image.Description,
            IsMainImage = image.IsMainImage,
            Name = image.Name,
            ProjectId = image.ProjectId,
            Sort = image.Sort,
        };

        if (newImage.IsMainImage == true)
        {
            var porjectImages = await _context.Images.Where(i => i.ProjectId == project.Id).ToListAsync();
            foreach (var projectImage in porjectImages)
                projectImage.IsMainImage = false;
        }
        await _context.Images.AddAsync(newImage);
        await _context.SaveChangesAsync();

        var mapper = Mappings.ReadImageResponseProjection.Compile();
        return Created($"/api/v1/images/{newImage.ProjectId}", mapper(newImage));
    }

    [HttpPut("{id}")]
    [ProducesResponseType<ReadImageResponse>(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateImage(long id, UpdateImageRequest updateImage)
    {
        var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == id);
        if (image == null)
            return Problem(detail: "Image not found", statusCode: StatusCodes.Status404NotFound);

        var projectImages = await _context.Images
            .Where(i => i.ProjectId == image.ProjectId)
            .ToListAsync();

        image.Name = updateImage.Name ?? image.Name;
        image.Description = updateImage.Description ?? image.Description;
        image.Sort = updateImage.Sort ?? image.Sort;

        if (updateImage.IsMainImage == true)
            foreach (var projectImage in projectImages) 
                projectImage.IsMainImage = false;
        
        image.IsMainImage = updateImage.IsMainImage ?? image.IsMainImage;
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteImage(long id)
    {
        var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == id);
        if (image == null) 
            return Problem(detail: "Image not found", statusCode: StatusCodes.Status404NotFound);

        if (image.FilePath != null)
            System.IO.File.Delete(image.FilePath);
        
        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}