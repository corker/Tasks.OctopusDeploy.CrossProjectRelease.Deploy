using FakeItEasy;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Spec.UnitTests
{
    public class describe_SnapshotDeployer : nspec
    {
        private IDeploySnapshotSteps _stepDeployer;
        private ISnapshotEnvironment _environment;
        private IValidateSnapshotSteps _validator;
        private Domain.Services.SnapshotDeployer _deployer;
        private Snapshot _snapshot;
        private SnapshotStep _snapshotStep1;
        private SnapshotStep _snapshotStep2;

        private void before_each()
        {
            _snapshot = new Snapshot();
            _environment = A.Fake<ISnapshotEnvironment>(x => x.Strict());
            A.CallTo(() => _environment.Id).Returns("environment");
            A.CallTo(() => _environment.Name).Returns("name");
            _stepDeployer = A.Fake<IDeploySnapshotSteps>(x => x.Strict());
            _validator = A.Fake<IValidateSnapshotSteps>(x => x.Strict());
            _deployer = new Domain.Services.SnapshotDeployer(_stepDeployer, _environment, _validator);
        }

        private void when_deploy_a_snapshot()
        {
            act = () => _deployer.Deploy(_snapshot);
            
            context["with a step"] = () =>
            {
                before = () =>
                {
                    _snapshotStep1 = new SnapshotStep
                    {
                        ProjectId = "Project01",
                        ReleaseId = "Release01"
                    };
                    _snapshot.Steps = new[] {_snapshotStep1};
                    A.CallTo(() => _stepDeployer.Deploy(_snapshotStep1, _environment.Id)).DoesNothing();
                    A.CallTo(() => _validator.Validate(_snapshotStep1, _environment.Id)).Returns(true);
                };

                it["should deploy the step"] =
                    () =>
                    {
                        var step = _snapshotStep1;
                        A.CallTo(() => _stepDeployer.Deploy(step, A<string>._)).MustHaveHappened();
                    };


                it["should deploy to a configured environment"] = () =>
                {
                    var id = _environment.Id;
                    A.CallTo(() => _stepDeployer.Deploy(A<SnapshotStep>._, id)).MustHaveHappened();
                };
            };

            context["with two steps"] =
                () =>
                {
                    before = () =>
                    {
                        _snapshotStep1 = new SnapshotStep
                        {
                            Index = 1,
                            ProjectId = "Project01",
                            ReleaseId = "Release01"
                        };
                        _snapshotStep2 = new SnapshotStep
                        {
                            Index = 2,
                            ProjectId = "Project02",
                            ReleaseId = "Release02"
                        };
                        _snapshot.Steps = new[] {_snapshotStep2, _snapshotStep1};
                        A.CallTo(() => _validator.Validate(A<SnapshotStep>._, _environment.Id)).Returns(true);
                        A.CallTo(() => _stepDeployer.Deploy(A<SnapshotStep>._, _environment.Id)).DoesNothing();
                    };
                    it["should deploy in correct order"] = () =>
                    {
                        var id = _environment.Id;
                        A.CallTo(() => _stepDeployer.Deploy(_snapshotStep1, id)).MustHaveHappened()
                            .Then(A.CallTo(() => _stepDeployer.Deploy(_snapshotStep2, id)).MustHaveHappened());
                    };
                };
                
            context["with a step is not valid"] =
                () =>
                {
                    before = () =>
                    {
                        _snapshotStep1 = new SnapshotStep
                        {
                            Index = 1,
                            ProjectId = "Project01",
                            ReleaseId = "Release01"
                        };

                        _snapshot.Steps = new[] {_snapshotStep1};
                        A.CallTo(() => _validator.Validate(A<SnapshotStep>._, _environment.Id)).Returns(false);
                        A.CallTo(() => _stepDeployer.Deploy(A<SnapshotStep>._, _environment.Id)).DoesNothing();
                    };

                    it["should skip the step"] =
                        () =>
                        {
                            A.CallTo(() => _stepDeployer.Deploy(_snapshotStep1, _environment.Id)).MustNotHaveHappened();
                        };


                };
        }
    }
}