using ItemNumberManager;
using KilokoModelLibrary;
using LuxScanOrdModel;
using System.Collections.Generic;
using System.Linq;

namespace LuxScanRawItems
{
    public class KilokoGrouper
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<LuxScanItem> RawDatas { get; set; }

        public KilokoGrouper(List<LuxScanItem> rawDatas)
        {
            RawDatas = rawDatas;
        }

        public MonitorPosition GetPosition(string raw)
        {
            _logger.Debug($"A {raw} asztalhoz a pozició megszerzése...");
            if (raw.ToUpper().EndsWith("B"))
            {
                _logger.Debug($"A {raw} asztalhoz a pozició megszerzése, bal oldal lett");
                return MonitorPosition.Left;
            }
            else if (raw.ToUpper().EndsWith("J"))
            {
                _logger.Debug($"A {raw} asztalhoz a pozició megszerzése, a jobb oldal lett");
                return MonitorPosition.Right;
            }
            else
            {
                _logger.Debug($"A {raw} asztalhoz a pozició megszerzése, ismeretlen lett");
                return MonitorPosition.None;
            }
        }

        public List<KilokoModel> GroupKilokok()
        {
            var output = new List<KilokoModel>();

            _logger.Info($"A {RawDatas.Count} darab nyers adat lefordítása kiküldhető asztal hozzárendelésekre...");

            foreach (var item in RawDatas)
            {
                foreach (var kilokoItem in item.Models)
                {
                    var model = output.FirstOrDefault(m=> m.Kiloko == kilokoItem.Kiloko);

                    if (model == default)
                    {
                        _logger.Debug($"Az adatokból a {kilokoItem.Kiloko} létrehozva");
                        model = new KilokoModel(kilokoItem.Kiloko.ToString());
                        output.Add(model);
                    }

                    foreach (var material in item.Items)
                    {
                        _logger.Debug($"Az adatokból a {kilokoItem.Kiloko}-hoz {material.MaterialName} cikk hozzáadása");
                        model.AddNewCikk(material,GetPosition(kilokoItem.RawKiloko));
                    }
                }
            }

            _logger.Info($"A {RawDatas.Count} darab nyers adat lefordítása kiküldhető asztal hozzárendelésekre sikeres volt {output.Count} darabra sikerült lefordítani");

            return output;
        }
    }
}