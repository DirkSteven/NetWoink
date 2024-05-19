using PcapDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace NetworkMonitor
{
    public partial class InitializeWindow : Window
    {
        public PacketDevice SelectedDevice { get; private set; }
        public event EventHandler<NetworkInterface> SelectedAdapterChanged;

        public InitializeWindow()
        {
            InitializeComponent();
            PopulateComboBox();
        }

        public class NetworkAdapterHelper
        {
            public static List<NetworkInterface> GetNetworkAdapters()
            {
                return NetworkInterface.GetAllNetworkInterfaces().ToList();
            }
        }

        private void PopulateComboBox()
        {
            List<NetworkInterface> adapters = NetworkAdapterHelper.GetNetworkAdapters();

            foreach (var adapter in adapters)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = adapter.Name;
                item.Tag = adapter; // Set the Tag property to the NetworkInterface object
                comboBox.Items.Add(item);
            }
        }





        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                NetworkInterface selectedAdapter = (NetworkInterface)((ComboBoxItem)comboBox.SelectedItem).Tag;
                Console.WriteLine("Selected adapter: " + selectedAdapter.Name); // Debugging statement
                SelectedAdapterChanged?.Invoke(this, selectedAdapter);
                this.Close();
            }
            else
            {
                MessageBox.Show("No network adapter selected.");
            }
        }





        private PacketDevice GetPacketDevice(NetworkInterface networkInterface)
        {
            // Check if networkInterface is null
            if (networkInterface == null)
            {
                Console.WriteLine("Error: NetworkInterface is null.");
                return null;
            }

            // Use PcapDotNet to get the corresponding PacketDevice for the NetworkInterface
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            // Check if allDevices is null or empty
            if (allDevices == null || allDevices.Count == 0)
            {
                Console.WriteLine("Error: No packet devices available.");
                return null;
            }

            Console.WriteLine($"Total number of devices: {allDevices.Count}");

            foreach (var device in allDevices)
            {
                // Check if device is null
                if (device == null)
                {
                    Console.WriteLine("Error: Device instance is null.");
                    continue;
                }

                Console.WriteLine($"Device Name: {device.Name}, Network Interface Name: {networkInterface.Name}");

                if (device.Name.Equals(networkInterface.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Device found!");
                    return device;
                }
            }

            Console.WriteLine("No matching device found!");
            return null; // If no matching PacketDevice is found, return null
        }






        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                if (comboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    NetworkInterface selectedAdapter = NetworkInterface.GetAllNetworkInterfaces()
                        .FirstOrDefault(adapter => adapter.Name == (string)selectedItem.Content);

                    if (selectedAdapter != null)
                    {
                        IPInterfaceProperties adapterProperties = selectedAdapter.GetIPProperties();
                        IPAddress ipAddress = adapterProperties.UnicastAddresses.FirstOrDefault()?.Address;
                        PhysicalAddress macAddress = selectedAdapter.GetPhysicalAddress();
                        GatewayIPAddressInformation gateway = adapterProperties.GatewayAddresses.FirstOrDefault();
                        string ssid = selectedAdapter.Name; // Assuming SSID is the same as adapter name for wired connections
                        string driverVersion = selectedAdapter.Description;

                        // Update labels with adapter information
                        UpdateLabelContent(nicTypeLabel, selectedAdapter.NetworkInterfaceType.ToString(), "NIC Type: ");
                        UpdateLabelContent(ipAddressLabel, ipAddress?.ToString(), "IP Address: ");
                        UpdateLabelContent(macAddressLabel, macAddress?.ToString(), "MAC Address: ");
                        UpdateLabelContent(gatewayLabel, gateway?.Address?.ToString(), "Gateway: ");
                        UpdateLabelContent(networkSSIDLabel, ssid, "Network SSID: ");
                        UpdateLabelContent(driverVersionLabel, driverVersion, "Driver Version: ");
                    }
                }
            }
        }



        private void UpdateLabelContent(Label label, string content, string labelText)
        {
            if (label != null)
            {
                label.Content = labelText + (string.IsNullOrEmpty(content) ? "N/A" : content);
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
