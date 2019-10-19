namespace ItemNumberManager
{
    internal class PDFekModel
    {
        public PDFekModel(string mask, string[] files)
        {
            Mask = mask;
            Files = files;
        }

        public string Mask { get; private set; }
        public string[] Files { get; private set; }
    }
}