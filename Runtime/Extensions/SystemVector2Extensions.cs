namespace LinkEngine.Unity.Extensions
{
    static class SystemVector2Extensions
    {
        public static UnityEngine.Vector2 ToUnity(this System.Numerics.Vector2 vector2) => new(vector2.X, vector2.Y);
        public static System.Numerics.Vector3 To3D(this System.Numerics.Vector2 vector2) => new(vector2.X, vector2.Y, 0);
        public static System.Numerics.Vector3 To3DScale(this System.Numerics.Vector2 vector2) => new(vector2.X, vector2.Y, 1);
    }
}