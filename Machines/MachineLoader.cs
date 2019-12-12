using System.Collections.Generic;

namespace Machines
{
    public class MachineLoader
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string[] Lines { get; private set; }
        public string FilePath { get; set; }

        public MachineLoader(string path)
        {
            FilePath = path;
            Lines = System.IO.File.ReadAllLines(path);
        }

        public List<Machine> Load()
        {
            var output = new List<Machine>();

            _logger.Debug($"A {FilePath} fájlból gép adatok kiolvasása...");

            foreach (var item in Lines)
            {
                if (item.StartsWith("//") == false)
                {
                    _logger.Debug($"A {item} sor feldolgozása...");
                    var datas = item.Split(';');

                    _logger.Debug($"A {item} sor feldarabolása, {datas.Length} darabra lett feldarabolva");

                    if (datas.Length >= 3)
                    {
                        var kilokoNum = datas[0];
                        var ip = datas[1];
                        var monitorIndex = int.Parse(datas[2]);

                        _logger.Debug($"A {item} sorból a gép adat kiolvasva: IP:{ip}, asztalszám: {kilokoNum}, monitor száma: {monitorIndex}");
                        var model = new Machine(ip, monitorIndex, kilokoNum);
                        output.Add(model); 
                    }
                    else
                    {
                        _logger.Error($"Az {item} sorból a feldarabolás túl rövid volt, {datas.Length} darabbal");
                    }
                }
                else
                {
                    _logger.Debug($"A {item} sor komment volt");
                }
            }

            _logger.Debug($"A {FilePath} fájlból {output.Count} darab gép adat kiolvasva");
            return output;
        }
    }
}