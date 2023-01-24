using LinkEngine.IO.Mouse;

namespace LinkEngine.Unity.IO.Mouse
{
    class MouseScrollButton : MouseButton, IMouseScrollButton
    {
        public float ScrollDelta { get; private set; }

        public MouseScrollButton(int number) : base(number) { }

        public override void SyncWithNative()
        {
            base.SyncWithNative();

            ScrollDelta = UnityEngine.Input.mouseScrollDelta.y;
        }
    }
}