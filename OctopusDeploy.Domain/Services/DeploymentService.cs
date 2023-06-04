using Microsoft.Extensions.Logging;
using OctopusDeploy.Domain.Models;
using OctopusDeploy.Domain.Repositories;
using Environment = OctopusDeploy.Domain.Models.Environment;

namespace OctopusDeploy.Domain.Services;

public class DeploymentService : IDeploymentService
{
    private readonly IDeploymentRepository _deploymentRepository;
    private readonly IReleaseRepository _releaseRepository;
    private readonly IProjectEnvironmentRepository _projectEnvironmentRepository;
    private readonly ILogger _logger;

    public DeploymentService(
        IDeploymentRepository deploymentRepositroy,
        IReleaseRepository releaseRepository,
        IProjectEnvironmentRepository projectEnvironmentRepository,
        ILogger logger)
    {
        _deploymentRepository = deploymentRepositroy;
        _logger = logger;
        _projectEnvironmentRepository = projectEnvironmentRepository;
        _releaseRepository = releaseRepository;
    }

    public IEnumerable<Deployment> GetAllDeployments() => this.GetListDeployments();
    
    public IEnumerable<Deployment> GetAllDeploymentsForProject(string? projectId)
    {
        return GetDescSortedDeployments(projectId);
    }
    
    public void GetAndDeleteOldDeployments(string projectId, int deploymentToKeep)
    {
        var descSortedDeployments = GetDescSortedDeployments(projectId);
        (_projectEnvironmentRepository.GetAll() ?? Array.Empty<Environment>())
            .Select(x => x.Name)
            .ToList()
            .ForEach(env => {
            DeleteOldDeployment(env, descSortedDeployments, deploymentToKeep);
        });
    }
    
    private IEnumerable<Deployment> GetDescSortedDeployments(string? projectId)
    {
        var releaseIds = (_releaseRepository
                .QueryBy((x) => x.ProjectId == projectId) ?? Array.Empty<Release>())
            .Select(p => p.Id).ToList();
         
        return (_deploymentRepository
                .QueryBy((x) => releaseIds.Contains(x.ReleaseId)) ?? Array.Empty<Deployment>())
            .OrderByDescending(d => d.DeployedAt);
    }

    private void DeleteOldDeployment(string envId, IEnumerable<Deployment> deployments, int deploymentToKeep)
    {
        var filteredDeployment = deployments
            .ToList()
            .FindAll(d => d.EnvironmentId.Equals(envId));

        var deploymentToKeepIds = filteredDeployment.Take(deploymentToKeep).Select(x => x.Id);
        
        _logger.LogInformation($"Keep deployment at {string.Join(",", deploymentToKeepIds)} as it the latest deployment");
        
        var deploymentToRemove = filteredDeployment
            .Skip(deploymentToKeep)
            .ToList();

        if (deploymentToRemove.Count > 0)
        {
            var allDeployments = GetAllDeployments();

            filteredDeployment.ForEach(dep =>
            {
                RemoveDeployments(dep.Id, allDeployments);
            });

            _deploymentRepository.Update(allDeployments.ToList());
        }
    }
    
    private IEnumerable<Deployment> GetListDeployments()
    {
        return _deploymentRepository.GetAll() ?? Array.Empty<Deployment>();
    }
    
    private bool RemoveDeployments(string deploymentId, IEnumerable<Deployment> allDeployments)
    {
        var deployments = allDeployments.ToList();
        
        var itemToRemove = deployments.Single(ad => ad.Id == deploymentId);
        
        return deployments
            .Remove(itemToRemove);
    }
    
}