namespace PalworldSaveDecoding
{
    public class WorkableBounds
    {
        public Vector3D Location { get; private set; }
        public QuaternionD Rotation { get; private set; }
        public (Vector3D Origin, Vector3D BoxExtent, double SphereRadius) BoxSphereBounds { get; private set; }

        


        public static WorkableBounds Read(GvasFileReader reader)
        {
            var result = new WorkableBounds();
            result.Location = reader.ReadVector3D();
            result.Rotation = reader.ReadQuaternionD();
            result.BoxSphereBounds = (reader.ReadVector3D(), reader.ReadVector3D(), reader.ReadDouble());
            return result;
        }
    }
}
