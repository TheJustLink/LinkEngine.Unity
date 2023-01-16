using GameProject;

using UnityEngine;

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