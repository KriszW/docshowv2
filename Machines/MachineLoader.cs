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

        public List<MachineModel> Load()
        {
            var output = new List<MachineModel>();

            foreach (var item in Lines)
            {
                if (item.StartsWith("//") == false)
                {
                    var datas = item.Split(';');

                    var kilokoNum = datas[0];
                    var ip = datas[1];
                    var monitorIndex = int.Parse(datas[2]);

                    var model = new MachineModel(ip, monitorIndex, kilokoNum);
                    output.Add(model);
                }
            }

            return output;
        }
    }
}