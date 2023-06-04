using System.Text.Json;
using OctopusDeploy.Domain.Models;

namespace OctopusDeploy.Domain.Repositories.Implementation;

public class DeploymentRepository : IDeploymentRepository
{
    private readonly string _jsonFilePath;

    public DeploymentRepository(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
    }
    
    public IEnumerable<Deployment>? GetAll()
    {
        using StreamReader r = new StreamReader(_jsonFilePath);
        var json = r.ReadToEnd();
        var result =  JsonSerializer.Deserialize<List<Deployment>>(json);

        return result;
    }

    public IEnumerable<Deployment>? QueryBy(Func<Deployment, bool> func)
    {
        using StreamReader r = new StreamReader(_jsonFilePath);
        var json = r.ReadToEnd();
        var result =  JsonSerializer.Deserialize<List<Deployment>>(json);
        
        return result?.Where(func);
    }

    public void Update(List<Deployment> allDeployments)
    {
        using StreamWriter r = new StreamWriter(_jsonFilePath);
        var serializedPlayerDetails = JsonSerializer.Serialize<List<Deployment>>(allDeployments);
        
        r.Write(serializedPlayerDetails);
    }
}