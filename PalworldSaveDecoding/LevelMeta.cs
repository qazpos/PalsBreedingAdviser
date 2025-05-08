using PalworldSaveDecoding.MessageCollecting;
using System;

namespace PalworldSaveDecoding
{
    public class LevelMeta
    {
        public GvasDataHeader? Header { get; private set; }

        public int Version { get; private set; }
        public DateTime Timestamp { get; private set; }
        public SaveData? SaveData { get; private set; }
        public byte[]? Footer { get; private set; }



        public static LevelMeta Read(string filename, MessageCollection? messages = null)
        {
            using (var reader = new GvasFileReader(new FileStream(filename, FileMode.Open), true))
                return Read(reader, messages);
        }



        public static LevelMeta Read(GvasFileReader reader, MessageCollection? messages = null)
        {
            var localMessages = new MessageCollection();
            var result = new LevelMeta();
            result.Header = GvasDataHeader.Read(reader);

            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();

                switch (structName) {
                    case "Version":
                        result.Version = reader.ReadInt32Property(); break;
                    case "Timestamp":
                        result.Timestamp = StructProperty.ReadSP(reader, reader.ReadDateTime); break;
                    case "SaveData":
                        result.SaveData = SaveData.Read(reader, messages);
                        break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown LevelMeta struct {structName}");
                        localMessages.Add(new Message("StructName", "LevelMeta", $"Unknown structName {structName} of type {typeName}", null));
                        reader.Skip(size);
                        break;
                }

                structName = reader.ReadString();
            }
            result.Footer = reader.ReadToEnd();

            if (messages != null)
                messages.AddRange(localMessages);
            return result;
        }
    }
}
