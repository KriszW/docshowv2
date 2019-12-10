using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositioningLib
{

    [Serializable]
    public class PDFNotFoundException : Exception
    {
        public PDFNotFoundException(string message, string filePath) : base(message) { FileName = filePath; }
        public PDFNotFoundException(string message, Exception inner, string filePath) : base(message, inner) { FileName = filePath; }
        protected PDFNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context, string filePath) : base(info, context) { FileName = filePath; }

        public string FileName { get; set; }

        public PDFNotFoundException()
        {
        }

        public PDFNotFoundException(string message) : base(message)
        {
        }

        public PDFNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PDFNotFoundException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
