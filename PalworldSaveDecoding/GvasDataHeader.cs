namespace PalworldSaveDecoding
{
    public class GvasDataHeader
    {
        private string? GvasStr { get; set; }
        private int SaveGameVersion { get; set; }
        private int PackageFileVersionUE4 { get; set; }
        private int PackageFileVersionUE5 { get; set; }
        private int EngineVersionMajor { get; set; }
        private int EngineVersionMinor { get; set; }
        private int EngineVersionPatch { get; set; }
        private uint EngineVersionChangelist { get; set; }
        private string? EngineVersionBranch { get; set; }
        private int CustomVersionFormat { get; set; }
        private (Guid, int)[]? CustomVersions { get; set; }
        private string? SaveGameClassName { get; set; }
        public long Length { get; private set; }




        public static GvasDataHeader Read(GvasFileReader reader)
        {
            var result = new GvasDataHeader();

            if (reader.BaseStream.Position != 0)
                throw new ArgumentException("The readers base stream is not at postition 0");
            result.GvasStr = new string(reader.ReadChars(4));
            if (result.GvasStr != "GVAS")
                throw new InvalidDataException("The header GvasStr is invalid");

            result.SaveGameVersion = reader.ReadInt32();
            if (result.SaveGameVersion != 3)
                throw new InvalidDataException($"The correct game version is 3, but the result is {result.SaveGameVersion}");

            result.PackageFileVersionUE4 = reader.ReadInt32();
            result.PackageFileVersionUE5 = reader.ReadInt32();

            result.EngineVersionMajor = reader.ReadUInt16();
            result.EngineVersionMinor = reader.ReadUInt16();
            result.EngineVersionPatch = reader.ReadUInt16();
            result.EngineVersionChangelist = reader.ReadUInt32();
            result.EngineVersionBranch = reader.ReadString();

            result.CustomVersionFormat = reader.ReadInt32();
            if (result.CustomVersionFormat != 3)
                throw new InvalidDataException($"The correct custom game version format is 3, but the result is {result.CustomVersionFormat}");

            result.CustomVersions = reader.ReadArray(() => (reader.ReadGuid(), reader.ReadInt32()));
            result.SaveGameClassName = reader.ReadString();

            result.Length = reader.BaseStream.Position;

            return result;
        }
    }
}
