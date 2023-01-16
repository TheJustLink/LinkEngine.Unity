using UnityEngine;

using LinkEngine.Components;
using LinkEngine.GameObjects;

namespace Assets.Scripts
{
    class GameObjectWrapper : IGameObject
    {
        public ITransform Transform { get; }

        private readonly GameObject _gameObject;

        public GameObjectWrapper(GameObject gameObject)
        {
            _gameObject = gameObject;

            Transform = new TransformWrapper(gameObject.transform);
        }
    }
}