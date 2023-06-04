namespace OctopusDeploy.Domain.Models;

public class Release
{
    public string Id { get; set; }
    public string? ProjectId { get; set; }
    public string? Version { get; set; }
    public DateTimeOffset Created { get; set; }
}