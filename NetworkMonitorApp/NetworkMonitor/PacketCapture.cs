    using System;
    using System.Windows;

    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using PcapDotNet.Core;
    using PcapDotNet.Packets;
    using PcapDotNet.Packets.IpV4;
    using PcapDotNet.Packets.Transport;
    using static NetworkMonitor.SnifferWindow;
    using System.Net;

    namespace NetworkMonitor
    {
        public class PacketCapture
        {
            private PacketDevice selectedAdapter;
            private SnifferWindow snifferWindow;
            public string sourceIP;
            private PacketCommunicator communicator;

        //private ObservableCollection<PacketInfo> packetList;


        public bool isInitialized = true;


            public void checkSoure()
            {
                Console.WriteLine("SOURCE:" + sourceIP);
            }





            public PacketCapture(NetworkInterface adapter, SnifferWindow snifferWindow)
            {
                this.snifferWindow = snifferWindow;

                // Attempt to find a matching PacketDevice
                selectedAdapter = GetPacketDevice(adapter);

                if (selectedAdapter != null)
                {
                    Console.WriteLine("Selected PacketDevice: " + selectedAdapter);
                }
                else
                {
                    Console.WriteLine("No matching PacketDevice found for the provided NetworkInterface.");
                }
            }


            private PacketDevice GetPacketDevice(NetworkInterface adapter)
            {
                // Get all available packet devices
                var allDevices = LivePacketDevice.AllLocalMachine;

                // Find the device that contains the adapter's description or name
                var matchingDevice = allDevices.FirstOrDefault(device =>
                    device.Description.Contains(adapter.Description) || device.Name.Contains(adapter.Name));

                return matchingDevice;
            }


            public void StartCapture(string sourceIP)
            {
                this.sourceIP = sourceIP;

                // Ensure the packet capture is initialized
                if (!isInitialized)
                {
                    Console.WriteLine("PacketCapture is not initialized. Call Initialize method before starting the capture.");
                    return;
                }

                // Start capturing packets from the specified source IP using selectedAdapter
                Console.WriteLine($"Starting packet capture on source IP: {sourceIP}");

            // Open the selected adapter
            using (communicator = selectedAdapter.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                    // Set the filter to capture only packets from the specified source IP
                    communicator.SetFilter($"src host {sourceIP}");

                    // Start capturing packets
                    communicator.ReceivePackets(0, OnPacketArrival);


                
                }
            }
            private void OnPacketArrival(Packet packet)
            {
                // Extract necessary information from the packet
                string sourceIp = null;
                string destinationIp = null;
                string sourceMac = null;
                string protocol = null;
                string packetType = null;

                string sourceHostname = null; // Declaring sourceHostname here
                string destinationHostname = null; // Declaring destinationHostname here

                if (packet.Ethernet != null && packet.Ethernet.IpV4 != null)
                {
                    var ethernet = packet.Ethernet;
                    var ip = packet.Ethernet.IpV4;

                    sourceIp = ip.Source.ToString();
                    destinationIp = ip.Destination.ToString();
                    sourceMac = ethernet.Source.ToString();
                    protocol = ip.Protocol.ToString();
                    packetType = (ip.Tcp != null ? "TCP" : ip.Udp != null ? "UDP" : "Unknown");

                    // Perform reverse DNS lookup for source and destination IPs
                    sourceHostname = ResolveHostname(sourceIp); // Assigning value to sourceHostname
                    destinationHostname = ResolveHostname(destinationIp); // Assigning value to destinationHostname
                }

                // Print out captured packet information to the console
                Console.WriteLine($"Source IP: {sourceIp}, Source Hostname: {sourceHostname}, Destination IP: {destinationIp}, Destination Hostname: {destinationHostname}, " +
                                  $"Source MAC: {sourceMac}, Protocol: {protocol}, " +
                                  $"Packet Type: {packetType}");

                // Update UI with captured packet information if the window is initialized and Dispatcher is not null
                if (snifferWindow != null && snifferWindow.Dispatcher != null)
                {
                    Console.WriteLine($"UI Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                    snifferWindow.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        snifferWindow.UpdatePacketListView(sourceIp, destinationIp, sourceMac, protocol, packetType, destinationHostname);
                    }));
                }
                else
                {
                    Console.WriteLine("Sniffer window is not initialized or Dispatcher is null.");
                }
            }


            private string ResolveHostname(string ipAddress)
            {
                try
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(IPAddress.Parse(ipAddress));
                    return hostEntry.HostName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error resolving hostname for IP {ipAddress}: {ex.Message}");
                    return ipAddress; // Return IP address if hostname resolution fails
                }
            }





        public void StopCapture()
        {
            // You need to add logic here to stop the packet capture process.
            // One way to do this is to dispose of the PacketCommunicator object
            // that is used for capturing packets.
            

            Console.WriteLine("Stopping packet capture...");

            // If the PacketCommunicator is not null, dispose it to stop capturing packets


            Console.WriteLine("Packet capture stopped.");
        }




    }
    }