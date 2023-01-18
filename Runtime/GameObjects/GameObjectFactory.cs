using System.Threading.Tasks;

using UnityEngine;

using LinkEngine.GameObjects;
using LinkEngine.Unity.Threads;

namespace LinkEngine.Unity.GameObjects
{
    class GameObjectFactory : IGameObjectFactory
    {
        public IGameObject Create() => UnityThreadDispatcher.Instance.Invoke(() =>
        {
            var gameObject = new GameObject();

            return new GameObjectWrapper(gameObject) as IGameObject;
        });
        public Task<IGameObject> CreateAsync() => UnityThreadDispatcher.Instance.InvokeAsync(() =>
        {
            var gameObject = new GameObject();

            return new GameObjectWrapper(gameObject) as IGameObject;
        });
    }
}