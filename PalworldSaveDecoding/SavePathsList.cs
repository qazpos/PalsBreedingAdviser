namespace PalworldSaveDecoding
{
    /// <summary>
    /// List<string> with some new methods. Designed to work with dot-separated paths.
    /// </summary>
    public class SavePathsList : List<string>
    {
        public bool ContainsSubPath(string subPath)
        {
            if (Count == 0) return false;

            for (int i = 0; i < Count; i++) {
                if (this[i].StartsWith(subPath))
                    return true;
            }
            return false;
        }
    }
}
