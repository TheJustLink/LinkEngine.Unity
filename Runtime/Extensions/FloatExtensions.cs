namespace Assets.Scripts.Extensions
{
    static class FloatExtensions
    {
        public static System.Numerics.Quaternion To3DRotation(this float value) => System.Numerics.Quaternion.CreateFromYawPitchRoll(0, 0, value);
    }
}