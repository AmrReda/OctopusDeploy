using Environment = OctopusDeploy.Domain.Models.Environment;

namespace OctopusDeploy.Domain.Repositories;

public interface IProjectEnvironmentRepository
{
    IEnumerable<Environment>? GetAll();
}