using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;

using LinkEngine.Assets;
using LinkEngine.Unity.Extensions;
using LinkEngine.Unity.Objects;
using LinkEngine.Unity.Threads;

namespace LinkEngine.Unity.Assets
{
    class AssetProvider : IAssetProvider
    {
        public IEnumerable<T> GetMany<T>(string folderPath) where T : class => GetManyAsync<T>(folderPath).Result;
        public T Get<T>(string path) where T : class => GetAsync<T>(path).Result;

        public Task<T> GetAsync<T>(string path) where T : class
        {
            var entry = WrapperProvider.GetWrapperEntry<T>();
            var resource = GetResourceAsync(path, entry.NativeType);

            return resource.ContinueWith(task => entry.CreateWrapper<T>(task.Result));
        }
        public Task<IEnumerable<T>> GetManyAsync<T>(string folderPath) where T : class
        {
            var entry = WrapperProvider.GetWrapperEntry<T>();
            var resources = GetResourcesAsync(folderPath, entry.NativeType);
            
            return resources.ContinueWith(task => task.Result.Select(entry.CreateWrapper<T>));
        }
        
        private Task<Object> GetResourceAsync(string path, System.Type resourceType)
        {
            return UnityThreadDispatcher.Instance
                .InvokeResourceRequestAsync(() => Resources.LoadAsync(path, resourceType))
                .ThrowIfTaskReturnNullAsync<Object, System.IO.FileNotFoundException>($"Resource not found: \"{path}\"({resourceType})");
        }
        private Task<Object[]> GetResourcesAsync(string path, System.Type resourceType)
        {
            return UnityThreadDispatcher.Instance.InvokeAsync(() => Resources.LoadAll(path, resourceType));
        }
    }
}