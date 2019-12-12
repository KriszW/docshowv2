using Settings.Server;
using System;

namespace ItemNumberManager
{
    public class PDFModel
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public PDFModel(string name)
        {
            FileName = name ?? throw new ArgumentNullException(nameof(name));
            FilePath = System.IO.Path.Combine(ServerSettings.CurrentSettings.Resources,$"{name}.pdf");

            if (System.IO.File.Exists(FilePath))
            {
                _logger.Debug($"A {FilePath} fájlhoz az összes adat kiolvasása...");
                Datas = System.IO.File.ReadAllBytes(FilePath);
                _logger.Debug($"A {FilePath} fájlhoz az összes adat kiolvasása kész");
            }
            else
            {
                var ex = new ApplicationException(FilePath + " fájl nem található");
                _logger.Error($"A {name} pdfhez nem volt fájl megtalálható a {FilePath} helyen, a {ServerSettings.CurrentSettings.Resources} mappába", ex);
                throw ex;
            }

            Position = MonitorPosition.None;
        }

        public MonitorPosition Position { get; set; }
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public byte[] Datas { get; private set; }

        public override string ToString()
        {
            return FileName;
        }
    }
}