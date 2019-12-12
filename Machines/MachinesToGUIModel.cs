using KilokoModelLibrary;
using System;
using System.Linq;

namespace Machines
{
    public class MachinesToGUIModel
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MachinesToGUIModel()
        {
        }

        public MachinesToGUIModel(KilokoModel model)
        {
            _logger.Debug($"A {model.Kiloko} kilőkőmodel GUI modelre fordítása...");
            Kiloko = model.Kiloko;
            var item = model.GetItemnamesModel();

            Itemnumber = item.Material;

            var left = item.PDFs.FirstOrDefault(p => p.Position == ItemNumberManager.MonitorPosition.Left);
            var right = item.PDFs.FirstOrDefault(p => p.Position == ItemNumberManager.MonitorPosition.Right);

            if (left != default)
            {
                DocLeft = left.FileName;
            }

            if (right != default)
            {
                DocRight = right.FileName; 
            }
            _logger.Debug($"A {model.Kiloko} kilőkőmodel GUI modelre fordítása kész");
            _logger.Debug($"A {model.Kiloko} kilőkőmodel cikke: {Itemnumber}");
            _logger.Debug($"A {model.Kiloko} kilőkőmodel bal doksija: {DocLeft}");
            _logger.Debug($"A {model.Kiloko} kilőkőmodel jobb doksija: {DocRight}");
        }

        public MachinesToGUIModel(int kiloko, string itemnumber, string docLeft, string docRight, bool active)
        {
            Kiloko = kiloko;
            Itemnumber = itemnumber ?? throw new ArgumentNullException(nameof(itemnumber));
            DocLeft = docLeft ?? throw new ArgumentNullException(nameof(docLeft));
            DocRight = docRight ?? throw new ArgumentNullException(nameof(docRight));
            Active = active;
        }

        public int Kiloko { get; set; }
        public string Itemnumber { get; set; }
        public string DocLeft { get; set; } = "Nincs PDF";
        public string DocRight { get; set; } = "Nincs PDF";
        public bool Active { get; set; }

        public string GetIP()
        {
            _logger.Debug($"A {Kiloko} asztal számához a gép megszerzése...");
            var machine = Machine.Machines.Where(m => m.KilokoNum == Kiloko).FirstOrDefault();
            _logger.Debug($"A {Kiloko} asztal számához a gép megszerzve");
            _logger.Debug($"A {Kiloko}-nél az IP cím visszaadása");
            return machine != default ? machine.IP : (default);
        }
    }
}