namespace PalworldSaveDecoding
{
    public record SaveReadingProgressData
    {
        public ProgressType ProgressType { get; init; }
        public float ProcessedPart { get; init; }


        public SaveReadingProgressData(ProgressType progressType, float processedPart)
        {
            ProgressType = progressType;
            ProcessedPart = processedPart;
        }
    }
}
