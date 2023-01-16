using UnityEngine;

namespace Assets.Scripts.Extensions
{
    static class SystemQuaternionExtensions
    {
        public static Quaternion ToUnity(this System.Numerics.Quaternion quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        public static float To2D(this System.Numerics.Quaternion quaternion) => quaternion.ToUnity().eulerAngles.z;
    }
}