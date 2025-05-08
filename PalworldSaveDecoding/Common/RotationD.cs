namespace PalworldSaveDecoding
{
    public struct RotationD
    {
        public double Pitch;
        public double Yaw;
        public double Roll;

        public RotationD(double pitch, double yaw, double roll)
        {
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
        }
    }
}
