using LinkEngine.IO;
using LinkEngine.Math;
using LinkEngine.Unity.Threads;

namespace LinkEngine.Unity.IO
{
    class Input : IInput<Vector2>
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";

        private Vector2 _input;

        public Input()
        {
            UnityThreadDispatcher.Instance.AddSynchronizationJob(() => _input = new Vector2(
                UnityEngine.Input.GetAxisRaw(HorizontalAxisName),
                UnityEngine.Input.GetAxisRaw(VerticalAxisName)
            ));
        }

        public Vector2 Read() => _input;
    }
}