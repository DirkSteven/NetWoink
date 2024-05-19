using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetworkMonitor
{
    public class NetworkScan
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(60);

        public static List<string> ScanNetworkDevices(string networkAddressString, string subnetMaskString)
        {
            List<string> devices = new List<string>();

            IPAddress networkAddress = IPAddress.Parse(networkAddressString);
            IPAddress subnetMask = IPAddress.Parse(subnetMaskString);

            byte[] networkAddressBytes = networkAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            // Calculate the range of IP addresses to scan
            byte[] startIpBytes = new byte[networkAddressBytes.Length];
            byte[] endIpBytes = new byte[networkAddressBytes.Length];
            for (int i = 0; i < networkAddressBytes.Length; i++)
            {
                startIpBytes[i] = (byte)(networkAddressBytes[i] & subnetMaskBytes[i]);
                endIpBytes[i] = (byte)(networkAddressBytes[i] | ~subnetMaskBytes[i]);
            }

            Console.WriteLine($"Scanning IP addresses from {new IPAddress(startIpBytes)} to {new IPAddress(endIpBytes)}...");

            using (var cancellationTokenSource = new CancellationTokenSource(Timeout))
            {
                var cancellationToken = cancellationTokenSource.Token;

                // Scan each IP address in the range
                for (byte b1 = startIpBytes[0]; b1 <= endIpBytes[0]; b1++)
                {
                    for (byte b2 = startIpBytes[1]; b2 <= endIpBytes[1]; b2++)
                    {
                        for (byte b3 = startIpBytes[2]; b3 <= endIpBytes[2]; b3++)
                        {
                            for (byte b4 = startIpBytes[3]; b4 <= endIpBytes[3]; b4++)
                            {
                                if (cancellationToken.IsCancellationRequested)
                                {
                                    Console.WriteLine("Scan aborted due to timeout.");
                                    return devices;
                                }

                                IPAddress ipAddress = new IPAddress(new byte[] { b1, b2, b3, b4 });
                                Console.WriteLine($"Scanning IP address: {ipAddress}");

                                // Perform ARP resolution for each IP address
                                string vendorName = ArpResolve(ipAddress);

                                if (vendorName != null)
                                {
                                    devices.Add($"IP: {ipAddress}, Vendor: {vendorName}");
                                    Console.WriteLine($"Found device: IP: {ipAddress}, Vendor: {vendorName}");
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("Scan completed.");
                return devices;
            }
        }

        private static async Task<bool> PingAddress(IPAddress ipAddress)
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = await ping.SendPingAsync(ipAddress, 1000); // 1000ms timeout
                    return reply.Status == IPStatus.Success;
                }
                catch (PingException)
                {
                    return false;
                }
            }
        }

        public static string ArpResolve(IPAddress ipAddress)
        {
            try
            {
                byte[] macAddressBytes = new byte[6];
                int macAddrLen = macAddressBytes.Length;
                int ret = NativeMethods.SendARP((int)ipAddress.Address, 0, macAddressBytes, ref macAddrLen);

                if (ret == 0)
                {
                    string macAddressString = BitConverter.ToString(macAddressBytes).Replace("-", ":");

                    // Check if MAC address is valid
                    if (!IsValidMACAddress(macAddressString))
                    {
                        Console.WriteLine($"Invalid MAC address: {macAddressString}");
                        return null;
                    }

                    // Pass the complete MAC address string to LookupVendorName method
                    string vendorName = LookupVendorName(macAddressString);

                    if (vendorName != null)
                    {
                        string deviceName = GetDeviceName(ipAddress);
                        return $"Device Name: {deviceName}, MAC: {macAddressString}, Vendor: {vendorName}";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"ARP resolution failed for IP address: {ipAddress}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during ARP resolution: {ex.Message}");
                return null;
            }
        }

        public static string GetLocalIPAddress()
        {
            // Get the local IPv4 address
            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }

            throw new Exception("Local IPv4 address not found.");
        }

        public static string GetSubnetMask(string ipAddressString)
        {
            // Get the subnet mask from the local network interface
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (unicastAddress.Address.ToString() == ipAddressString)
                    {
                        return unicastAddress.IPv4Mask.ToString();
                    }
                }
            }

            throw new Exception("Subnet mask not found.");
        }

        public static string GetNetworkAddress(string ipAddressString, string subnetMaskString)
        {
            // Calculate the network address based on the IP address and subnet mask
            byte[] ipAddressBytes = IPAddress.Parse(ipAddressString).GetAddressBytes();
            byte[] subnetMaskBytes = IPAddress.Parse(subnetMaskString).GetAddressBytes();

            byte[] networkAddressBytes = new byte[ipAddressBytes.Length];
            for (int i = 0; i < ipAddressBytes.Length; i++)
            {
                networkAddressBytes[i] = (byte)(ipAddressBytes[i] & subnetMaskBytes[i]);
            }

            return new IPAddress(networkAddressBytes).ToString();
        }

        private static string GetDeviceName(IPAddress ipAddress)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                return hostEntry.HostName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting device name: {ex.Message}");
                return "Unknown";
            }
        }

        private static string LookupVendorName(string macAddress)
        {
            try
            {
                // Format the MAC address string
                string formattedMAC = FormatMACAddress(macAddress);

                // Call the VendorAPI to get vendor information based on the formatted MAC address
                Console.WriteLine($"Sending request to macvendors.com API for MAC address: {formattedMAC}");
                var vendorInfoTask = VendorAPI.GetVendorInfo(formattedMAC);

                // Await for the response
                VendorClass vendorInfo = vendorInfoTask.Result;

                // Log the response from the API
                if (vendorInfo != null && vendorInfo.data != null)
                {
                    Console.WriteLine($"Received response from macvendors.com API: {vendorInfo}");
                    return vendorInfo.data.organization_name;
                }
                else
                {
                    Console.WriteLine($"No vendor information received from macvendors.com API.");
                    return "Unknown";
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the API request
                Console.WriteLine($"Error looking up vendor name: {ex.Message}");
                return "Unknown";
            }
        }

        private static string FormatMACAddress(string macAddress)
        {
            // Remove any non-hexadecimal characters
            string cleanedMAC = Regex.Replace(macAddress, "[^0-9A-Fa-f]", "");

            // Insert colons after every 2 characters
            return string.Join(":", Enumerable.Range(0, cleanedMAC.Length / 2).Select(i => cleanedMAC.Substring(i * 2, 2)));
        }

        private static bool IsValidMACAddress(string macAddress)
        {
            // Remove any non-hexadecimal characters
            string cleanedMAC = Regex.Replace(macAddress, "[^0-9A-Fa-f]", "");

            // Check if the cleaned MAC address has the correct length (12 characters)
            if (cleanedMAC.Length != 12)
            {
                return false;
            }

            // MAC address is considered valid
            return true;
        }


        // Native methods for ARP resolution
        private class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("iphlpapi.dll", EntryPoint = "SendARP")]
            internal extern static int SendARP(int destIp, int srcIp, byte[] pMacAddr, ref int phyAddrLen);
        }
    }
}