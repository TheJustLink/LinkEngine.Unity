using GameProject;

using UnityEngine;

namespace Assets.Scripts
{
    class SpriteWrapper : AssetWrapper<Sprite>, ISprite
    {
        public bool Packed => NativeObject.packed;
        
        public SpriteWrapper(Object nativeObject) : base(nativeObject) { }
    }
}