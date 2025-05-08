namespace PalworldSaveDecoding
{
    public class WorkTransform
    {
        public byte Type { get; private set; }
        public byte V2 { get; private set; }
        public Guid MapObjectInstanceId { get; private set; }
        public Transform Transform { get; private set; }
        public Guid Guid { get; private set; }
        public Guid InstanceId { get; private set; }



        public static WorkTransform Read(GvasFileReader reader)
        {
            var result = new WorkTransform();
            result.Type = reader.ReadByte();
            result.V2 = 0;

            var transformType = reader.ReadByte();
            switch (transformType)
            {
                case 1:
                    result.Transform = reader.ReadTransform();
                    break;
                case 2:
                    result.MapObjectInstanceId = reader.ReadGuid();
                    break;
                case 3:
                    result.Guid = reader.ReadGuid();
                    result.InstanceId = reader.ReadGuid();
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
