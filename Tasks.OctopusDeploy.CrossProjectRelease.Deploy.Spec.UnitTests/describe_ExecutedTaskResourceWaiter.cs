using System;
using System.Linq;
using FakeItEasy;
using NSpec;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Spec.UnitTests
{
    internal class describe_ExecutedTaskResourceWaiter : nspec
    {
        private ExecutedTaskResourceWaiter _waiter;
        private IStepDeployerConfiguration _configuration;
        private IOctopusRepository _repository;
        private IProvideUtcDateTime _dateTime;

        private ITaskRepository _taskRepository;
        private TaskResource _originalTask;
        private TaskResource _actualTask;
        private DateTime[] _dateTimeSequence;
        private TaskResource _finishedTask;
        private const string ExpectedTaskId = "Task01";

        private void before_each()
        {
            _configuration = A.Fake<IStepDeployerConfiguration>(x => x.Strict());
            A.CallTo(() => _configuration.Interval).Returns(TimeSpan.FromTicks(1));
            A.CallTo(() => _configuration.Timeout).Returns(TimeSpan.FromTicks(3));

            _dateTime = A.Fake<IProvideUtcDateTime>();

            _repository = A.Fake<IOctopusRepository>(x => x.Strict());

            _waiter = new ExecutedTaskResourceWaiter(_repository, _configuration, _dateTime);
        }

        private static DateTime[] CreateDateTimeSequence(int count)
        {
            var now = DateTime.UtcNow;
            return Enumerable.Range(0, count).Select(x => now + TimeSpan.FromTicks(x)).ToArray();
        }

        private void when_wait_for_task()
        {
            act = () => { _actualTask = _waiter.Wait(_originalTask); };

            context["that is finished"] = () =>
            {
                before = () =>
                {
                    _originalTask = new TaskResource {Id = ExpectedTaskId, State = TaskState.Canceled};
                    _dateTimeSequence = CreateDateTimeSequence(1);
                    A.CallTo(() => _dateTime.Now).ReturnsNextFromSequence(_dateTimeSequence);
                };

                it["should return this task"] = () => { _actualTask.should_be_same(_originalTask); };
            };


            context["that is not finished"] = () =>
            {
                before = () =>
                {
                    _taskRepository = A.Fake<ITaskRepository>(x => x.Strict());
                    A.CallTo(() => _repository.Tasks).Returns(_taskRepository);
                };

                context["and task queued"] = () =>
                {
                    before = () =>
                    {
                        _originalTask = new TaskResource {Id = ExpectedTaskId, State = TaskState.Queued};
                        _finishedTask = new TaskResource {Id = ExpectedTaskId, State = TaskState.Success};
                        TaskResource[] tasks =
                        {
                            new TaskResource {Id = ExpectedTaskId, State = TaskState.Queued},
                            new TaskResource {Id = ExpectedTaskId, State = TaskState.Executing},
                            new TaskResource {Id = ExpectedTaskId, State = TaskState.Executing},
                            _finishedTask,
                            _finishedTask
                        };
                        _dateTimeSequence = CreateDateTimeSequence(4);
                        A.CallTo(() => _taskRepository.Get(ExpectedTaskId)).ReturnsNextFromSequence(tasks);
                    };

                    it["should return finished task"] = () => { _actualTask.should_be_same(_finishedTask); };
                };

                context["and task executing"] = () =>
                {
                    before = () =>
                    {
                        _originalTask = new TaskResource {Id = ExpectedTaskId, State = TaskState.Executing};
                        _finishedTask = new TaskResource {Id = ExpectedTaskId, State = TaskState.Failed};
                        TaskResource[] tasks =
                        {
                            new TaskResource {Id = ExpectedTaskId, State = TaskState.Executing},
                            _finishedTask
                        };
                        _dateTimeSequence = CreateDateTimeSequence(2);
                        A.CallTo(() => _taskRepository.Get(ExpectedTaskId)).ReturnsNextFromSequence(tasks);
                    };

                    it["should return finished task"] = () => { _actualTask.should_be_same(_finishedTask); };
                };
            };
        }
    }
}