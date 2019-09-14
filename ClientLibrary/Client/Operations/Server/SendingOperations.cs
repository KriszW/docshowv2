using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class SendingOperations
    {

        public static void WriteMSG(string msg)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(msg);

                DocsShowClient.ClientStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
