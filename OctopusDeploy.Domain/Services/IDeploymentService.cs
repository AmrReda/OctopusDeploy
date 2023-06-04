using OctopusDeploy.Domain.Models;

namespace OctopusDeploy.Domain.Services;

public interface IDeploymentService
{
    IEnumerable<Deployment> GetAllDeployments();
    
    IEnumerable<Deployment> GetAllDeploymentsForProject(string? projectId);

    void GetAndDeleteOldDeployments(string projectId, int deploymentToKeep);
}