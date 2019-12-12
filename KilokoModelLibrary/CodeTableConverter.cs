using System.Collections.Generic;
using System.Linq;

namespace KilokoModelLibrary
{
    public class CodeTableConverter
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<string> Lines { get; private set; }

        public CodeTableConverter(string path)
        {
            _logger.Info($"A {path} fájlból a kódtábla beolvasása");
            Lines = System.IO.File.ReadAllLines(path).ToList();
            CodeTableModel.Codes = new List<CodeTableModel>();
        }

        public void Convert()
        {
            foreach (var item in Lines)
            {
                var datas = item.Split(';');

                var name = datas[0];
                _logger.Info($"A {name} kód létrehozássa...");

                var models = new List<IKilokoModel>();

                for (int i = 1; i < datas.Length; i++)
                {
                    if (datas[i] != "" && char.IsDigit(name[0]))
                    {
                        var kiloko = new KilokoModel(datas[i]);

                        models.Add(kiloko);
                        _logger.Info($"A {name} kódhoz a {kiloko.RawKiloko} hozzáadva");
                    }
                }

                var newModel = new CodeTableModel(name, models);

                _logger.Info($"A {name} kód létrehozva");
                CodeTableModel.Codes.Add(newModel);
            }
        }
    }
}