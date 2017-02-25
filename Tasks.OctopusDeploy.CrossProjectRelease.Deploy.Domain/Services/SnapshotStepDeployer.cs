using System;
using System.Reflection;
using log4net;
using Octopus.Client.Model;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public class SnapshotStepDeployer : IDeploySnapshotSteps
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IWaitForExecutedTaskResources _executedTaskResources;

        private readonly ICreateTaskResources _taskResources;

        public SnapshotStepDeployer(
            ICreateTaskResources taskResources,
            IWaitForExecutedTaskResources executedTaskResources
            )
        {
            _taskResources = taskResources;
            _executedTaskResources = executedTaskResources;
        }

        public void Deploy(SnapshotStep step, string environmentId)
        {
            Log.Info($"Deployment step {step.Index} project {step.ProjectName} version {step.ReleaseVersion}. Started.");

            var task = _taskResources.Create(step, environmentId);

            task = _executedTaskResources.Wait(task);

            Log.Debug("Validating deployment task state..");

            if (task.State != TaskState.Success)
            {
                var message = $"Deployment task for project {step.ProjectName} finished with {task.State} state.";
                throw new IndexOutOfRangeException(message);
            }

            Log.Info($"Deployment step {step.Index} project {step.ProjectName} version {step.ReleaseVersion}. Finished.");
        }
    }
}