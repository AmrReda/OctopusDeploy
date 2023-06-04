using OctopusDeploy.Domain.Models;

namespace OctopusDeploy.Domain.Repositories;

public interface IDeploymentRepository
{
    IEnumerable<Deployment>? GetAll();
    
    IEnumerable<Deployment>? QueryBy(Func<Deployment, bool> func);
    void Update(List<Deployment> allDeployments);
}