using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace NetworkMonitor
{
    public class NetworkAdapterHelper
    {
        public static List<NetworkInterface> GetNetworkAdapters()
        {
            return NetworkInterface.GetAllNetworkInterfaces().ToList();
        }
    }
}
