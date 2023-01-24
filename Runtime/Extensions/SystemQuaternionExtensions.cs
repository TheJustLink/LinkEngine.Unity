using UnityEngine;

namespace LinkEngine.Unity.Extensions
{
    static class SystemQuaternionExtensions
    {
        public static Quaternion ToUnity(this System.Numerics.Quaternion quaternion) => new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
}