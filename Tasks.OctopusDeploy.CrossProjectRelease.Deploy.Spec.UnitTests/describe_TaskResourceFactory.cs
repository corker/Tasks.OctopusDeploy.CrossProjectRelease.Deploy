using FakeItEasy;
using NSpec;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Spec.UnitTests
{
    public class describe_TaskResourceFactory : nspec
    {
        private SnapshotStep _snapshotStep;
        private IOctopusRepository _repository;
        private TaskResourceFactory _factory;
        private TaskResource _actualTaskResource;
        private ITaskRepository _tasks;
        private IDeploymentRepository _deployments;
        private TaskResource _expectedTaskResource;
        
        private void before_each()
        {
            _deployments = A.Fake<IDeploymentRepository>(x => x.Strict());
            _tasks = A.Fake<ITaskRepository>(x => x.Strict());
            _repository = A.Fake<IOctopusRepository>(x => x.Strict());
            A.CallTo(() => _repository.Tasks).Returns(_tasks);
            A.CallTo(() => _repository.Deployments).Returns(_deployments);
            _factory = new TaskResourceFactory(_repository);
        }

        private void when_create_a_task()
        {
            before = () =>
            {
                _snapshotStep = new SnapshotStep();
                _expectedTaskResource = new TaskResource {Id = "Task01"};
                var deploymentResource = new DeploymentResource {TaskId = _expectedTaskResource.Id};
                A.CallTo(() => _deployments.Create(A<DeploymentResource>._)).Returns(deploymentResource);
                A.CallTo(() => _tasks.Get(A<string>._)).Returns(_expectedTaskResource);
            };

            act = () => { _actualTaskResource = _factory.Create(_snapshotStep, "Environment01"); };

            it["should return a task"] = () => { _actualTaskResource.should_be(_expectedTaskResource); };

            it["should create a deployment"] = () =>
            {
                A.CallTo(() => _deployments.Create(A<DeploymentResource>.That.Matches(x =>
                    x.ReleaseId == _snapshotStep.ReleaseId &&
                    x.ProjectId == _snapshotStep.ProjectId &&
                    x.EnvironmentId == "Environment01"
                    ))).MustHaveHappened();
            };
        }
    }
}