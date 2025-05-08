namespace PalworldSaveDecoding
{
    public static class SavDecompresser
    {
        static ProgressType progressReportType = ProgressType.Decompress;



        public static string Decompress(string fileName, IProgress<SaveReadingProgressData>? progress)
        {
            MemoryStream decompressedData;
            SavFileSystemData fileData;
            string outputFileName;

            using (var originalFile = File.OpenRead(fileName))
            {
                if (originalFile.Length < 12)
                    throw new InvalidDataException("The save file has no a 12 system bytes");

                var systemBytes = new byte[12];
                originalFile.Read(systemBytes, 0, systemBytes.Length);

                fileData = new SavFileSystemData(fileName, systemBytes);
                fileData.Check();

                if (fileData.CompressionType == 49 && originalFile.Length != fileData.CompressedLength + 12)
                    throw new InvalidDataException("The save file has an incorrect compressed length");

                decompressedData = ZLibDataCompresser.Decompress(originalFile);
            }

            if (fileData.CompressionType == 50)
                progress?.Report(new (progressReportType, 0.3f));
            else
                progress?.Report(new(progressReportType, 0.7f));


            try
            {
                if (fileData.CompressionType == 50)
                {
                    if (fileData.CompressedLength != decompressedData.Length)
                        throw new InvalidDataException("The save file has an incorrect compressed length");

                    using (var tempData = decompressedData)
                        decompressedData = ZLibDataCompresser.Decompress(tempData);
                }

                if (fileData.UncompressedLength != decompressedData.Length)
                    throw new InvalidDataException("The save file has an incorrect uncompressed length");

                outputFileName = Path.ChangeExtension(fileData.FilePath, ".gvas");

                using (var writer = File.OpenWrite(outputFileName))
                {
                    var buffer = new byte[128];
                    int bytesRead;
                    while ((bytesRead = decompressedData.Read(buffer, 0, buffer.Length)) > 0)
                        writer.Write(buffer, 0, bytesRead);
                }
            }
            finally
            {
                decompressedData.Dispose();
            }

            progress?.Report(new(progressReportType, 1f));
            return outputFileName;
        }



        public static Task<string> DecompressAsync(string fileName, IProgress<SaveReadingProgressData>? progress)
        {
            return Task.Run(() => Decompress(fileName, progress));
        }



        public static string Decompress(string fileName) => Decompress(fileName, null);

        public static Task<string> DecompressAsync(string fileName) => DecompressAsync(fileName, null);
    }
}
