using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace NetworkMonitor
{
    public class NetworkAdapterHelper
    {
        public static List<NetworkInterface> GetNetworkAdapters()
        {
            return NetworkInterface.GetAllNetworkInterfaces().ToList();
        }
    }

    public partial class InitializeWindow : Window
    {
        public delegate void OKButtonClickedEventHandler(object sender, NetworkInterface selectedAdapter);


        public event OKButtonClickedEventHandler OKButtonClicked;


        public InitializeWindow()
        {
            InitializeComponent();
            PopulateComboBox();
        }

        private void PopulateComboBox()
        {
            List<NetworkInterface> adapters = NetworkAdapterHelper.GetNetworkAdapters();

            foreach (var adapter in adapters)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = adapter.Name;
                comboBox.Items.Add(item);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                string selectedAdapterName = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
                NetworkInterface selectedAdapter = NetworkAdapterHelper.GetNetworkAdapters()
    .FirstOrDefault(adapter => adapter.Name == selectedAdapterName);


                // Raise the event with the selected network adapter
                OKButtonClicked?.Invoke(this, selectedAdapter);

                // Close the window
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a network adapter.");
            }
        }



        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                string selectedAdapterName = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
                NetworkInterface selectedAdapter = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(adapter => adapter.Name == selectedAdapterName);

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

        private void UpdateLabelContent(Label label, string content, string labelText)
        {
            if (label != null)
            {
                label.Content = labelText + (string.IsNullOrEmpty(content) ? "N/A" : content);
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
