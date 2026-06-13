namespace SipHouseCSharpBackend.Contracts;

public class CreateImageRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsMainImage { get; set; } = false;
    public int Sort { get; set; }
    public long ProjectId { get; set; }
    
}

public class UpdateImageRequest
{
    public string? Name { get; set; } = null;
    public string? Description { get; set; } = null;
    public bool? IsMainImage { get; set; } = null;
    public int? Sort { get; set; } = null;
}

public class ReadImageResponse 
{
    public long Id { get; set; }
    public string? FilePath { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsMainImage { get; set; } = false;
    public int Sort { get; set; }
    public long ProjectId { get; set; }

}
