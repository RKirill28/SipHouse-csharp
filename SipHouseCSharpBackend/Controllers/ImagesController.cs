using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SipHouseCSharpBackend.Domain;
using SipHouseCSharpBackend.Models;

namespace SipHouseCSharpBackend.Controllers;

[ApiController]
[Route("/api/v1/images")]
public class ImagesController(SipHouseContext _context) : ControllerBase
{
    [HttpGet("{project_id}")]
    [ProducesResponseType<IEnumerable<ReadImageDTO>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReadImageDTO>>> GetImagesByProject([FromRoute(Name = "project_id")] long projectId)
    {
        return Ok(
            await _context.Images
                .Where(i => i.ProjectId == projectId)
                .Select(ReadImageDTO.Projection)
                .ToListAsync()
            );
    }
    
    [HttpPost]
    [ProducesResponseType<ReadImageDTO>(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateImage(CreateImageDTO image)
    {
        Project? project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == image.ProjectId);
        if (project == null)
        {
            return Problem(detail: "Project not found", statusCode: StatusCodes.Status404NotFound);
        }
        
        Image newImage = new Image
        {
            Description = image.Description,
            IsMainImage = image.IsMainImage,
            Name = image.Name,
            ProjectId = image.ProjectId,
            Sort = image.Sort,
        };
        await _context.Images.AddAsync(newImage);
        await _context.SaveChangesAsync();

        var mapper = ReadImageDTO.Projection.Compile();
        return Created($"/api/v1/images/{newImage.ProjectId}", mapper(newImage));
    }

    [HttpPut("{id}")]
    [ProducesResponseType<ReadImageDTO>(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateImage(long id, UpdateImageDTO updateImage)
    {
        Image? image = await _context.Images.FirstOrDefaultAsync(i => i.Id == id);
        if (image == null)
        {
            return Problem(detail: "Image not found", statusCode: StatusCodes.Status404NotFound);
        }

        IEnumerable<Image> projectImages =
            await _context.Images.Where(i => i.ProjectId == image.ProjectId).ToListAsync();

        image.Name = updateImage.Name ?? image.Name;
        image.Description = updateImage.Description ?? image.Description;
        image.Sort = updateImage.Sort ?? image.Sort;

        if (updateImage.IsMainImage == true)
        {
            foreach (var projectImage in projectImages)
            {
                projectImage.IsMainImage = false;
            }

        } 
        image.IsMainImage = updateImage.IsMainImage ?? image.IsMainImage;
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType<ReadImageDTO>(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteImage(long id)
    {
        Image? image = await _context.Images.FirstOrDefaultAsync(i => i.Id == id);
        if (image == null)
        {
            return Problem(detail: "Image not found", statusCode: StatusCodes.Status404NotFound);
        }
        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}