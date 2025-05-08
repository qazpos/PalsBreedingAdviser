using System.IO;

namespace PalsBreedingAdvicer
{
    public static class SaveFileSearcher
    {
        public static IEnumerable<SaveFileLocation> SearchSaveFiles(string path)
        {
            string? directory = null;
            if (File.Exists(path))
                directory = Path.GetDirectoryName(path);
            else if (Directory.Exists(path))
                directory = path;

            if (directory != null) {
                var levelMetaFile = Path.Combine(directory, "LevelMeta.sav");
                var levelFile = Path.Combine(directory, "Level.sav");
                if (File.Exists(levelMetaFile) && File.Exists(levelFile)) {
                    yield return new SaveFileLocation(levelMetaFile, levelFile);
                } else {
                    var subDirectories = Directory.GetDirectories(path);
                    foreach (var subDirectory in subDirectories) {
                        foreach (var saveFileLocation in SearchSaveFiles(subDirectory))
                            yield return saveFileLocation;
                    }
                }
            }
        }
    }
}
