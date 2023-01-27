using UnityEngine;

using LinkEngine.Components;
using LinkEngine.Math;
using LinkEngine.Unity.Extensions;
using LinkEngine.Unity.Threads;

using Vector2 = LinkEngine.Math.Vector2;
using Vector3 = LinkEngine.Math.Vector3;
using Quaternion = LinkEngine.Math.Quaternion;

namespace LinkEngine.Unity.Components
{
    class TransformWrapper : ComponentWrapper<Transform>, ITransform
    {
        public bool HasParent => Parent != null;
        
        ITransform2D? ITransform2D.Parent
        {
            get => Parent;
            set => Parent = value as ITransform;
        }
        public ITransform? Parent
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
            get => Position;
            set => Position = Position.WithXY(value);
        }
        float ITransform2D.RotationInRadians
        {
            get => Rotation.EulerInRadians.Z;
            set => Rotation = Rotation.WithEulerZInRadians(value);
        }
        float ITransform2D.RotationInDegrees
        {
            get => Rotation.EulerInDegrees.Z;
            set => Rotation = Rotation.WithEulerZInDegrees(value);
        }

        Vector2 ITransform2D.Scale
        {
            get => Scale;
            set => Scale = Scale.WithXY(value);
        }

        public Vector3 Position
        {
            get => Parent == null ? LocalPosition : LocalPosition + Parent.Position;
            set => LocalPosition = Parent == null ? value : value - Parent.Position;
        }
        public Quaternion Rotation
        {
            get => Parent == null ? LocalRotation : LocalRotation * Parent.Rotation;
            set => LocalRotation = Parent == null ? value : Parent.Rotation.Inversed * value;
        }
        public Vector3 Scale
        {
            get => Parent == null ? LocalScale : LocalScale * Parent.Scale;
            set => LocalScale = Parent == null ? value : value / Parent.Scale;
        }

        Vector2 ITransform2D.LocalPosition
        {
            get => LocalPosition;
            set => LocalPosition = LocalPosition.WithXY(value);
        }
        float ITransform2D.LocalRotationInRadians
        {
            get => LocalRotation.EulerInRadians.Z;
            set => LocalRotation = LocalRotation.WithEulerZInRadians(value);
        }
        float ITransform2D.LocalRotationInDegrees
        {
            get => LocalRotation.EulerInDegrees.Z;
            set => LocalRotation = LocalRotation.WithEulerZInDegrees(value);
        }
        Vector2 ITransform2D.LocalScale
        {
            get => LocalScale;
            set => LocalScale = LocalScale.WithXY(value);
        }

        public Vector3 LocalPosition { get; set; }
        public Quaternion LocalRotation { get; set; }
        public Vector3 LocalScale { get; set; }
        
        private bool _parentChanged;
        private ITransform? _parent;

        public TransformWrapper(Transform nativeObject)
            : base(nativeObject)
        {
            if (NativeObject.parent != null)
                Parent = new TransformWrapper(NativeObject.parent);
            
            LocalPosition = NativeObject.localPosition.ToLinkEngine();
            LocalRotation = NativeObject.localRotation.ToLinkEngine();
            LocalScale = NativeObject.localScale.ToLinkEngine();

            Enable();
        }
        
        protected override void InternalEnable() => UnityThreadDispatcher.Instance.AddSynchronizationJob(SyncWithNative);
        protected override void InternalDisable() => UnityThreadDispatcher.Instance.RemoveSynchronizationJob(SyncWithNative);

        public override void Destroy() => Disable();

        private void SyncWithNative()
        {
            if (_parentChanged)
            {
                _parentChanged = false;
                NativeObject.SetParent((Parent as TransformWrapper)?.NativeObject, false);
            }

            NativeObject.SetLocalPositionAndRotation(LocalPosition.ToUnity(), LocalRotation.ToUnity());
            NativeObject.localScale = LocalScale.ToUnity();
        }
    }
}