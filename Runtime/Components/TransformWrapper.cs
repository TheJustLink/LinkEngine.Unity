using UnityEngine;

using LinkEngine.Components;
using LinkEngine.Unity.Extensions;
using LinkEngine.Unity.Objects;
using LinkEngine.Unity.Threads;

using Quaternion = System.Numerics.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;

namespace LinkEngine.Unity.Components
{
    class TransformWrapper : ObjectWrapper<Transform>, ITransform
    {
        public bool HasParent => Parent != null;

        public ITransform Parent
        {
            get => _parent;
            set
            {
                if (_parent == value)
                    return;
                
                if (_parent == null)
                    LocalPosition += value.Position;
                else if (value == null)
                    LocalPosition -= _parent.Position;
                else LocalPosition += value.Position - _parent.Position;

                _parent = value;
                
                _parentChanged = true;
            }
        }

        Vector2 ITransform2D.Position
        {
            get => Position.To2D();
            set => Position = value.To3D();
        }
        float ITransform2D.Rotation
        {
            get => Rotation.To2D();
            set => Rotation = value.To3DRotation();
        }
        Vector2 ITransform2D.Scale
        {
            get => Scale.To2D();
            set => Scale = value.To3D();
        }

        public Vector3 Position
        {
            get => Parent == null ? LocalPosition : LocalPosition + Parent.Position;
            set => LocalPosition = Parent == null ? value : value - Parent.Position;
        }
        public Quaternion Rotation
        {
            get => Parent == null ? LocalRotation : LocalRotation * Parent.Rotation;
            set => LocalRotation = Parent == null ? value : Quaternion.Inverse(Parent.Rotation) * value;
        }
        public Vector3 Scale
        {
            get => Parent == null ? LocalScale : LocalScale * Parent.Scale;
            set => LocalScale = Parent == null ? value : value / Parent.Scale;
        }

        Vector2 ITransform2D.LocalPosition
        {
            get => LocalPosition.To2D();
            set => LocalPosition = value.To3D();
        }
        float ITransform2D.LocalRotation
        {
            get => LocalRotation.To2D();
            set => LocalRotation = value.To3DRotation();
        }
        Vector2 ITransform2D.LocalScale
        {
            get => LocalScale.To2D();
            set => LocalScale = value.To3DScale();
        }

        public Vector3 LocalPosition { get; set; }
        public Quaternion LocalRotation { get; set; }
        public Vector3 LocalScale { get; set; }
        
        private bool _parentChanged;
        private ITransform _parent;

        public TransformWrapper(Transform transform)
            : base(transform)
        {
            if (transform.parent != null)
                Parent = new TransformWrapper(transform.parent);
            
            LocalPosition = transform.localPosition.ToSystem();
            LocalRotation = transform.localRotation.ToSystem();
            LocalScale = transform.localScale.ToSystem();

            UnityThreadDispatcher.Instance.AddSynchronizationJob(() =>
            {
                if (_parentChanged)
                {
                    _parentChanged = false;
                    transform.SetParent((Parent as TransformWrapper)?.NativeObject, false);
                }

                transform.SetLocalPositionAndRotation(LocalPosition.ToUnity(), LocalRotation.ToUnity());
                transform.localScale = LocalScale.ToUnity();
            });
        }

        public void Destroy() => UnityThreadDispatcher.Instance.InvokeAsync(() => Object.Destroy(NativeObject));
    }
}