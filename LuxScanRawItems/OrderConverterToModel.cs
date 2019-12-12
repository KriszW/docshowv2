using LuxScanOrdModel;
using System.Collections.Generic;

namespace LuxScanRawItems
{
    public class OrderConverterToModel
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public OrderConverterToModel()
        {
        }

        public List<LuxScanItem> Convert(string path)
        {
            var rawLines = new string[0];

            try
            {
                rawLines = System.IO.File.ReadAllLines(path);
            }
            catch (System.Exception ex)
            {
                _logger.Error($"A {path} fájl olvasása közben váratlan hiba lépett fel",ex);
            }

            try
            {
                var getter = new GetRawItems(rawLines);

                return getter.Convert();
            }
            catch (System.Exception ex)
            {
                _logger.Error($"A {path} fájl lekonvertálása közben váratlan hiba lépett fel", ex);
            }

            return new List<LuxScanItem>();
        }
    }
}