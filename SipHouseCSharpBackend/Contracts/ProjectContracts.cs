namespace SipHouseCSharpBackend.Contracts;

public class CreateProjectRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string PriceDescription { get; set; } 
    public bool IsPublic { get; set; } = false;
}

public class ReadProjectResponse 
{
    public long Id { get; set; }
    public required ICollection<ReadImageResponse> Images { get; set; }
    public required ICollection<string> PdfFilePaths { get; set; }
    
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string PriceDescription { get; set; } 
    public bool IsPublic { get; set; } = false;

}

public class UpdateProjectRequest
{
    public string? Name { get; set; } = null;
    public string? Description { get; set; } = null;
    public decimal? Price { get; set; } = null;
    public string? PriceDescription { get; set; } = null;
    public bool? IsPublic { get; set; } = null;
}