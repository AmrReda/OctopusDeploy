using System.Text.Json;
using OctopusDeploy.Domain.Models;

namespace OctopusDeploy.Domain.Repositories.Implementation;

public class ReleaseRepository : IReleaseRepository
{
    private readonly string _jsonFilePath;

    public ReleaseRepository(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
    }
    public IEnumerable<Release>? QueryBy(Func<Release, bool> func)
    {
        using StreamReader r = new StreamReader(_jsonFilePath);
        var json = r.ReadToEnd();
        var result =  JsonSerializer.Deserialize<List<Release>>(json);
        
        return result?.Where(func);
    }
}