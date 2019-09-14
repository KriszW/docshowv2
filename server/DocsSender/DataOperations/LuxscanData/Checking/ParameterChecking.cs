using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class ParameterChecking
    {
        //megnézzi, hogy a paraméter nem-e fake adat
        public static void CheckParamValidate(string param)
        {
            if (param is "")
                throw new ArgumentException(nameof(param),"A paraméter nem lehet üres!");
            if (param == null)
                throw new ArgumentNullException(nameof(param), "A paraméter nem lehet null!");
        }
        //megnézzi, hogy a gépekből nem-e jött vissza fake adat
        public static void CheckgepInfosValidate(string[] gepRaw, string asztal)
        {
            if (gepRaw == null)
                throw new ApplicationException( $"A monitor száma: { asztal } érvénytelen, nem található egy gép sem ilyen monitor számhoz csatolva!");
        }
        //megnézzi, hogy a pdfekből nem-e jött vissza fake adat
        public static void CheckpdfInfosValidate(string[] pdfRaw, string cikk, string asztal)
        {
            if (pdfRaw == null) {
                CommonDatas.GUI.DGVManager.ReplaceInfo(asztal, "Nincs PDF", "Nincs PDF", cikk);
                throw new ApplicationException( $"A cikkhez: { cikk } nincs pdf társítva a(z) { asztal }. asztalon");
            }
        }
    }
}
