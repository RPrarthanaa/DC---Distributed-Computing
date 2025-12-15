

namespace Library
{
    public class FileItem
    {
        public string DisplayText { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string Username { get; set; }

        public override string ToString()
        {
            return DisplayText;
        }
    }
}