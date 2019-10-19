using System.Collections.Generic;
using System.Linq;

namespace KilokoModelLibrary
{
    public class CodeTableConverter
    {
        public List<string> Lines { get; private set; }

        public CodeTableConverter(string path)
        {
            Lines = System.IO.File.ReadAllLines(path).ToList();
            CodeTableModel.Codes = new List<CodeTableModel>();
        }

        public void Convert()
        {
            foreach (var item in Lines)
            {
                var datas = item.Split(';');

                var name = datas[0];

                var models = new List<IKilokoModel>();

                for (int i = 1; i < datas.Length; i++)
                {
                    if (datas[i] != "" && char.IsDigit(name[0]))
                    {
                        var kiloko = new KilokoModel(datas[i]);

                        models.Add(kiloko);
                    }
                }

                CodeTableModel.Codes.Add(new CodeTableModel(name, models));
            }
        }
    }
}