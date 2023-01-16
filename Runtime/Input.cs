using System.Numerics;

using GameProject;

namespace Assets.Scripts
{
    class Input : IInput<Vector2>
    {
        private Vector2 _input;
        
        public Vector2 Read()
        {
            UnityThreadDispatcher.Instance.InvokeAsync(() =>
            {
                _input = new(
                    UnityEngine.Input.GetAxisRaw("Horizontal"),
                    UnityEngine.Input.GetAxisRaw("Vertical")
                );
            });

            return _input;
        }
    }
}