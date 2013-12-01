using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace iCrawler.ServiceLayer
{
    public static class NetworkService
    {
        public static bool IsConnectedToDBServer()
        {

            Uri url = new Uri("http://chuyentin.vn");
            string pingurl = string.Format("{0}", url.Host);
            string host = pingurl;
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }
            return result;

        }

        public static bool IsConnectedToInternet()
        {
            
                Uri url = new Uri("www.abhisheksur.com");
                string pingurl = string.Format("{0}", url.Host);
                string host = pingurl;
                bool result = false;
                Ping p = new Ping();
                try
                {
                    PingReply reply = p.Send(host, 3000);
                    if (reply.Status == IPStatus.Success)
                        return true;
                }
                catch { }
                return result;
            
        }

        public static bool IsConnectedToBIDV()
        {
           
                Uri url = new Uri("http://bidvportal.vn");
                string pingurl = string.Format("{0}", url.Host);
                string host = pingurl;
                bool result = false;
                Ping p = new Ping();
                try
                {
                    PingReply reply = p.Send(host, 3000);
                    if (reply.Status == IPStatus.Success)
                        return true;
                }
                catch { }
                return result;
           
        }
    }
}
