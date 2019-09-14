using System.Collections.Generic;
using System.Threading;

namespace DocsShowServer
{
    class SendingServerMethods
    { 
        public static void SendFirstDocs(List<string> asztalSzamok)
        {
            //a luxscanfile frissitése
            LuxScanFileGetter.SetLuxscanFilePath();

            //ha nincs beállítva akkor ne küldje el
            if (!CommonDatas.HasLuxscanFile)
            {
                return;
            }

            //a kilőkő modelen a cikkek frissitése
            OperationModel.LineMaker.SetLineModel(asztalSzamok);

            //végig menni az asztalokon és aztán elküldeni rájuk a megfelelő cikket
            foreach (var kilokoSzama in asztalSzamok) {
                IKilokoModel model = DataOperations.GetKilokoModel(kilokoSzama);

                OperationModel.SendingOperations.ManageCikks(model);
                CommonDatas.GUI.Setter.SetKilokoOnline(kilokoSzama);
            }
        }
    }
}
