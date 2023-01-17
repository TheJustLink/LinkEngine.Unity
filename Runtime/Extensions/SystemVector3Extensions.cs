namespace LinkEngine.Unity.Extensions
{
    static class SystemVector3Extensions
    {
        public static UnityEngine.Vector3 ToUnity(this System.Numerics.Vector3 vector3) => new(vector3.X, vector3.Y, vector3.Z);
        public static System.Numerics.Vector2 To2D(this System.Numerics.Vector3 vector3) => new(vector3.X, vector3.Y);
    }
}