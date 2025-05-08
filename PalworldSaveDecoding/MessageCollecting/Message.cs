namespace PalworldSaveDecoding.MessageCollecting
{
    public class Message
    {
        public string? EntityType { get; set; }
        public string? EntityName { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string? Text { get; set; }
        public string? Data { get; set; }




        public Message(string? entityType, string? entityName, string? text, string? data) {
            EntityType = entityType;
            EntityName = entityName;
            Text = text;
            Data = data;
        }
    }
}
