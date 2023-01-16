using System.Threading.Tasks;

using GameProject;

using UnityEngine;

namespace Assets.Scripts
{
    class GameObjectFactory : IGameObjectFactory
    {
        public IGameObject Create() => UnityThreadDispatcher.Instance.Invoke(() =>
        {
            var gameObject = new GameObject();

            return (IGameObject)new GameObjectWrapper(gameObject);
        });
        public Task<IGameObject> CreateAsync() => UnityThreadDispatcher.Instance.InvokeAsync(() =>
        {
            var gameObject = new GameObject();

            return (IGameObject)new GameObjectWrapper(gameObject);
        });
    }
}