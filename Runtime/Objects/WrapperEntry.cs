using UnityEngine;

namespace LinkEngine.Unity.Objects
{
    class WrapperEntry
    {
        public readonly System.Type NativeType;

        public readonly System.Type WrapperType;

        public WrapperEntry(System.Type wrapperType)
        {
            WrapperType = wrapperType;

            var baseType = wrapperType.BaseType;
            if (baseType == null || baseType.IsGenericType == false || baseType.IsAssignableFrom(typeof(ObjectWrapper<>)))
                throw new System.NotImplementedException($"Type {wrapperType} does not have base type of type {typeof(ObjectWrapper<>)}");

            NativeType = baseType.GetGenericArguments()[0];
        }

        public T CreateWrapper<T>(Object nativeObject) where T : class
        {
            return System.Activator.CreateInstance(WrapperType, args: nativeObject) as T;
        }
    }
}