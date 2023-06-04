using OctopusDeploy.Domain.Models;

namespace OctopusDeploy.Domain.Repositories;

public interface IReleaseRepository
{
    IEnumerable<Release>? QueryBy(Func<Release, bool> func);
}