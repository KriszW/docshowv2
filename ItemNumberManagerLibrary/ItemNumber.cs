using System;
using System.Collections.Generic;

namespace ItemNumberManager
{
    public class ItemNumber
    {
        public ItemNumber(string itemNum, string itemName, string mask, string[] pDfNames)
        {
            Material = itemNum ?? throw new ArgumentNullException(nameof(itemNum));
            MaterialName = itemName ?? throw new ArgumentNullException(nameof(itemName));
            Mask = mask ?? throw new ArgumentNullException(nameof(mask));

            PDFs = new List<PDFModel>();

            AddNewPDFs(pDfNames);
        }

        public ItemNumber(string itemNum, string itemName)
        {
            Material = itemNum ?? throw new ArgumentNullException(nameof(itemNum));
            MaterialName = itemName ?? throw new ArgumentNullException(nameof(itemName));

            PDFs = new List<PDFModel>();
        }

        public void AddNewPDFs(string[] pdfNames)
        {
            foreach (var item in pdfNames)
            {
                PDFs.Add(new PDFModel(item));
            }
        }

        public string Material { get; private set; }
        public string MaterialName { get; private set; }
        public string Mask { get; set; }
        public List<PDFModel> PDFs { get; private set; }
    }
}