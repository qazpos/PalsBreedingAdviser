using System.IO.Compression;

namespace PalworldSaveDecoding
{
    internal static class ZLibDataCompresser
    {
        public static MemoryStream Decompress(Stream compressedData)
        {
            var result = new MemoryStream();
            try
            {
                var sourceStartPosition = compressedData.Position;
                using (var decompresser = new ZLibStream(compressedData, CompressionMode.Decompress, true))
                {
                    var buffer = new byte[128];
                    int bytesRead;
                    while ((bytesRead = decompresser.Read(buffer, 0, buffer.Length)) > 0)
                        result.Write(buffer, 0, bytesRead);
                }
                compressedData.Position = sourceStartPosition;
            }
            catch
            {
                result.Dispose();
                throw;
            }
            result.Position = 0;
            return result;
        }
    }
}
