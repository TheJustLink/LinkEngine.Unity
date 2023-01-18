using System;
using System.Collections.Generic;

using Object = UnityEngine.Object;

namespace LinkEngine.Unity.Objects
{
    class ObjectWrapper
    {
        public ObjectWrapper(Object nativeObject) { }
    }

    class ObjectWrapper<T> : ObjectWrapper, IEquatable<ObjectWrapper<T>>
        where T : Object
    {
        protected readonly T NativeObject;
        
        public ObjectWrapper(Object nativeObject) : base(nativeObject) => NativeObject = nativeObject as T;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((ObjectWrapper<T>)obj);
        }
        public bool Equals(ObjectWrapper<T> other)
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