using System.Threading;

using UnityEngine;

using LinkEngine.Unity.Engines;
using LinkEngine.Unity.Threads;
using LinkEngine.Unity.GameObjects;
using LinkEngine.Unity.Assets;
using LinkEngine.Unity.IO.Mouse;

using ThreadPriority = System.Threading.ThreadPriority;
using Input = LinkEngine.Unity.IO.Input;
using Logger = LinkEngine.Unity.Logs.Logger;

namespace LinkEngine.Unity
{
    static class UnityApplication
    {
        private static Thread s_appThread;
        private static Engine s_engine;
        
        [RuntimeInitializeOnLoadMethod]
        public static void Main()
        {
            Application.quitting += OnStop;
            _ = UnityThreadDispatcher.Instance;

            Debug.Log("Starting");

            s_engine = new Engine(
                new Logger(),
                new Mouse(),
                new GameObjectFactory(),
                new Input(),
                new AssetProvider()
            );

            StartApplication(s_engine);
            Debug.Log("Started");
        }

        private static void StartApplication(LinkEngine.Engines.IEngine engine)
        {
            s_appThread = new(() => LinkEngine.EntryPoint.Enter(engine));
            s_appThread.IsBackground = true;
            s_appThread.Priority = ThreadPriority.Highest;
            s_appThread.Start();
        }

        private static void OnStop()
        {
            Application.quitting -= OnStop;
            
            if (s_appThread.IsAlive)
            {
                s_appThread.Abort();

                Debug.LogError("Application initialize thread aborted");
            }
            
            s_engine?.Stop();
        }
    }
}