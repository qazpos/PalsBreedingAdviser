using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class SupplyInfo
    {
        public PalSupplyType SupplyType { get; private set; }
        public Guid SupplySpawnerGuid { get; private set; }
        public DateTime SupplyTime { get; private set; }
        public DateTime SupplyLandedTime { get; private set; }
        public bool WipedOut_Pal { get; private set; }
        public bool WipedOut_Npc { get; private set; }
        public Guid SupplyMapObjectId { get; private set; }
        public Guid[]? DropItemGuids { get; private set; }
        public Guid MapLocationId { get; private set; }




        public static SupplyInfo Read(GvasFileReader reader, MessageCollection? messages)
        {
            var localMessages = new MessageCollection();
            var result = new SupplyInfo();

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "SupplyType":
                        result.SupplyType = reader.ReadEnumProperty<PalSupplyType>(); break;
                    case "SupplySpawnerGuid":
                        result.SupplySpawnerGuid = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "SupplyTime":
                        result.SupplyTime = StructProperty.ReadSP(reader, reader.ReadDateTime); break;
                    case "SupplyLandedTime":
                        result.SupplyLandedTime = StructProperty.ReadSP(reader, reader.ReadDateTime); break;
                    case "bWipedOut_Pal":
                        result.WipedOut_Pal = reader.ReadBoolProperty(); break;
                    case "bWipedOut_NPC":
                        result.WipedOut_Npc = reader.ReadBoolProperty(); break;
                    case "SupplyMapObjectId":
                        result.SupplyMapObjectId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    case "DropItemGuids":
                        result.DropItemGuids = reader.ReadArrayProperty(reader.ReadGuid, true); break;
                    case "MapLocationId":
                        result.MapLocationId = StructProperty.ReadSP(reader, reader.ReadGuid); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown SupplyInfo struct {structName}");
                        localMessages.Add(new Message("StructName", "SupplyInfo", $"Unknown structName {structName} of type {typeName}", null));
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
