using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocsShowServer
{
    class LuxScanFileOperations
    {
        //luxscan file error végrehajtása
        void WriteLuxscanFileError(string directoryPath)
        {
            //a fájlok lekérdezése
            string[] LuxscanDataPaths = Directory.GetFiles(directoryPath);

            //linq query készítés
            var paths = from path in LuxscanDataPaths
                        where path.EndsWith(".ord")
                        select path;

            //az errormsg inicializálása
            string errMSG = "";

            //a fájlútak száma, ami .ordra végződik
            int pathCount = paths.Count();

            //ha 0 akkor nincs, ha nem 0 akkor több van mint 1
            if(pathCount == 0)
            {
                errMSG = $"A .ord fájl nem található a helyén";
            }
            else
            {
                errMSG = $"A .ord fájlból több is megtalálható, ezért nem folytatható a doksi küldés, töröljétek azt amelyik nem kell!!";
            }

            //a file error beállítása
            CommonDatas.GUI.SetFileError(errMSG);

            //a hasluxscanfile igazra állítása
            CommonDatas.HasLuxscanFile = false;

            //az elérésiút beállítása
            CommonDatas.LuxscanFilePath = "";
        }

        //a luxscan file kiolvasása egy megadott útvonalról
        public void ReadLuxScanFilePath(string directoryPath)
        {
            //fájlok beolvasása
            string[] LuxscanDataPaths = Directory.GetFiles(directoryPath);

            //query
            var paths = from path in LuxscanDataPaths
                        where path.EndsWith(".ord")
                        select path;

            //ha egy van akkor jól mükődik
            if (paths.Count() == 1) {
                CommonDatas.HasLuxscanFile = true;

                CommonDatas.LuxscanFilePath = paths?.First();
            }
            else {
                WriteLuxscanFileError(directoryPath);
            }
        }

        public void ReadLuxScanFilePathFromExePath()
        {
            //a fileok beolvasása az exe fájl helyéről
            string[] LuxscanDataPaths = Directory.GetFiles(Directory.GetCurrentDirectory());

            //query
            var paths = from path in LuxscanDataPaths
                        where path.EndsWith(".ord")
                        select path;

            //ha egy van akkor jól mükődik
            if(paths.Count() == 1) {
                CommonDatas.HasLuxscanFile = true;

                CommonDatas.LuxscanFilePath = paths?.First();
            }
            else {

                WriteLuxscanFileError(Directory.GetCurrentDirectory());
            }
        }
    }
}
