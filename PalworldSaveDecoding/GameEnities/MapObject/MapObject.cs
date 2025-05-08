using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class MapObject
    {
        public Vector3D WorldLocation { get; private set; }
        public QuaternionD WorldRotation { get; private set; }
        public Vector3D WorldScale3D { get; private set; }
        public string? MapObjectId { get; private set; }
        public Guid MapObjectInstanceId { get; private set; }
        public Guid MapObjectConcreteModelInstanceId { get; private set; }
        public MapObjectModel Model { get; private set; } = new();
        public MapObjectConcreteModel ConcreteModel { get; private set; } = new();




        public static MapObject Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new MapObject();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "WorldLocation":
                        result.WorldLocation = StructProperty.ReadSP(reader, reader.ReadVector3D); break;
                    case "WorldRotation":
                        result.WorldRotation = StructProperty.ReadSP(reader, reader.ReadQuaternionD); break;
                    case "WorldScale3D":
                        result.WorldScale3D = StructProperty.ReadSP(reader, reader.ReadVector3D); break;
                    case "MapObjectId":
                        result.MapObjectId = reader.ReadStringProperty(); break;
                    case "MapObjectInstanceId":
                        result.MapObjectInstanceId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "MapObjectConcreteModelInstanceId":
                        result.MapObjectInstanceId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "Model":
                        result.Model = MapObjectModel.Read(reader, messages); break;
                    case "ConcreteModel":
                        if (result.MapObjectId == null)
                            throw new InvalidDataException("Unknown MapObject structure. Reading ConcreteModel without MapObjectId");
                        result.ConcreteModel = MapObjectConcreteModel.Read(reader, result.MapObjectId, messages);
                        break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown MapObject struct {structName}");
                        localMessages.Add(new Message("StructName", "MapObject", $"Unknown structName {structName} of type {typeName}", null));
                        reader.Skip(size);
                        break;
                }

                structName = reader.ReadString();
            }

            if (messages != null) {
                foreach (var message in localMessages) {
                    message.Data = result.ToString();
                }
                messages.AddRange(localMessages);
            }
            return result;
        }
    }
}
