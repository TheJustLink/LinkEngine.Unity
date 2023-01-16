using System.Threading;

using UnityEngine;

namespace Assets.Scripts
{
    static class Application
    {
        private static GameProject.Application s_application;
        private static Thread s_appThread;

        [RuntimeInitializeOnLoadMethod]
        public static void Main()
        {
            UnityEngine.Application.quitting += OnStop;
            _ = UnityThreadDispatcher.Instance;

            Debug.Log("Starting");

            var logger = new Logger();
            var objectFactory = new GameObjectFactory();
            var engine = new Engine(logger, objectFactory, new Input(), new AssetProvider());

            StartApplication(engine);
            Debug.Log("Started");
        }

        private static void StartApplication(GameProject.IEngine engine)
        {
            s_appThread = new(() =>
            {
                s_application = new GameProject.Application(engine);
                s_application.Start();
            });
            s_appThread.IsBackground = true;
            s_appThread.Start();
        }

        private static void OnStop()
        {
            UnityEngine.Application.quitting -= OnStop;
            
            if (s_appThread.IsAlive)
            {
                s_appThread.Abort();

                Debug.LogError("Application initialize thread aborted");
            }

            s_application?.Stop();
        }
    }
}