namespace LinkEngine.Unity.Extensions
{
    static class QuaternionExtensions
    {
        public static Math.Quaternion ToLinkEngine(this UnityEngine.Quaternion quaternion) =>
            new(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        public static UnityEngine.Quaternion ToUnity(this Math.Quaternion quaternion) =>
            new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
}