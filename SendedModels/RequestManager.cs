using SendedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class RequestManager
    {

        public RequestManager(Request request, IRequestMethods methods)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));

            Methods = methods;
        }
        public Request Request { get; private set; }

        public IRequestMethods Methods { get; private set; }

        public byte[] ManageRequest()
        {
            Request.ArrivedDate = DateTime.Now;

            var output = ExecuteRequest();

            EndRequest();

            return output;
        }

        System.Reflection.MethodInfo GetMethod()
        {
            var method = typeof(IRequestMethods).GetMethod(Request.Command.ToString());

            return method;
        }

        byte[] ExecuteRequest()
        {
            var method = GetMethod();

            if (method != default)
            {
                Request.State = RequestState.Executing;

                var data = (byte[])method?.Invoke(Methods, new object[] { Serializer.Deserialize(Request.Data) });

                if (data != default)
                {
                    if (data.Length > 0)
                    {
                        data = Serializer.Serialize(data);
                    }
                    else
                    {
                        data = Serializer.Serialize(Request);
                    }
                }
                
                return (byte[])data;
            }
            else
            {
                Request.State = RequestState.Unknow;
                return default;
            }
        }

        void EndRequest()
        {
            Request.State = RequestState.Finished;
            Request.FinishedDate = DateTime.Now;
        }
    }
}
