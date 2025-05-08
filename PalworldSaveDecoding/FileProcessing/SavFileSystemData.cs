namespace PalworldSaveDecoding
{
    internal class SavFileSystemData
    {
        private const string MagicBytesValue = "50-6C-5A";
        public string FileName { get; }
        public string FileExtention { get; }
        public string FilePath { get; }
        public int UncompressedLength { get; }
        public int CompressedLength { get; }
        public string MagicBytes { get; }
        public byte CompressionType { get; }

        public SavFileSystemData (string fullPath, byte[] systemData)
        {
            UncompressedLength = BitConverter.ToInt32(systemData[0..4]);
            CompressedLength = BitConverter.ToInt32(systemData[4..8]);
            MagicBytes = BitConverter.ToString(systemData[8..11]);
            CompressionType = systemData[11];

            FileExtention = Path.GetExtension(fullPath);
            FileName = Path.GetFileName(fullPath);
            FilePath = Path.GetFullPath(fullPath);
        }

        public void Check()
        {
            if (CompressionType != 48 && CompressionType != 49 && CompressionType != 50)
                throw new InvalidDataException("The save file uses an unknown save type");

            if (CompressionType != 49 && CompressionType != 50)
                throw new InvalidDataException("The save file uses an unhandled compression type");

            if (MagicBytes != MagicBytesValue)
                throw new InvalidDataException("The file don't look like the save file");
        }
    }
}
