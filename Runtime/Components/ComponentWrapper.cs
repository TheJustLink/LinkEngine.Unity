using UnityEngine;

using LinkEngine.Components;
using LinkEngine.Unity.Objects;
using LinkEngine.Unity.Threads;

namespace LinkEngine.Unity.Components
{
    interface IComponentWrapper : IComponent
    {
        void Destroy();
    }
    abstract class ComponentWrapper<TNative> : ObjectWrapper<TNative>, IComponentWrapper where TNative : Object
    {
        public bool IsEnabled { get; private set; }

        protected ComponentWrapper(Object nativeObject) : base(nativeObject) { }

        public void Enable()
        {
            if (IsEnabled) return;

            InternalEnable();
            IsEnabled = true;
        }
        public void Disable()
        {
            if (IsEnabled == false) return;

            InternalDisable();
            IsEnabled = false;
        }

        protected abstract void InternalEnable();
        protected abstract void InternalDisable();

        public virtual void Destroy() => UnityThreadDispatcher.Instance.InvokeAsync(() => Object.DestroyImmediate(NativeObject));
    }
}