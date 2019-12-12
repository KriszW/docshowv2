namespace LuxScanRawItems
{
    internal class IDConverter
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string RawLine { get; private set; }

        public IDConverter(string line)
        {
            RawLine = line;
        }

        public int GetID()
        {
            var longID = RawLine.Split('=')[0];
            return GetIDFromRaw(longID);
        }

        private int GetIDFromRaw(string longID)
        {
            _logger.Debug($"A {longID} adatból az ID számának megszerzése");
            var tempLine = "";

            foreach (var item in longID)
            {
                if (char.IsDigit(item))
                {
                    tempLine += item;
                }
            }

            _logger.Debug($"A {longID} adatból az ID száma megszerezve: {tempLine}");
            return int.Parse(tempLine);
        }
    }
}