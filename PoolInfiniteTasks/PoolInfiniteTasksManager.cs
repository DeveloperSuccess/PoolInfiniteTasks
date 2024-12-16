using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PoolInfiniteTasks
{
    /// <summary>
    /// Pool Infinite Tasks
    /// </summary>
    public class PoolInfiniteTasksManager
    {
        private readonly Func<CancellationToken, Task> _taskFactory;
        private readonly int _poolCount;
        private readonly object _lockObject = new object();

        private bool _isStarted = false;
        private List<Task> _runningTasks = new List<Task>();

        /// <summary>
        /// Count tasks in progress
        /// </summary>
        public int CountTasksInProgress => _runningTasks.Count;

        /// <summary>
        /// Constructor for creating a new task pool
        /// </summary>
        /// <param name="taskFactory">A delegate with your logic to run in the task pool</param>
        /// <param name="poolCount">Task pool size</param>
        public PoolInfiniteTasksManager(Func<CancellationToken, Task> taskFactory, int poolCount)
        {
            _taskFactory = taskFactory;
            _poolCount = poolCount;
        }

        /// <summary>
        /// Run tasks in a pool
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task Run(CancellationToken cancellationToken = default)
        {
            lock (_lockObject)
            {
                if (_isStarted) return;
                _isStarted = true;
            }

            for (int i = 0; i < _poolCount; i++)
            {
                _runningTasks.Add(_taskFactory(cancellationToken));
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                _runningTasks = _runningTasks.Where(
                    t => !t.IsCanceled
                    && !t.IsFaulted
                    && !t.IsCompleted
                    && !t.IsCompletedSuccessfully).ToList();

                if (_runningTasks.Count < _poolCount)
                    for (int i = 0; _poolCount - _runningTasks.Count != 0; i++)
                        _runningTasks.Add(_taskFactory(cancellationToken));

                await Task.WhenAny(_runningTasks).ConfigureAwait(false);
            }
        }
    }
}
