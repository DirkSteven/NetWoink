using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace NetworkMonitor
{
    public static class NetworkScanner
    {
        public static List<Tuple<IPAddress, PhysicalAddress>> Scan()
        {
            List<Tuple<IPAddress, PhysicalAddress>> devices = new List<Tuple<IPAddress, PhysicalAddress>>();

            Parallel.ForEach(NetworkInterface.GetAllNetworkInterfaces(), networkInterface =>
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            string ipBase = ip.Address.ToString().Substring(0, ip.Address.ToString().LastIndexOf('.') + 1);

                            for (int i = 1; i < 255; i++)
                            {
                                string ipToPing = ipBase + i.ToString();
                                PingReply reply = new Ping().Send(ipToPing, 100);
                                if (reply.Status == IPStatus.Success)
                                {
                                    devices.Add(Tuple.Create(IPAddress.Parse(ipToPing), GetMacAddress(ipToPing)));
                                }
                            }
                        }
                    }
                }
            });

            return devices;
        }

        private static PhysicalAddress GetMacAddress(string ipAddress)
        {
            // Implement code to get MAC address from IP address
            // For example:
            return PhysicalAddress.None;
        }
    }
}
