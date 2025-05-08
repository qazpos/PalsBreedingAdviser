using PalworldSaveDecoding.MessageCollecting;

namespace PalworldSaveDecoding
{
    public class Level
    {
        public GvasDataHeader? Header { get; private set; }

        public int Version { get; private set; }
        public DateTime Timestamp { get; private set; }
        public WorldSaveData WorldSaveData { get; private set; } = new WorldSaveData();

        public string? Footer { get; private set; }



        static ProgressType progressReportType = ProgressType.Level;

        static readonly string thisTypeName = "Level";

        static readonly Dictionary<string, string> subPaths = new Dictionary<string, string>() {
            { "Version", "Version" },
            { "Timestamp", "Timestamp" },
            { "worldSaveData", "WorldSaveData" },
        };


        public static Level Read(string filename, SavePathsList? pathsList, IProgress<SaveReadingProgressData>? progress, MessageCollection? messages)
        {
            using (var reader = new GvasFileReader(new FileStream(filename, FileMode.Open), true))
                return Read(reader, pathsList, progress, messages);
        }


        public static Level Read(GvasFileReader reader, SavePathsList? pathsList, IProgress<SaveReadingProgressData>? progress, MessageCollection? messages)
        {
            var localMessages = new MessageCollection();
            var result = new Level();

            var isFilteredReading = pathsList != null && pathsList.Count > 0;

            progress?.Report(new(progressReportType, reader.WasReadPart));
            result.Header = GvasDataHeader.Read(reader);


            var structName = reader.ReadString();
            while (structName != "None") {
                var typeName = reader.ReadString();
                var size = reader.ReadUInt64();
                var bytesLeft = reader.BytesLeft;

                var subPathFull = "";
                var needToRead = true;
                if (isFilteredReading) {
                    if (subPaths.TryGetValue(structName, out subPathFull)) {
                        //Если subPathFull нашёлся, значит его мы умеем его читать и прочитаем, если не будет ошибок
                        if (!pathsList!.ContainsSubPath(subPathFull))
                            needToRead = false;
                        if (needToRead)
                            pathsList.RemoveAll(p => p == subPathFull);
                    } else
                        subPathFull = "";
                }

                switch (structName) {
                    case "Version":
                        result.Version = reader.ReadInt32Property();
                        break;
                    case "Timestamp":
                        result.Timestamp = StructProperty.ReadSP(reader, reader.ReadDateTime);
                        break;
                    case "worldSaveData":
                        result.WorldSaveData = WorldSaveData.Read(reader, size, progress, messages, pathsList, subPathFull);
                        break;
                    default:
                        if (messages == null)
                            throw new InvalidDataException($"Unknown save struct {structName}");
                        localMessages.Add(new Message("StructName", "Level", $"Unknown structName {structName} of type {typeName}", null));
                        reader.Skip(size);
                        break;
                }
                System.Diagnostics.Debug.WriteLine($"{structName};{size};{bytesLeft};{reader.BytesLeft}");
                progress?.Report(new(progressReportType, reader.WasReadPart));

                structName = reader.ReadString();
            }

            if (messages != null)
                messages.AddRange(localMessages);

            progress?.Report(new(progressReportType, reader.WasReadPart));
            return result;
        }


        public static Task<Level> ReadAsync(string filename, SavePathsList? pathsList, IProgress<SaveReadingProgressData> progress, MessageCollection? messages) =>
            Task.Run(() => Read(filename, pathsList, progress, messages));

        public static Task<Level> ReadAsync(GvasFileReader reader, SavePathsList? pathsList, IProgress<SaveReadingProgressData> progress, MessageCollection? messages) =>
            Task.Run(() => Read(reader, pathsList, progress, messages));





        public static Level Read(GvasFileReader reader, SavePathsList? pathsList, MessageCollection? messages) =>
            Read(reader, pathsList, null, messages);

        public static Level Read(GvasFileReader reader, SavePathsList? pathsList, IProgress<SaveReadingProgressData> progress) =>
            Read(reader, pathsList, progress, null);

