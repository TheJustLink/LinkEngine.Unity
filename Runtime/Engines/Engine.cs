using System.Numerics;

using LinkEngine.Assets;
using LinkEngine.Engines;
using LinkEngine.GameObjects;
using LinkEngine.IO;
using LinkEngine.Logs;

namespace LinkEngine.Unity.Engines
{
    class Engine : IEngine
    {
        public ILogger Logger { get; }
        public IInput<Vector2> Input { get; }
        public IGameObjectFactory GameObjectFactory { get; }
        public IAssetProvider AssetProvider { get; }

        public Engine(ILogger logger, IGameObjectFactory gameObjectFactory, IInput<Vector2> input, IAssetProvider assetProvider)
        {
            Logger = logger;
            GameObjectFactory = gameObjectFactory;
            Input = input;
            AssetProvider = assetProvider;
        }
    }
}