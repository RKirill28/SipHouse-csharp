using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SipHouseCSharpBackend.Domain;
using SipHouseCSharpBackend.Models;

namespace SipHouseCSharpBackend.Controllers;

[ApiController]
[Route("/api/v1/projects")]
public class ProjectController(SipHouseContext _context, IConfiguration configuration) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<ReadProjectDTO>(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateProject(CreateProjectDTO project)
    {
        var newProject = new Project
        {
            Images = new List<Image>(),
            Description = project.Description,
            IsPublic = project.IsPublic,
            Name = project.Name,
            PdfFilePaths = new List<string>(),
            PriceDescription = project.PriceDescription,
            Price = project.Price
        };

        await _context.AddAsync(newProject);
        await _context.SaveChangesAsync();

        // По стандартам REST API, оказывается метод создания объектов должен возвращать результат с заголовком Location
        // В Location указывается путь, по которому можно получить только что созданный объект
        // Однако возвращать сам объект - необязательно
        var mapper = ReadProjectDTO.Projection.Compile();
        return Created($"/api/v1/projects/{newProject.Id}", mapper(newProject)); 
    }

    [HttpPost("add_pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadPdfFile(IFormFile file, [FromForm(Name = "project_id")] long id)
    {
        if (file.ContentType != "application/pdf") return BadRequest("File must be a pdf");

        if (file == null | file.Length == 0)
            return Problem(detail: "No file", statusCode: StatusCodes.Status502BadGateway);

        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (project == null)
            return Problem(detail: "No project by id", statusCode: StatusCodes.Status404NotFound);

        // TODO: Get base file path from config
        var fileId = Guid.NewGuid();
        string filesPath = configuration.GetValue<string>("StorageSettings:UploadFolder") ?? "files";
        string pdfFilePath = $"{filesPath}/{fileId}.pdf";
        // Uploading large files: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-10.0#upload-large-files-with-streaming
        using (var stream = new FileStream(pdfFilePath, FileMode.Create))
            await file.CopyToAsync(stream);
        
        project.PdfFilePaths.Add(pdfFilePath);
        await _context.SaveChangesAsync();
        
        return Ok(new {fileId = fileId});
    }
    
    [HttpGet]
    [ProducesResponseType<List<ReadProjectDTO>>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetProjects()
    {
       // Одним запросом через LEFT JOIN 
        return Ok(await _context.Projects.Select(ReadProjectDTO.Projection).ToListAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType<ReadProjectDTO>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetProject(long id)
    {
        var project = await _context.Projects
            .Where(p => p.Id == id)
            .Select(ReadProjectDTO.Projection)
            .FirstOrDefaultAsync();
        
        if (project == null) return NotFound();

        return Ok(project);
    }
    
    [HttpGet("random")]
    [ProducesResponseType<List<ReadProjectDTO>>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetRandom(int limit)
    {
        if (limit <= 0) limit = 3;
        
        return Ok(await _context.Projects
            .Where(p => p.IsPublic) // Берем только публичные
            .OrderBy(p => EF.Functions.Random()) // Сортируем по рандому
            .Take(limit)
            .Select(ReadProjectDTO.Projection)
            .ToListAsync()
        );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProject(long id, UpdateProjectDTO newProject)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (project == null) return NotFound();

        project.Name = newProject.Name ?? project.Name;
        project.Description = newProject.Description ?? project.Description;
        project.IsPublic = newProject.IsPublic ?? project.IsPublic;
        project.Price = newProject.Price ?? project.Price;
        project.PriceDescription = newProject.PriceDescription ?? project.PriceDescription;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteProject(long id)
    {
        var project = await _context.Projects
            .Include(p => p.Images) // Берем картинки тоже, чтобы сработало каскадное удаление
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (project == null) return NotFound();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}