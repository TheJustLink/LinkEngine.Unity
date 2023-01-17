using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace LinkEngine.Unity.Threads
{
    class UnityThreadDispatcher : MonoBehaviour
    {
        public static UnityThreadDispatcher Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;

                s_instance = new GameObject($"[{nameof(UnityThreadDispatcher)}]")
                    .AddComponent<UnityThreadDispatcher>();

                DontDestroyOnLoad(s_instance);

                return s_instance;
            }
        }
        private static UnityThreadDispatcher s_instance;

        private readonly ConcurrentQueue<Task> _updateTasks = new();
        private readonly List<Action> _synchronizationJobs = new();

        private Thread _mainThread;

        private void Awake() => _mainThread = Thread.CurrentThread;

        public void Invoke(Action action)
        {
            var task = InvokeAsync(action);
            
            task.Wait();
        }
        public T Invoke<T>(Func<T> func)
        {
            var task = InvokeAsync(func);

            return task.Result;
        }

        public Task InvokeAsync(Action action)
        {
            var task = new Task(action);

            if (Thread.CurrentThread == _mainThread)
                task.RunSynchronously();
            else _updateTasks.Enqueue(task);

            return task;
        }
        public Task<T> InvokeAsync<T>(Func<T> func)
        {
            var task = new Task<T>(func);

            if (Thread.CurrentThread == _mainThread)
                task.RunSynchronously();
            else _updateTasks.Enqueue(task);

            return task;
        }
        public Task<UnityEngine.Object> InvokeResourceRequestAsync(Func<ResourceRequest> func)
        {
            var completionSource = new TaskCompletionSource<UnityEngine.Object>();

            InvokeAsync(func).ContinueWith(task =>
            {
                var operation = task.Result;
                operation.completed += _ => completionSource.SetResult(operation.asset);
            }, TaskContinuationOptions.ExecuteSynchronously);

            return completionSource.Task;
        }

        public void AddSynchronizationJob(Action action)
        {
            _synchronizationJobs.Add(action);
        }
        public void RemoveSynchronizationJob(Action action)
        {
            if (_synchronizationJobs.Remove(action))
                return;

            throw new InvalidOperationException($"Synchronization job with {action} does not exist");
        }

        private void Update()
        {
            UpdateTasks();
            UpdateSynchronizationJobs();
        }
        private void UpdateSynchronizationJobs()
        {
            if (_synchronizationJobs.Count == 0) return;
            
            for (var i = 0; i < _synchronizationJobs.Count; i++)
                _synchronizationJobs[i]();
        }
        private void UpdateTasks()
        {
            if (_updateTasks.IsEmpty) return;
            
            while (_updateTasks.TryDequeue(out var task))
                task.RunSynchronously();
        }
    }
}