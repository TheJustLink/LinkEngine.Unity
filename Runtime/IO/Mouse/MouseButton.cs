using LinkEngine.IO.Mouse;

namespace LinkEngine.Unity.IO.Mouse
{
    class MouseButton : IMouseButton
    {
        public bool IsDown { get; private set; }

        private readonly int _number;

        public MouseButton(int number) => _number = number;

        public virtual void SyncWithNative() => IsDown = UnityEngine.Input.GetMouseButton(_number);
    }
}