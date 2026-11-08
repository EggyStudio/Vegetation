using System.Numerics;

namespace Vegetation;

// What the growth asks of a vector beyond what System.Numerics gives, a way that is never NaN.
internal static class Vectors
{
    extension(Vector3 vector)
    {
        // The vector at unit length, or along Z where it has no length, so a stem whose ways cancel
        // turns along an axis rather than filling the skeleton with NaN, which a level would keep.
        public Vector3 Normalized
        {
            get
            {
                var length = MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
                return length > 0f ? vector * (1f / length) : Vector3.UnitZ;
            }
        }
    }
}
