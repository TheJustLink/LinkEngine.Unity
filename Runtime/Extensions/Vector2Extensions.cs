namespace LinkEngine.Unity.Extensions
{
    static class Vector2Extensions
    {
        public static Math.Vector2 ToLinkEngine(this UnityEngine.Vector2 vector2) =>
            new(vector2.x, vector2.y);
        public static UnityEngine.Vector2 ToUnity(this Math.Vector2 vector2) =>
            new(vector2.X, vector2.Y);
    }
}