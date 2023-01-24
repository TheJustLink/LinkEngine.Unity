namespace LinkEngine.Unity.Extensions
{
    static class Vector3Extensions
    {
        public static System.Numerics.Vector3 ToSystem(this UnityEngine.Vector3 vector3) =>
            new(vector3.x, vector3.y, vector3.z);

        public static UnityEngine.Vector2 ToVector2(this UnityEngine.Vector3 vector3) =>
            new(vector3.x, vector3.y);
    }
}