namespace OctopusDeploy.Domain.Models;

public class ProjectEnvironment
{
    public string? ProjectId { get; set; }
    
    public IEnumerable<string?> EnvironmentIds { get; set; } = null!;
}