namespace LinkEngine.Unity.Extensions
{
    static class Vector3Extensions
    {
        public static Math.Vector3 ToLinkEngine(this UnityEngine.Vector3 vector3) =>
            new(vector3.x, vector3.y, vector3.z);
        public static UnityEngine.Vector3 ToUnity(this Math.Vector3 vector3) =>
            new(vector3.X, vector3.Y, vector3.Z);

        public static UnityEngine.Vector2 ToVector2(this UnityEngine.Vector3 vector3) =>
            new(vector3.x, vector3.y);
    }
}