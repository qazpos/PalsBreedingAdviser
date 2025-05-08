namespace PalworldSaveDecoding
{
    internal static class EnumCollector
    {
        private static string outputFilename = "Stats\\Enum.values";

        private static List<string> enumValues = new List<string>();

        private static StreamWriter? writer;




        static EnumCollector()
        {
            if (!Directory.Exists(Path.GetDirectoryName(outputFilename)))
                Directory.CreateDirectory(Path.GetDirectoryName(outputFilename)!);
            if (!File.Exists(outputFilename)) {
                File.Create(outputFilename);
                return;
            }

            using (var stream = new StreamReader(outputFilename)) {
                while (!stream.EndOfStream) {
                    var line = stream.ReadLine();
                    if (line != null)
                        enumValues.Add(line.Trim());
                }
            }

            writer = new StreamWriter(outputFilename);
        }


        public static void AddValue(string value)
        {
            if (enumValues.Contains(value))
                return;

            //using (var stream = new StreamWriter(outputFilename, true)) {
            //    stream.WriteLine(value);
            //}

            if (writer != null){
                writer.WriteLine(value);
                writer.Flush();
            }

            enumValues.Add(value);
        }
    }
}
