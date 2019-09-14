using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class PDFNameChecking
    {

        public static void CheckIsValidPDFName(string docsName)
        {
            if (docsName == "")
            {
                SendInvalidPDFName($"A szerver nem létező doksi nevet küldőtt: \"{docsName}\"!");
            }
            else if (docsName.Contains("#") == true)
            {
                SendInvalidPDFName($"A szerver még régebbi verziót használ, frissítsd le a szervert!");
            }
            else
            {
                SendingOperations.WriteMSG("/okDocs");
            }

        }

        static void SendInvalidPDFName(string msg)
        {
            SendingOperations.WriteMSG("/InvalidDocsName");

            throw new ApplicationException(msg);
        }

    }
}
