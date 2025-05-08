namespace PalworldSaveDecoding
{
    public struct Transform
    {
        public QuaternionD Rotation;
        public Vector3D Translation;
        public Vector3D Scale3d;

        public Transform(QuaternionD rotation, Vector3D translation, Vector3D scale3d)
        {
            Rotation = rotation;
            Translation = translation;
            Scale3d = scale3d;
        }
    }
}
