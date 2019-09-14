using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class AdobeClosingMethod
    {
        //az összes adoba bezárása ami éppen fut
        public static void CloseAllAdobe()
        {
            foreach(var adobe in Process.GetProcessesByName("AcroRd32"))
            {
                try
                {
                    adobe.Kill();
                }
                catch(Exception)
                {

                }
            }
        }
    }
}
