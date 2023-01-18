using UnityEngine;

using LinkEngine.Components;
using LinkEngine.GameObjects;
using LinkEngine.Unity.Components;

namespace LinkEngine.Unity.GameObjects
{
    class GameObjectWrapper : IGameObject
    {
        public IComponentContainer Components { get; }

        private readonly GameObject _gameObject;

        public GameObjectWrapper(GameObject gameObject)
        {
            _gameObject = gameObject;

            Components = new ComponentContainer(gameObject);
        }
    }
}