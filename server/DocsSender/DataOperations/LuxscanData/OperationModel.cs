using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsShowServer
{
    //az metodúsok incialízálása
    class OperationModel
    {
        public static DataChecking DataChecking = new DataChecking();
        public static WaitingMethods Waiting = new WaitingMethods();
        public static DataManipulating DataManipulating = new DataManipulating(DataChecking);
        public static MakeLinesFromData LineMaker = new MakeLinesFromData();
        public static ItemOperations ItemOperations = new ItemOperations();
        public static DoubleLineOperations DoubleLineOperations = new DoubleLineOperations();
        public static SendingOperations SendingOperations = new SendingOperations();
    }
}
