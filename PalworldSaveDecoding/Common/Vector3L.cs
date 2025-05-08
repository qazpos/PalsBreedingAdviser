namespace PalworldSaveDecoding
{
    public struct Vector3L
    {
        // <summary>The X component of the vector.</summary>
        public long X;

        /// <summary>The Y component of the vector.</summary>
        public long Y;

        /// <summary>The Z component of the vector.</summary>
        public long Z;

        public Vector3L(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
