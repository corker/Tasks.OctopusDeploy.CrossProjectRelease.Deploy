using System.Collections.Generic;
using FakeItEasy;
using NSpec;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Spec.UnitTests
{
    internal class describe_SnapshotStepValidator : nspec
    {
        private IValidateSnapshotSteps _validator;
        private IOctopusRepository _repository;
        private IDashboardRepository _dashboard;
        private DashboardResource _dashboardResource;
        private SnapshotStep _snapshotStep;
        private bool _actualResult;

        private void before_each()
        {
            _dashboardResource = new DashboardResource
            {
                Items = new List<DashboardItemResource>
                {
                    new DashboardItemResource
                    {
                        EnvironmentId = "Environment01",
                        ProjectId = "Project01.1",
                        ReleaseId = "Release01.1",
                        State = TaskState.Success
                    },
                    new DashboardItemResource
                    {
                        EnvironmentId = "Environment01",
                        ProjectId = "Project01.2",
                        ReleaseId = "Release01.2",
                        State = TaskState.Failed
                    },
                    new DashboardItemResource
                    {
                        EnvironmentId = "Environment02",
                        ProjectId = "Project02",
                        ReleaseId = "Release02",
                        State = TaskState.Success
                    }
                }
            };

            _dashboard = A.Fake<IDashboardRepository>(x => x.Strict());
            A.CallTo(() => _dashboard.GetDashboard()).Returns(_dashboardResource);

            _repository = A.Fake<IOctopusRepository>(x => x.Strict());
            A.CallTo(() => _repository.Dashboards).Returns(_dashboard);

            _validator = new SnapshotStepValidator(_repository);
        }

        private void when_validate_a_snapshot_step()
        {
            act = () => { _actualResult = _validator.Validate(_snapshotStep, "Environment01"); };

            context["the step already exists and successfully deployed"] = () =>
            {
                before = () => { _snapshotStep = new SnapshotStep {ProjectId = "Project01.1", ReleaseId = "Release01.1"}; };

                it["should be resolved as valid"] = () => { _actualResult.should_be_false(); };
            };

            context["the step already exists and failed to deploy"] = () =>
            {
                before = () => { _snapshotStep = new SnapshotStep {ProjectId = "Project01.2", ReleaseId = "Release01.2"}; };

                it["should be resolved as invalid"] = () => { _actualResult.should_be_true(); };
            };

            context["the step does not exist"] = () =>
            {
                before = () => { _snapshotStep = new SnapshotStep {ProjectId = "SomeName", ReleaseId = "SomeRelease"}; };

                it["should be resolved as invalid"] = () => { _actualResult.should_be_true(); };
            };
        }
    }
}