using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;

using LinkEngine.Assets;
using LinkEngine.Unity.Extensions;
using LinkEngine.Unity.Threads;

namespace LinkEngine.Unity.Assets
{
    class AssetProvider : IAssetProvider
    {
        class WrapperEntry
        {
            public readonly System.Type ResourceType;
            
            private readonly System.Type _type;

            public WrapperEntry(System.Type type)
            {
                _type = type;

                var baseType = type.BaseType;
                if (baseType == null || baseType.IsGenericType == false || baseType.GetGenericTypeDefinition() != typeof(AssetWrapper<>))
                    throw new System.NotImplementedException($"Type {type} does not have base type of type {typeof(AssetWrapper<>)}");
                
                ResourceType = baseType.GetGenericArguments()[0];
            }

            public T CreateWrapper<T>(Object resource) where T : class
            {
                return System.Activator.CreateInstance(_type, args: resource) as T;
            }
        }

        private readonly Dictionary<System.Type, System.Type> _assetWrapperTypes = new()
        {
            { typeof(ISprite), typeof(SpriteWrapper) }
        };
        private readonly Dictionary<System.Type, WrapperEntry> _wrapperEntries = new();

        public IEnumerable<T> GetMany<T>(string folderPath) where T : class => GetManyAsync<T>(folderPath).Result;
        public T Get<T>(string path) where T : class => GetAsync<T>(path).Result;

        public Task<T> GetAsync<T>(string path) where T : class
        {
            var entry = GetWrapperEntry<T>();
            var resource = GetResourceAsync(path, entry.ResourceType);

            return resource.ContinueWith(task => entry.CreateWrapper<T>(task.Result));
        }
        public Task<IEnumerable<T>> GetManyAsync<T>(string folderPath) where T : class
        {
            var entry = GetWrapperEntry<T>();
            var resources = GetResourcesAsync(folderPath, entry.ResourceType);
            
            return resources.ContinueWith(task => task.Result.Select(entry.CreateWrapper<T>));
        }
        
        private WrapperEntry GetWrapperEntry<T>()
        {
            var assetType = typeof(T);
            
            if (_assetWrapperTypes.TryGetValue(assetType, out var wrapperType))
                return GetWrapperEntryBy(wrapperType);

            throw new System.NotImplementedException($"Asset type not supported: {assetType}");
        }
        private WrapperEntry GetWrapperEntryBy(System.Type wrapperType)
        {
            if (_wrapperEntries.TryGetValue(wrapperType, out var entry) == false)
                _wrapperEntries[wrapperType] = entry = new WrapperEntry(wrapperType);

            return entry;
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