using System;
using FakeItEasy;
using Octopus.Client.Model;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Models;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Spec.UnitTests
{
    internal class describe_SnapshotStepDeployer : nspec
    {
        private SnapshotStepDeployer _deployer;
        private ICreateTaskResources _taskFactory;
        private IWaitForExecutedTaskResources _taskWaiter;
        private TaskResource _task;
        private SnapshotStep _step;
        private string _environmentId;

        private void before_each()
        {
            _environmentId = "EnvironmentId01";
            _taskFactory = A.Fake<ICreateTaskResources>(x => x.Strict());
            _taskWaiter = A.Fake<IWaitForExecutedTaskResources>(x => x.Strict());
            _deployer = new SnapshotStepDeployer(_taskFactory, _taskWaiter);
            _step = new SnapshotStep();
        }

        private void when_deploy_a_snapshot_step()
        {
            act = () => { _deployer.Deploy(_step, _environmentId); };

            context["and task executed successfully"] = () =>
            {
                before = () =>
                {
                    _task = new TaskResource {State = TaskState.Success};
                    A.CallTo(() => _taskFactory.Create(A<SnapshotStep>._, A<string>._)).Returns(_task);
                    A.CallTo(() => _taskWaiter.Wait(A<TaskResource>._)).Returns(_task);
                };

                it["should create a task"] =
                    () => { A.CallTo(() => _taskFactory.Create(_step, _environmentId)).MustHaveHappened(); };

                it["should wait for task to complete"] =
                    () => { A.CallTo(() => _taskWaiter.Wait(_task)).MustHaveHappened(); };
            };

            context["and task failed to execute"] = () =>
            {
                before = () =>
                {
                    A.CallTo(() => _taskFactory.Create(A<SnapshotStep>._, A<string>._)).Returns(null);

                    var taskResource = new TaskResource {State = TaskState.Failed};
                    A.CallTo(() => _taskWaiter.Wait(A<TaskResource>._)).Returns(taskResource);
                };

                it["should throw index out of range exception"] = expect<IndexOutOfRangeException>();
            };
        }
    }
}