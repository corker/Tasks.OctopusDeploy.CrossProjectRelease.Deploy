using System.Linq;
using System.Reflection;
using log4net;
using Octopus.Client;
using Octopus.Client.Model;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    public class SnapshotStepValidator : IValidateSnapshotSteps
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IOctopusRepository _repository;

        public SnapshotStepValidator(IOctopusRepository repository)
        {
            _repository = repository;
        }

        public bool Validate(SnapshotStep step, string environmentId)
        {
           var itemResources = _repository.Dashboards.GetDashboard().Items;

           var deployed = itemResources.Any(
                x =>
                    x.EnvironmentId == environmentId &&
                    x.ProjectId == step.ProjectId &&
                    x.ReleaseId == step.ReleaseId && 
                    x.State == TaskState.Success);

            return !deployed;
        }
    }
}