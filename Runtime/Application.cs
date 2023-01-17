﻿using System.Threading;

using UnityEngine;

using LinkEngine.Unity.Engines;
using LinkEngine.Unity.Threads;
using LinkEngine.Unity.GameObjects;
using LinkEngine.Unity.Assets;

using Input = LinkEngine.Unity.IO.Input;
using Logger = LinkEngine.Unity.Logs.Logger;

namespace LinkEngine.Unity
{
    static class Application
    {
        private static LinkEngine.Application s_application;
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

        private static void StartApplication(LinkEngine.Engines.IEngine engine)
        {
            s_appThread = new(() =>
            {
                s_application = new LinkEngine.Application(engine);
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