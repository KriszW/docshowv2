using System.Collections.Generic;

namespace Machines
{
    public class MachineLoader
    {
        public string[] Lines { get; private set; }

        public MachineLoader(string path)
        {
            Lines = System.IO.File.ReadAllLines(path);
        }

        public List<Machine> Load()
        {
            var output = new List<Machine>();

            foreach (var item in Lines)
            {
                if (item.StartsWith("//") == false)
                {
                    var datas = item.Split(';');

                    if (datas.Length >= 3)
                    {
                        var kilokoNum = datas[0];
                        var ip = datas[1];
                        var monitorIndex = int.Parse(datas[2]);

                        var model = new Machine(ip, monitorIndex, kilokoNum);
                        output.Add(model); 
                    }
                }
            }

            return output;
        }
    }
}