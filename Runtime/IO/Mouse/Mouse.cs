using LinkEngine.IO.Mouse;
using LinkEngine.Unity.Extensions;
using LinkEngine.Unity.Threads;

using UnityEngine;

using Vector2 = System.Numerics.Vector2;

namespace LinkEngine.Unity.IO.Mouse
{
    public class Mouse : IMouse
    {
        private const int AdditionalButtonsCount = 4;
        
        public bool Enabled { get; private set; }

        public Vector2 PositionInScreen { get; private set; }
        public Vector2 PositionInWorld { get; private set; }

        public IMouseButton LeftButton { get; }
        public IMouseButton RightButton { get; }

        public IMouseScrollButton ScrollButton { get; }

        public IMouseButton[] AdditionalButtons { get; }
        public IMouseButton[] AllButtons { get; }

        private readonly Camera _camera;

        public Mouse()
        {
            _camera = Camera.main;

            LeftButton = new MouseButton(0);
            RightButton = new MouseButton(1);
            ScrollButton = new MouseScrollButton(2);

            AllButtons = new IMouseButton[3 + AdditionalButtonsCount];
            AllButtons[0] = LeftButton;
            AllButtons[1] = RightButton;
            AllButtons[2] = ScrollButton;

            AdditionalButtons = new IMouseButton[AdditionalButtonsCount];

            for (var i = 0; i < AdditionalButtonsCount; i++)
            {
                var button = new MouseButton(i + 3);

                AdditionalButtons[i] = button;
                AllButtons[i + 3] = button;
            }

            UnityThreadDispatcher.Instance.AddSynchronizationJob(SyncWithNative);
        }

        private void SyncWithNative()
        {
            Enabled = UnityEngine.Input.mousePresent;

            var mousePositionInScreen = UnityEngine.Input.mousePosition;
            var mousePositionInWorld = _camera.ScreenToWorldPoint(mousePositionInScreen);

            PositionInScreen = mousePositionInScreen.ToVector2().ToSystem();
            PositionInWorld = mousePositionInWorld.ToVector2().ToSystem();

            for (var i = 0; i < AllButtons.Length; i++)
                (AllButtons[i] as MouseButton).SyncWithNative();
        }
    }
}