using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;
using Octopus.Client.Model;
using System.Threading;
using Octopus.Client;

namespace Tasks.OctopusDeploy.CrossProjectRelease.Deploy.Domain.Services
{
    
    public class ExecutedTaskResourceWaiter : IWaitForExecutedTaskResources
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IStepDeployerConfiguration _configuration;
        private readonly IProvideUtcDateTime _dateTime;
        private readonly IOctopusRepository _repository;
        private readonly HashSet<TaskState> _waitingStates = new HashSet<TaskState>
        {
             TaskState.Executing,
             TaskState.Queued
        };

        public ExecutedTaskResourceWaiter(IOctopusRepository repository, IStepDeployerConfiguration configuration, IProvideUtcDateTime dateTime )
        {
            _repository = repository;
            _configuration = configuration;
            _dateTime = dateTime;
        }

        public TaskResource Wait(TaskResource task)
        {
            var interval = _configuration.Interval;
            var timeout = _configuration.Timeout;
            Log.Info($"Interval {interval} ");
            Log.Info($"Timeout {timeout} ");
            var timeToStop = _dateTime.Now + timeout;
            while (_waitingStates.Contains(task.State))
            {
                if (_dateTime.Now > timeToStop)
                {
                    var message = $"Task creation took too much time";
                    throw new TimeoutException(message);
                }
                Thread.Sleep(interval);

                Log.Debug($"Polling deployment task state .. {task.State}");
                task = _repository.Tasks.Get(task.Id);
            }
            return task;
        }
    }
}