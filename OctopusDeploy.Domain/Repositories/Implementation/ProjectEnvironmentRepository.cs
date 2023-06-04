using System.Text.Json;
using Environment = OctopusDeploy.Domain.Models.Environment;

namespace OctopusDeploy.Domain.Repositories.Implementation;

public class ProjectEnvironmentRepository : IProjectEnvironmentRepository
{
    private readonly string _jsonFilePath;

    public ProjectEnvironmentRepository(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
    }
    
    public IEnumerable<Environment>? GetAll()
    {
        using StreamReader r = new StreamReader(_jsonFilePath);
        var json = r.ReadToEnd();
        var result =  JsonSerializer.Deserialize<List<Environment>>(json);

        return result;
    }
}