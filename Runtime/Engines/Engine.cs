using System;
using System.Numerics;

using LinkEngine.Assets;
using LinkEngine.Engines;
using LinkEngine.GameObjects;
using LinkEngine.IO;
using LinkEngine.IO.Mouse;
using LinkEngine.Logs;

namespace LinkEngine.Unity.Engines
{
    class Engine : IEngine
    {
        public event Action Stopped;
        
        public ILogger Logger { get; }
        public IMouse Mouse { get; }
        public IInput<Vector2> Input { get; }
        public IGameObjectFactory GameObjectFactory { get; }
        public IAssetProvider AssetProvider { get; }

        public Engine(ILogger logger, IMouse mouse, IGameObjectFactory gameObjectFactory, IInput<Vector2> input, IAssetProvider assetProvider)
        {
            Logger = logger;
            Mouse = mouse;
            GameObjectFactory = gameObjectFactory;
            Input = input;
            AssetProvider = assetProvider;
        }

        public void Stop() => Stopped?.Invoke();
    }
}