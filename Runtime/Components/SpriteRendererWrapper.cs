using UnityEngine;

using LinkEngine.Assets;
using LinkEngine.Components;
using LinkEngine.Unity.Assets;
using LinkEngine.Unity.Extensions;
using LinkEngine.Unity.Threads;

using Color = LinkEngine.Graphics.Color;

namespace LinkEngine.Unity.Components
{
    class SpriteRendererWrapper : ComponentWrapper<SpriteRenderer>, ISpriteRenderer
    {
        public ISprite? Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                _spriteChanged = true;
            }
        }
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _colorChanged = true;
            }
        }

        private Color _color;
        private ISprite? _sprite;

        private bool _spriteChanged;
        private bool _colorChanged;

        public SpriteRendererWrapper(Object nativeObject) : base(nativeObject)
        {
            Sprite = SpriteWrapper.Wrap<ISprite>(NativeObject.sprite);

            Enable();
        }

        protected override void InternalEnable() => UnityThreadDispatcher.Instance.AddSynchronizationJob(SyncWithNative);
        protected override void InternalDisable() => UnityThreadDispatcher.Instance.RemoveSynchronizationJob(SyncWithNative);

        private void SyncWithNative()
        {
            if (IsEnabled != NativeObject.enabled)
                NativeObject.enabled = IsEnabled;

            if (_spriteChanged)
            {
                NativeObject.sprite = Sprite == null ? null
                    : SpriteWrapper.Unwrap(Sprite);

                _spriteChanged = false;
            }

            if (_colorChanged)
            {
                NativeObject.color = _color.ToUnityColor();

                _colorChanged = false;
            }
        }
    }
}