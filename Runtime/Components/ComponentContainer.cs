using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using LinkEngine.Components;
using LinkEngine.Unity.Objects;
using LinkEngine.Unity.Threads;

namespace LinkEngine.Unity.Components
{
    class ComponentContainer : IComponentContainer
    {
        private readonly GameObject _gameObject;
        private readonly Dictionary<Type, IComponent> _components = new();

        public ComponentContainer(GameObject gameObject) => _gameObject = gameObject;

        public T Add<T>() where T : class, IComponent => AddAsync<T>().Result;
        public async Task<T> AddAsync<T>() where T : class, IComponent
        {
            var type = typeof(T);

            if (_components.TryGetValue(type, out var result))
                return result as T;
            
            var wrapperEntry = WrapperProvider.GetWrapperEntry<T>();

            var component = await UnityThreadDispatcher.Instance.InvokeAsync(() =>
            {
                Component nativeComponent;

                if (_gameObject.TryGetComponent(wrapperEntry.NativeType, out var result))
                    nativeComponent = result;
                else
                    nativeComponent = _gameObject.AddComponent(wrapperEntry.NativeType);

                return wrapperEntry.CreateWrapper<T>(nativeComponent);
            }).ConfigureAwait(false);
            
            _components.Add(type, component);

            return component;
        }
        
        public void Remove<T>() where T : class, IComponent
        {
            var type = typeof(T);

            if (_components.TryGetValue(type, out var component) == false)
                return;

            _components.Remove(type);
            component.Destroy();
        }
        
        public T Get<T>() where T : class, IComponent
        {
            if (TryGet<T>(out var component))
                return component;

            throw new KeyNotFoundException($"Component not found: {typeof(T)}");
        }
        public bool TryGet<T>(out T component) where T : class, IComponent
        {
            if (_components.TryGetValue(typeof(T), out var result))
            {
                component = result as T;
                return true;
            }

            component = null;
            return false;
        }
        public bool Has<T>() where T : class, IComponent
        {
            return _components.ContainsKey(typeof(T));
        }
    }
}