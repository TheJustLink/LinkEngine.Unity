using UnityEngine;

using LinkEngine.Assets;
using LinkEngine.Unity.Objects;

namespace LinkEngine.Unity.Assets
{
    class SpriteWrapper : ObjectWrapper<Sprite>, ISprite
    {
        public bool Packed => NativeObject.packed;
        
        public SpriteWrapper(Object nativeObject) : base(nativeObject) { }
    }
}