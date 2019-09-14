using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxScanOrdReader
{


    public delegate void OnLuxScanFileCopyError(object sender, FileCountArgs args);
    public delegate void OnSuccessFileCopy(object sender,SuccessFileCopyArgs args );
    public class FileCountArgs : EventArgs
    {
        public FileCountArgs(string[] filenames, int count)
        {
            Count = count;
            FileNames = filenames;
        }

        public int Count { get; private set; }

        public string[] FileNames { get; private set; }
    }

    public class SuccessFileCopyArgs : EventArgs
    {
        public SuccessFileCopyArgs(FileInfo ordFile)
        {
            NewFile = ordFile;
            ReadTime = DateTime.Now;
        }
        public SuccessFileCopyArgs(FileInfo ordFile,DateTime date)
        {
            NewFile = ordFile;
            ReadTime = date;
        }

        public FileInfo NewFile { get; private set; }

        public DateTime ReadTime { get; private set; }
    }
}
