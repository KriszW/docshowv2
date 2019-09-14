using IOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class GetDocsProtocol
    {
        public static void GetDocs()
        {
            try
            {
                //az alap adatok megszerzése
                string[] splitter = DocsDataOperations.GetDatasSplitter();

                //ha a gépelérhetőségét vizsgáljuk
                if (splitter == null)
                {
                    //akkor folytassa
                    return;
                }

                int monitorIndex = DocsDataOperations.GetMonitorIndex(splitter);

                DocsShowClient.Standards = new List<string>();

                for (int i = 0; i < 2; i++)
                {
                    GetDocsDataMethod.GetDocsData(i, monitorIndex, splitter);
                }

                //a doksi adatok végén a vég protokoll elküldése
                DocsDataOperations.SendEndOfDocs();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"{ DateTime.Now.ToString()}:{ ex.Message}");

                Logger.MakeLog(ex.Message);
            }
            catch (NullReferenceException)
            {
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"{ DateTime.Now.ToString()}:{ ex.Message}");
                Logger.MakeLog(ex.Message);
            }
            finally
            {
                DocsShowClient.InPDF = false;
            }
        }
    }
}
