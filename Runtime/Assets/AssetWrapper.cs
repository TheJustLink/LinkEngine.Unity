using System;
using System.Collections.Generic;

using Object = UnityEngine.Object;

namespace LinkEngine.Unity.Assets
{
    class AssetWrapper
    {
        public AssetWrapper(Object nativeObject) { }
    }

    class AssetWrapper<T> : AssetWrapper, IEquatable<AssetWrapper<T>>
        where T : Object
    {
        protected readonly T NativeObject;

        public AssetWrapper(Object nativeObject) : base(nativeObject) => NativeObject = nativeObject as T;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AssetWrapper<T>) obj);
        }
        public bool Equals(AssetWrapper<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(NativeObject, other.NativeObject);
        }
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(NativeObject);
        }
    }
}