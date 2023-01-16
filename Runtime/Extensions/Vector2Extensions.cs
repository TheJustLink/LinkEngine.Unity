namespace Assets.Scripts.Extensions
{
    static class Vector2Extensions
    {
        public static System.Numerics.Vector2 ToSystem(this UnityEngine.Vector2 vector2) => new(vector2.x, vector2.y);
    }
}