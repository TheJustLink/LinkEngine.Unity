namespace LinkEngine.Unity.Extensions
{
    static class ColorExtensions
    {
        public static UnityEngine.Color ToUnityColor(this Graphics.Color color) =>
            new(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        public static UnityEngine.Color32 ToUnityColor32(this Graphics.Color color) =>
            new(color.R, color.G, color.B, color.A);
    }
}