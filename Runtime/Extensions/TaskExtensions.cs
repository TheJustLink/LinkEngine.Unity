using System;
using System.Threading.Tasks;

namespace Assets.Scripts.Extensions
{
    static class TaskExtensions
    {
        public static Task<T> ThrowIfTaskReturnNullAsync<T, TException>(this Task<T> task, string message)
            where TException : Exception => task.ContinueWith(task =>
            {
                var result = task.Result;
                
                if (result == null)
                    throw Activator.CreateInstance(typeof(TException), message) as TException;

                return result;
            });
    }
}