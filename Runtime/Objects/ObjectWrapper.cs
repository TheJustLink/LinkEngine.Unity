using System;
using System.Collections.Generic;

using Object = UnityEngine.Object;

namespace LinkEngine.Unity.Objects
{
    class ObjectWrapper
    {
        protected ObjectWrapper(Object? nativeObject) { }
    }
    
    class ObjectWrapper<TNative> : ObjectWrapper, IEquatable<ObjectWrapper<TNative>>
        where TNative : Object
    {
        protected static TManaged Wrap<TManaged>(Object @object) where TManaged : class =>
            WrapperProvider.CreateWrapper<TManaged>(@object);
        protected static TNative Unwrap<TManaged>(TManaged @object) =>
            (@object as ObjectWrapper<TNative>)!.NativeObject;

        public readonly TNative NativeObject;

        protected ObjectWrapper(Object? nativeObject) : base(nativeObject) => NativeObject = (nativeObject as TNative)!;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((ObjectWrapper<TNative>)obj);
        }
        public bool Equals(ObjectWrapper<TNative> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return EqualityComparer<TNative>.Default.Equals(NativeObject, other.NativeObject);
        }
        public override int GetHashCode()
        {
            return EqualityComparer<TNative>.Default.GetHashCode(NativeObject);
        }
    }
}