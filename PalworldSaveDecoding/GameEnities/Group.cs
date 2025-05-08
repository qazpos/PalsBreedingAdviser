using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class Group
    {
        public string? GroupType { get; private set; }

        public byte[]? RawData { get; private set; }
        public string? GroupTypeStr { get; private set; }
        public Guid GroupId { get; private set; }
        public string? GroupName { get; private set; }
        public IndividualId[]? IndividualCharacterHandleId { get; private set; }

        public byte OrgType { get; private set; }
        public Guid[]? BaseIds { get; private set; }

        public int BaseCampLevel { get; private set; }
        public Guid[]? MapObjectInstanceIdsBaseCampPoints { get; private set; }
        public string? GuildName { get; private set; }

        public Guid PlayerUId { get; private set; }
        public string? GuildName2 { get; private set; }
        public (DateTime LastOnlineRealTime, string PlayerName) PlayerInfo { get; private set; }

        public Guid AdminPlayerUId { get; private set; }
        public (Guid PlayerUId, (DateTime LastOnlineRealTime, string PlayerName))[]? Players { get; private set; }

        public byte[]? CustomVersionData { get; private set; }




        public static Group Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new Group();
            var structName = reader.ReadString();
            while (structName != "None")
            {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName)
                {
                    case "GroupType":
                        result.GroupType = reader.ReadEnumPropertyAsString();
                        break;
                    case "RawData":
                        result.RawData = reader.ReadArrayProperty(reader.ReadByte);
                        result.DecodeRawData(result.RawData);
                        break;
                    case "CustomVersionData":
                        result.CustomVersionData = reader.ReadArrayProperty(reader.ReadByte); break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown Group struct {structName}");
                        localMessages.Add(new Message("StructName", "Group", $"Unknown structName {structName} of type {typeName}", null));
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


        private void DecodeRawData(byte[] data)
        {
            if (data.Length == 0)
                return;

            using (var reader = new GvasFileReader(new MemoryStream(data), true))
            {
                GroupId = reader.ReadGuid();
                GroupName = reader.ReadString();
                IndividualCharacterHandleId = reader.ReadArray(() => IndividualId.Read(reader, false));

                if (GroupType == "EPalGroupType::Guild" ||
                    GroupType == "EPalGroupType::IndependentGuild" ||
                    GroupType == "EPalGroupType::Organization")
                {
                    OrgType = reader.ReadByte();
                    BaseIds = reader.ReadArray(reader.ReadGuid);
                }

                if (GroupType == "EPalGroupType::Guild" ||
                    GroupType == "EPalGroupType::IndependentGuild")
                {
                    BaseCampLevel = reader.ReadInt32();
                    MapObjectInstanceIdsBaseCampPoints = reader.ReadArray(reader.ReadGuid);
                    GuildName = reader.ReadString();
                }

                if (GroupType == "EPalGroupType::IndependentGuild")
                {
                    PlayerUId = reader.ReadGuid();
                    GuildName2 = reader.ReadString();
                    PlayerInfo = (reader.ReadDateTime(), reader.ReadString());
                }

                if (GroupType == "EPalGroupType::Guild")
                {
                    AdminPlayerUId = reader.ReadGuid();
                    Players = new (Guid, (DateTime, string))[reader.ReadInt32()];

                    for (int i = 0; i < Players.Length; i++)
                        Players[i] = (reader.ReadGuid(), (reader.ReadDateTime(), reader.ReadString()));
                }

                if (!reader.IsBaseStreamEnds)
                    throw new InvalidDataException("Group raw data invalid length");
            }
        }
    }
}
