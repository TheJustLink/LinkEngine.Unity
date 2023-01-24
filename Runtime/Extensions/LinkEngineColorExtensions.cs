namespace LinkEngine.Unity.Extensions
{
    static class LinkEngineColorExtensions
    {
        public static UnityEngine.Color ToUnityColor(this LinkEngine.Graphics.Color color) =>
            new(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        public static UnityEngine.Color32 ToUnityColor32(this LinkEngine.Graphics.Color color) =>
            new(color.R, color.G, color.B, color.A);
    }
}