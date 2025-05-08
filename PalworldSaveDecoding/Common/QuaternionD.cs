namespace PalworldSaveDecoding
{
    public struct QuaternionD
    {
        /// <summary>The X value of the vector component of the quaternion.</summary>
        public double X;

        /// <summary>The Y value of the vector component of the quaternion.</summary>
        public double Y;

        /// <summary>The Z value of the vector component of the quaternion.</summary>
        public double Z;

        /// <summary>The rotation component of the quaternion.</summary>
        public double W;

        public QuaternionD(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }   
    }
}
