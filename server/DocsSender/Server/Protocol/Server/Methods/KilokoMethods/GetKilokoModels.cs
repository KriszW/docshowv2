using KilokoModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    static class GetKilokoModels
    {

        public static List<IKilokoModel> GetAllKiloko(List<string> asztalSzamok)
        {
            var output = new List<IKilokoModel>();

            foreach (var asztal in asztalSzamok)
            {
                output.Add(DataOperations.GetKilokoModel(asztal));
            }

            return output;
        }

    }
}
