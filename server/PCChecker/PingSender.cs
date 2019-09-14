using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace DocsShowServer
{
    class PingSender
    {
        public static bool IsItOn(string ip)
        {
            PingReply reply = null;

            Ping pinger = new Ping();

            int timeout = 2000;

            Task pingerTask = Task.Run(() => { reply = pinger.Send(ip, timeout); });

            pingerTask.Wait();

            if (reply == null)
            {
                return false;
            }
            else
            {
                return reply.Status == IPStatus.Success ? true : false;
            }
        }

    }
}
