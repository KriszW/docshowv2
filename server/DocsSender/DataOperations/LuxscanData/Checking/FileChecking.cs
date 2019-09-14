using System;
using System.IO;

namespace DocsShowServer
{
    static class FileChecking
    {
        public static void CheckFileExists(string[] pdfRaw)
        {
            for (int i = 1; i < pdfRaw.Length; i++)
            {
                string fileName = pdfRaw[i];
                string filePath = CommonDatas.PathtoResources + fileName;

                if (fileName != "")
                {
                    if (File.Exists(filePath + ".docx") || File.Exists(filePath + ".doc"))
                    {
                        throw new ApplicationException($"A {fileName} fájl nem érhető el PDF kiterjesztésben, csak .docx vagy .doc-ban.\nEzen az útvonalon: {filePath}");
                    }
                    if (!File.Exists(filePath + ".pdf"))
                    {
                        throw new ApplicationException($"Nem létezik a {fileName}.pdf fájl");
                    }
                }
            }
        }

    }
}
