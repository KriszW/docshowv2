using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class SetKilokoClients
    {

        public static void SetKilokoClient(List<string> asztalSzamok,Client setTo)
        {
            foreach (var asztalSzam in asztalSzamok)
            {
                var kiloko = DataOperations.GetKilokoModel(asztalSzam);

                kiloko.SetClient(setTo);
            }
        }

    }
}
