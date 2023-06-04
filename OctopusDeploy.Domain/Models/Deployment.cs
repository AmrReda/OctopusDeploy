namespace OctopusDeploy.Domain.Models;

public class Deployment
{
    public string Id { get; set; }
    public string ReleaseId { get; set; }
    public string EnvironmentId { get; set; }
    public DateTimeOffset DeployedAt { get; set; }
}