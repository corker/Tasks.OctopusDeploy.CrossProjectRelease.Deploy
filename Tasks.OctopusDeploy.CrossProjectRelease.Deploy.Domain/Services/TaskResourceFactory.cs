using System.Reflection;
using log4net;
using Octopus.Client;
using Octopus.Client.Model;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public class TaskResourceFactory : ICreateTaskResources
    {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IOctopusRepository _repository;

        public TaskResourceFactory(IOctopusRepository repository)
        {
            _repository = repository;
        }

        public TaskResource Create(SnapshotStep snapshotStep, string environmentId)
        {
            Log.Info($"Creating Delpoyment task.");

            var deploymentResource = new DeploymentResource
            {
                ReleaseId = snapshotStep.ReleaseId,
                ProjectId = snapshotStep.ProjectId,
                EnvironmentId = environmentId
            };
            deploymentResource = _repository.Deployments.Create(deploymentResource);
            return _repository.Tasks.Get(deploymentResource.TaskId);
        }
    }
}