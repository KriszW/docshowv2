using IOs;
using KilokoModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocsShowServer
{
    class DataOperations
    { 
        //egy kilokomodel visszaadása az kiloko érték alapján
        public static IKilokoModel GetKilokoModel(string asztal)
        {
            var query = from kiloko in CommonDatas.Kilokok where kiloko.Kiloko == asztal select kiloko;

            int count = query.Count();

            if (count==1)
            {
                return query.First();
            }
            else if(count > 1)
            {
                throw new ApplicationException($"A {asztal}. momitorhoz több klienshez is csatolva van");
            }
            else
            {
                return null;
            }
        }

        //az összes cikk törlése a kilőkőmodelekből
        public static void DeleteAllCikkFromModels()
        {
            foreach (var kiloko in CommonDatas.Kilokok)
            {
                kiloko.DeleteAllCikk();
            }
        }

        //új cikk hozzáadása a kilőkőmodelhez
        public static void AddNewCikkToModel(string cikk,string toGUI, string kiloko)
        {
            IKilokoModel kilokoModel = GetKilokoModel(kiloko);

            if (kilokoModel!=null)
            {
                if (!kilokoModel.Cikkek.Contains(cikk))
                {
                    kilokoModel.AddNewCikk(cikk, toGUI);
                }
            }
        }

        //beolvassa a pdfeket a dupla cikk egy asztalokra
        public static List<string[]> MakePDFRaws(string[] cikkek)
        {
            return new List<string[]>() {
                GetSearchedData(cikkek[0], CommonDatas.PathtoCikkek),
                GetSearchedData(cikkek[1], CommonDatas.PathtoCikkek)
            };
        }

        //megcsinálja a pdfeket a továbbküldésre a két cikk egy asztalra-hoz
        public static string[] MakeCompliedPDFRaw(List<string[]> pdfRaws)
        {
            string[] pdfRaw1 = pdfRaws[0];
            string[] pdfRaw2 = pdfRaws[1];

            //ha jobb oldalra kerül a monitoron
            if (pdfRaw1[0] == null)
            {
                return (pdfRaw2[0]+";;"+pdfRaw2[1]).Split(';');
            }

            return (pdfRaw1[0] + CommonDatas.MarkBetweenTwoCikk + pdfRaw2[0] + ";" + pdfRaw1[1] + ";" + pdfRaw2[1]).Split(';');
        }

        //gépinfók kiolvasása ellenőrive
        public static string[] GetSearchedData(string searchFor, string path)
        {
            //paraméterek ellenőrzése
            ParameterChecking.CheckParamValidate(searchFor);
            ParameterChecking.CheckParamValidate(path);

            //az adatok beolvasása
            List<string> datas = FileOperations.Read(path);

            //ha null akkor sikertelen volt a beolvasás
            if (datas == null)
            {
                //addig várjon amíg nem olvasható
                DataChecking.CheckTheExcelIsOpened(path,true);

                //ha olvasható már akkor olvassa be újra
                datas = FileOperations.Read(path);
            }

            //megkeresi a megfelelő adatokat és visszaadja return ként
            foreach (var line in datas)
            {
                string[] Infos = line.Split(';');

                if (Infos[0] == searchFor)
                {
                    return Infos;
                }
            }

            //ha nem talált akkor null-t add vissza
            return null;
        }
    }
}
