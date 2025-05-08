namespace PalworldSaveDecoding
{
    public struct ByteProperty
    {
        public string EnumType { get; set; }
        public Guid? Id { get; set; }
        public byte? ByteValue { get; set; }
        public string? EnumValue { get; set; }

        public ByteProperty(string enumType, Guid? id, byte? byteValue, string? enumValue)
        {
            EnumType = enumType;
            Id = id;
            ByteValue = byteValue;
            EnumValue = enumValue;
        }
    }
}
