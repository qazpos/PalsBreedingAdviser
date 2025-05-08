using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class EnemyCampStatus
    {
        public bool IsSpawned { get; private set; }
        public bool IsEnemyAllDead { get; private set; }
        public bool IsClear { get; private set; }
        public bool IsRewardReceived { get; private set; }
        public string? RewardPalId { get; private set; }
        public int RewardPalLevel { get; private set; }
        public DateTime? ClearDate { get; private set; }
        public float ElapsedTime { get; private set; }




        public static EnemyCampStatus Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new EnemyCampStatus();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "bIsSpawned":
                        result.IsSpawned = reader.ReadBoolProperty(); break;
                    case "bIsEnemyAllDead":
                        result.IsEnemyAllDead = reader.ReadBoolProperty(); break;
                    case "bIsClear":
                        result.IsClear = reader.ReadBoolProperty(); break;
                    case "bRewardReceived":
                        result.IsRewardReceived = reader.ReadBoolProperty(); break;
                    case "RewardPalId":
                        result.RewardPalId = reader.ReadStringProperty(); break;
                    case "RewardPalLevel":
                        result.RewardPalLevel = reader.ReadInt32Property(); break;
                    case "ClearDate":
                        result.ClearDate = StructProperty.ReadSP(reader, reader.ReadDateTime); break;
                    case "ElapsedTime":
                        result.ElapsedTime = reader.ReadFloatProperty(); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown EnemyCampStatus struct {structName}");
                        localMessages.Add(new Message("StructName", "EnemyCampStatus", $"Unknown structName {structName} of type {typeName}", null));
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
