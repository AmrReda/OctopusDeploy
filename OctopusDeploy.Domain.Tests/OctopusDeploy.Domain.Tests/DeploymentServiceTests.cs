using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using OctopusDeploy.Domain.Repositories;
using OctopusDeploy.Domain.Repositories.Implementation;
using OctopusDeploy.Domain.Services;

namespace OctopusDeploy.Domain.Tests;

public class DeploymentServiceTests
{
    const string BaseTestFilePath  =  @"./TestData/";
    const string DeploymentFileName = BaseTestFilePath + "Deployments.json";
    const string ReleaseFileName =  BaseTestFilePath + "Releases.json";
    const string EnvironmentFileName = BaseTestFilePath + "Environments.json";
    
    private DeploymentService _deploymentService;
    
    public DeploymentServiceTests()
    {
        var logger = A.Fake<ILogger>();
        var deploymentRepository = new DeploymentRepository(DeploymentFileName);
        var releaseRepository = new ReleaseRepository(ReleaseFileName);
        var environmentRepository = new ProjectEnvironmentRepository(EnvironmentFileName);

        _deploymentService = new DeploymentService(deploymentRepository, releaseRepository, environmentRepository, logger);
    }
    
    [Fact]
    public void GetAllDeployments_Success()
    {
        var allDeployments = _deploymentService.GetAllDeployments();

        allDeployments.Should().HaveCountGreaterThan(1);
    }
    
    
    [Fact]
    public void DeleteOldDeployments_ForProject1_Keep_1_Deployments()
    {
        var deploymentsBeforeDelete = _deploymentService.GetAllDeploymentsForProject("Project-1");

        _deploymentService.GetAndDeleteOldDeployments("Project-1", 1);

        var deploymentsAfterDelete = _deploymentService.GetAllDeploymentsForProject("Project-1");
        
        //Assert
        deploymentsBeforeDelete.Should().HaveCountGreaterThanOrEqualTo(deploymentsAfterDelete.Count());
    }
    
    [Fact]
    public void DeleteOldDeployments_ForProject1_Keep_2_Deplyoments()
    {
        var deploymentsBeforeDelete = _deploymentService.GetAllDeploymentsForProject("Project-1");

        _deploymentService.GetAndDeleteOldDeployments("Project-1", 2);

        var deploymentsAfterDelete = _deploymentService.GetAllDeploymentsForProject("Project-1");
        
        //Assert
        deploymentsBeforeDelete.Should().HaveCountGreaterThanOrEqualTo(deploymentsAfterDelete.Count());
    }

    [Fact]
    public void DeleteTwoOldDeployments_ForProject1_1_Deployments()
    {
        var deploymentsBeforeDelete = _deploymentService.GetAllDeploymentsForProject("Project-2");

        _deploymentService.GetAndDeleteOldDeployments("Project-2", 1);

        var deploymentsAfterDelete = _deploymentService.GetAllDeploymentsForProject("Project-2");

        
        //Assert
        deploymentsBeforeDelete.Should().HaveCountGreaterThanOrEqualTo(deploymentsAfterDelete.Count());
    }
}