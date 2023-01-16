﻿using GameProject;

namespace Assets.Scripts
{
    class Logger : ILogger
    {
        public void Write(string message) => UnityEngine.Debug.Log(message);
        public void WriteWarning(string message) => UnityEngine.Debug.LogWarning(message);
        public void WriteError(string message) => UnityEngine.Debug.LogError(message);
    }
}