        public static Level Read(GvasFileReader reader, SavePathsList? pathsList) =>
            Read(reader, pathsList, null, null);

        public static Level Read(GvasFileReader reader, IProgress<SaveReadingProgressData> progress, MessageCollection? messages) =>
            Read(reader, null, progress, messages);

        public static Level Read(GvasFileReader reader, MessageCollection? messages) =>
            Read(reader, null, null, messages);

        public static Level Read(GvasFileReader reader, IProgress<SaveReadingProgressData> progress) =>
            Read(reader, null, progress, null);

        public static Level Read(GvasFileReader reader) =>
            Read(reader, null, null, null);



        public static Level Read(string filename, SavePathsList? pathsList, MessageCollection? messages) =>
            Read(filename, pathsList, null, messages);

        public static Level Read(string filename, SavePathsList? pathsList, IProgress<SaveReadingProgressData> progress) =>
            Read(filename, pathsList, progress, null);

        public static Level Read(string filename, SavePathsList? pathsList) =>
           Read(filename, pathsList, null, null);

        public static Level Read(string filename, IProgress<SaveReadingProgressData> progress, MessageCollection? messages) =>
            Read(filename, null, progress, messages);

        public static Level Read(string filename, MessageCollection? messages) =>
            Read(filename, null, null, messages);

        public static Level Read(string filename, IProgress<SaveReadingProgressData> progress) =>
            Read(filename, null, progress, null);

        public static Level Read(string filename) =>
            Read(filename, null, null, null);



        public static Task<Level> ReadAsync(string filename, SavePathsList? pathsList, MessageCollection? messages) =>
            Task.Run(() => Read(filename, pathsList, null, messages));

        public static Task<Level> ReadAsync(string filename, SavePathsList? pathsList, IProgress<SaveReadingProgressData> progress) =>
            Task.Run(() => Read(filename, pathsList, progress, null));

        public static Task<Level> ReadAsync(string filename, SavePathsList? pathsList) =>
            Task.Run(() => Read(filename, pathsList, null, null));

        public static Task<Level> ReadAsync(string filename, IProgress<SaveReadingProgressData> progress, MessageCollection? messages) =>
            Task.Run(() => Read(filename, null, progress, messages));

        public static Task<Level> ReadAsync(string filename, MessageCollection? messages) => 
            Task.Run(() => Read(filename, null, null, messages));

        public static Task<Level> ReadAsync(string filename, IProgress<SaveReadingProgressData> progress) =>
            Task.Run(() => Read(filename, null, progress, null));

        public static Task<Level> ReadAsync(string filename) =>
            Task.Run(() => Read(filename, null, null, null));



        public static Task<Level> ReadAsync(GvasFileReader reader, SavePathsList? pathsList, MessageCollection? messages) =>
            Task.Run(() => Read(reader, pathsList, null, messages));

        public static Task<Level> ReadAsync(GvasFileReader reader, SavePathsList? pathsList, IProgress<SaveReadingProgressData> progress) =>
            Task.Run(() => Read(reader, pathsList, progress, null));

        public static Task<Level> ReadAsync(GvasFileReader reader, SavePathsList? pathsList) =>
            Task.Run(() => Read(reader, pathsList, null, null));

        public static Task<Level> ReadAsync(GvasFileReader reader, IProgress<SaveReadingProgressData> progress, MessageCollection? messages) =>
            Task.Run(() => Read(reader, null, progress, messages));

        public static Task<Level> ReadAsync(GvasFileReader reader, MessageCollection? messages) =>
            Task.Run(() => Read(reader, null, null, messages));

        public static Task<Level> ReadAsync(GvasFileReader reader, IProgress<SaveReadingProgressData> progress) =>
            Task.Run(() => Read(reader, null, progress, null));

        public static Task<Level> ReadAsync(GvasFileReader reader) =>
            Task.Run(() => Read(reader, null, null, null));
    }
}
