using System.Linq;
using System.Reflection;
using log4net;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public class SnapshotDeployer : IDeploySnapshots
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISnapshotEnvironment _environment;
        private readonly IDeploySnapshotSteps _stepDeployer;
        private readonly IValidateSnapshotSteps _steps;

        public SnapshotDeployer(
            IDeploySnapshotSteps stepDeployer,
            ISnapshotEnvironment environment,
            IValidateSnapshotSteps steps
            )
        {
            _environment = environment;
            _steps = steps;
            _stepDeployer = stepDeployer;
        }

        public void Deploy(Snapshot snapshot)
        {
            Log.Info($"Snapshot deployment to {_environment.Name} started.");

            var environmentId = _environment.Id;

            foreach (var step in snapshot.Steps.OrderBy(x => x.Index))
            {
                var valid = _steps.Validate(step, environmentId);
                if (valid)
                {
                    _stepDeployer.Deploy(step, environmentId);
                }
                else
                {
                    Log.Info(
                        $"Deployment step {step.Index} project {step.ProjectName} version {step.ReleaseVersion}. Already deployed. Skipping.");
                }
            }

            Log.Info($"Snapshot deployment to {_environment.Name} finished.");
        }
    }
}