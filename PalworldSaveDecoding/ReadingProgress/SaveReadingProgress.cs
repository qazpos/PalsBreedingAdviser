namespace PalworldSaveDecoding
{
    public class SaveReadingProgress
    {
        public bool IsFileDecompressed { get; set; }
        public float DecompressedPart { get; set; } = 0;
        public bool IsFileRead { get; set; }
        public float WasReadPart { get; set; } = 0;

        private const float decompressingPart = 0.3f;
        private const float readingPart = 0.7f;


        public int GetOveralProgress(int processFinishValue) =>
            (int)(processFinishValue * (decompressingPart * DecompressedPart + readingPart * WasReadPart));
    }
}