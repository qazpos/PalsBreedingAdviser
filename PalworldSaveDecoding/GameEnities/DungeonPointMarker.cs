using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class DungeonPointMarker
    {
        public Guid MarkerPointId { get; private set; }
        public DateTime NextRespawnGameTime { get; private set; }
        public Guid ConnectedDungeonInstanceId { get; private set; }




        public static DungeonPointMarker Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new DungeonPointMarker();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "MarkerPointId":
                        result.MarkerPointId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "NextRespawnGameTime":
                        result.NextRespawnGameTime = StructProperty.ReadSP(reader, () => reader.ReadDateTimePropertyComplex("Ticks"));
                        break;
                    case "ConnectedDungeonInstanceId":
                        result.ConnectedDungeonInstanceId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown DungeonPointMarker struct {structName}");
                        localMessages.Add(new Message("StructName", "DungeonPointMarker", $"Unknown structName {structName} of type {typeName}", null));
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
