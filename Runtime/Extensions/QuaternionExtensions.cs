namespace Assets.Scripts.Extensions
{
    static class QuaternionExtensions
    {
        public static System.Numerics.Quaternion ToSystem(this UnityEngine.Quaternion quaternion) => new(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }
}