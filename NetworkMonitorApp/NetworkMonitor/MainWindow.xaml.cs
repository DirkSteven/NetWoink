using NetworkMonitor;
using System.Windows;
using System.Net.NetworkInformation;
using System.Net;
using System.Collections.Generic;

using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Diagnostics;
using PcapDotNet.Core;
using System.ComponentModel;
using System.Collections.ObjectModel;
using SharpPcap;


namespace NetworkMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Device> devices = new ObservableCollection<Device>();
        private NetworkInterface selectedAdapter;
        private bool networkScanned = false;
        private bool deviceSelected = false;
        private SnifferWindow snifferWindow;
        private PacketDevice selectedDevice;
        private PacketCapture packetCapture;

        public MainWindow()
        {
            //InitializeComponent();
            //DeviceListView.ItemsSource = devices; // Bind the collection to the ListView

            //InitializeWindow InitWindow = new InitializeWindow();
            //InitWindow.SelectedAdapterChanged += (sender, adapter) =>
            //{
            //    selectedAdapter = adapter;
            //};
            //InitWindow.ShowDialog();
            InitializeComponent();
            DeviceListView.ItemsSource = devices; // Bind the collection to the ListView

            InitializeWindow InitWindow = new InitializeWindow();
            InitWindow.SelectedAdapterChanged += InitWindow_OKButtonClicked; // Subscribe to the event
            InitWindow.ShowDialog();
        }

        private void InitWindow_OKButtonClicked(object sender, NetworkInterface selectedAdapter)
        {
            if (selectedAdapter != null)
            {
                // Convert the selected adapter to PacketDevice and store it
                this.selectedAdapter = selectedAdapter;

                // Debugging statement to print the received device
                Console.WriteLine("Selected adapter received in MainWindow: " + selectedAdapter.Name);
                Console.WriteLine("Selected adapter description: " + selectedAdapter.Description);
                Console.WriteLine(selectedAdapter);

                // Debugging statement to print the received device
                Console.WriteLine("Received device in MainWindow: " + selectedAdapter);

                // Create a new instance of SnifferWindow and pass the selected adapter

                //snifferWindow = new SnifferWindow(selectedAdapter, new PacketCapture(selectedAdapter, snifferWindow));

                snifferWindow = new SnifferWindow(selectedAdapter);
                PacketCapture packetCapture = new PacketCapture(selectedAdapter, snifferWindow);
                snifferWindow = new SnifferWindow(selectedAdapter, packetCapture);

            }
            else
            {
                // Handle the case where selectedAdapter is null
                Console.WriteLine("Error: selectedAdapter is null.");
            }
        }
        

        private PacketDevice GetPacketDevice(NetworkInterface networkInterface)
        {
            // You need to implement this method to convert NetworkInterface to PacketDevice
            // This method depends on your specific implementation, such as using PcapDotNet or other libraries
            // This method is just a placeholder, and you need to replace it with your actual implementation
            // Here's an example of how it might look:
            // return PcapDotNet.Core.SomeMethodToGetPacketDevice(networkInterface);
            // Replace SomeMethodToGetPacketDevice with the actual method you use to get PacketDevice
            return null; // Placeholder, replace with actual implementation
        }
        public MainWindow(PacketDevice selectedDevice)
        {
            InitializeComponent();
            this.selectedDevice = selectedDevice;
        }







        // Device class
        public class Device : INotifyPropertyChanged
        {
            private bool isSelected;
            public bool IsSelected
            {
                get { return isSelected; }
                set
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                    Console.WriteLine($"Checkbox clicked for device: {DeviceName}, IsSelected: {isSelected}");
                }
            }

            public string IP { get; set; }
            public string MAC { get; set; }
            public string DeviceName { get; set; }
            public string Vendor { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void DeviceCheckbox_Click(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox != null && checkbox.DataContext is Device device)
            {
                // Update the device's IsSelected property
                device.IsSelected = checkbox.IsChecked ?? false;
            }
        }

        // MainWindow.xaml.cs



        private async void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear existing devices
            devices.Clear();

            // Get local IP address and subnet mask
            string localIPAddress = NetworkScan.GetLocalIPAddress();
            string subnetMask = NetworkScan.GetSubnetMask(localIPAddress);
            string networkAddress = NetworkScan.GetNetworkAddress(localIPAddress, subnetMask);

            // Scan network devices asynchronously
            List<string> scannedDevices = await Task.Run(() => NetworkScan.ScanNetworkDevices(networkAddress, subnetMask));

            // Populate the devices collection
            foreach (string deviceInfo in scannedDevices)
            {
                string[] parts = deviceInfo.Split(new string[] { ", " }, StringSplitOptions.None);
                string ip = parts[0].Trim().Substring(3);
                string mac = parts[2].Trim().Substring(4);
                string deviceName = parts[1].Trim().Substring(20);
                string vendor = parts[3].Trim().Substring(8);

                // Create a new Device object and add it to the collection
                devices.Add(new Device
                {
                    IP = ip,
                    MAC = mac,
                    DeviceName = deviceName,
                    Vendor = vendor
                });
            }

            networkScanned = true;
        }

        private void DeviceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DeviceListView.SelectedItem != null)
            {
                // A device is selected
                deviceSelected = true;
            }
            else
            {
                // No device selected
                deviceSelected = false;
            }

            // Update UI or perform other actions if needed
        }







        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
        }


        private void SnifferButton_Click(object sender, RoutedEventArgs e)
        {
            // Find the selected device
            Device selectedDevice = null;
            foreach (var device in devices)
            {
                if (device.IsSelected)
                {
                    selectedDevice = device;
                    break;
                }
            }

            if (selectedDevice != null)
            {
                // Ensure that snifferWindow is not null and has been initialized
                if (snifferWindow != null)
                {
                    // Pass the selected device's IP address to the existing SnifferWindow instance
                    snifferWindow.SnifferWindowGetIP(selectedDevice.IP);
                    snifferWindow.Show();
                }
                else
                {
                    MessageBox.Show("Error: Sniffer window has not been initialized.");
                }
            }
            else
            {
                MessageBox.Show("Please select a device before starting the sniffer.");
            }
        }














        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Option");
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow abtWindow = new AboutWindow();
            abtWindow.Show();
        }

        private void DevicesButton_Click(object sender, RoutedEventArgs e)
        {

            
            
            Device selectedDevice = null;
            foreach (var device in devices)
            {
                if (device.IsSelected)
                {
                    selectedDevice = device;
                    break;
                }
            }
            CFWindow cfWindow = new CFWindow(selectedDevice.IP,selectedDevice.MAC);
            cfWindow.Show();    
        }


        
    }
}