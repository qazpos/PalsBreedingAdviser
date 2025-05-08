using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class OilrigData
    {
        public Guid[]? DestroyedCannon { get; private set; }
        public Guid[]? DestroyedGasTank { get; private set; }
        public Guid[]? WipedOutEnemySpawner { get; private set; }
        public Guid[]? OpenedTreasureBoxSpawner { get; private set; }
        public int GoalTreasureBoxSpawnerIndex { get; private set; }
        public bool Alarm { get; private set; }
        public bool Clear { get; private set; }
        public float ResetTimer { get; private set; }
        public bool IsMachineTimerCountUp { get; private set; }
        public bool IsMachineDestroyed { get; private set; }
        public float MachineStartTimer { get; private set; }




        public static OilrigData Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new OilrigData();

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "DestroyedCannon":
                        result.DestroyedCannon = reader.ReadArrayProperty(reader.ReadGuid, true); break;
                    case "DestroyedGasTank":
                        result.DestroyedGasTank = reader.ReadArrayProperty(reader.ReadGuid, true); break;
                    case "WipedOutEnemySpawner":
                        result.WipedOutEnemySpawner = reader.ReadArrayProperty(reader.ReadGuid, true); break;
                    case "OpenedTreasureBoxSpawner":
                        result.OpenedTreasureBoxSpawner = reader.ReadArrayProperty(reader.ReadGuid, true); break;
                    case "GoalTreasureBoxSpawnerIndex":
                        result.GoalTreasureBoxSpawnerIndex = reader.ReadInt32Property(); break;
                    case "Alarm":
                        result.Alarm = reader.ReadBoolProperty(); break;
                    case "Clear":
                        result.Clear = reader.ReadBoolProperty(); break;
                    case "ResetTimer":
                        result.ResetTimer = reader.ReadFloatProperty(); break;
                    case "IsMachineTimerCountUp":
                        result.IsMachineTimerCountUp = reader.ReadBoolProperty(); break;
                    case "IsMachineDestroyed":
                        result.IsMachineDestroyed = reader.ReadBoolProperty(); break;
                    case "MachineStartTimer":
                        result.MachineStartTimer = reader.ReadFloatProperty(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown OilrigData struct {structName}");
                        localMessages.Add(new Message("StructName", "OilrigData", $"Unknown structName {structName} of type {typeName}", null));
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
