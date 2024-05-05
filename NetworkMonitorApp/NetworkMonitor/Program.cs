using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

class Program   
{


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

        // Scan each IP address in the range
        for (byte b1 = startIpBytes[0]; b1 <= endIpBytes[0]; b1++)
        {
            for (byte b2 = startIpBytes[1]; b2 <= endIpBytes[1]; b2++)
            {
                for (byte b3 = startIpBytes[2]; b3 <= endIpBytes[2]; b3++)
                {
                    for (byte b4 = startIpBytes[3]; b4 <= endIpBytes[3]; b4++)
                    {
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

        return devices;
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
                PhysicalAddress physicalAddress = new PhysicalAddress(macAddressBytes);
                string macAddressString = BitConverter.ToString(macAddressBytes).Replace("-", ":");
                string oui = macAddressString.Substring(0, 8); // Extract the first 3 bytes (OUI)
                string vendorName = LookupVendorName(oui);
                string deviceName = GetDeviceName(ipAddress);
                return $"Device Name: {deviceName}, MAC: {macAddressString}, Vendor: {vendorName}";
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

    public static string GetDeviceName(IPAddress ipAddress)
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

    public static string LookupVendorName(string oui)
    {
        // Lookup the vendor name using the OUI in a MAC address vendor database
        // You can implement this method by querying a database or using a pre-built lookup table.
        // For simplicity, let's assume a basic lookup table here.

        Dictionary<string, string> vendorLookupTable = new Dictionary<string, string>()
        {
            { "00-0C-29", "VMware" },
            { "00-50-56", "VMware" },
            // Add more entries as needed
        };

        if (vendorLookupTable.ContainsKey(oui))
        {
            return vendorLookupTable[oui];
        }
        else
        {
            return "Unknown";
        }
    }
}

internal static class NativeMethods
{
    [System.Runtime.InteropServices.DllImport("iphlpapi.dll", EntryPoint = "SendARP")]
    internal extern static int SendARP(int destIp, int srcIp, byte[] pMacAddr, ref int phyAddrLen);
}
