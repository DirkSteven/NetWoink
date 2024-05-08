using System.Windows;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using SharpPcap;
using PcapDotNet.Core;
using System.Net;
using System.Windows.Controls;


namespace NetworkMonitor
{
    public partial class SnifferWindow : Window
    {

        private PacketCapture packetCapture;
        private SnifferWindow selectAdapter;
        private string selectedIP;

        private ObservableCollection<PacketItem> packetItems = new ObservableCollection<PacketItem>();

        public ObservableCollection<PacketItem> PacketItems
        {
            get { return packetItems; }
            set { packetItems = value; }
        }




        public void SnifferWindowGetIP(string selectedIP)
        {
            Console.WriteLine(selectedIP);
            packetCapture.sourceIP = selectedIP;
            this.selectedIP = selectedIP;

            Console.WriteLine("selIPsaPACKCAPTURE:" + packetCapture.sourceIP);

        }

        public SnifferWindow(NetworkInterface selectedAdapter)
        {
            InitializeComponent();
            DataContext = this;



            // Create a new instance of PacketCapture with the provided NetworkInterface
            packetCapture = new PacketCapture(selectedAdapter, this);
            packetItems.Clear();
        }


        public SnifferWindow(NetworkInterface selectedAdapter, PacketCapture packetCapture)
        {
            InitializeComponent();
            Console.WriteLine("SNIFF WINN:" + selectedAdapter);
            this.packetCapture = packetCapture;
            Console.WriteLine("PACKetCAPS:" + packetCapture);

        }



        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StatusUpdate.Content = "Status: Running";
            MessageBox.Show("Packet Capturing has started");
            if (packetCapture != null)
            {
                // Start capturing packets on a separate thread
                await Task.Run(() => packetCapture.StartCapture(selectedIP));
            }
            else
            {
                MessageBox.Show("PacketCapture is not initialized. Make sure to initialize it before starting the capture.");
            }
        }
        public void UpdatePacketListView(string sourceIp, string destinationIp, string sourceMac, string protocol, string packetType, string destinationHost)
        {
  
            // Create a new PacketItem
            var packetItem = new PacketItem
            {
                SourceIpAddress = sourceIp,
                DestinationIpAddress = destinationIp,
                SourceMacAddress = sourceMac,
                Protocol = protocol,
                PacketType = packetType,
                DestinationHostName = destinationHost
            };

            // Add the PacketItem to the ObservableCollection
            packetItems.Add(packetItem);    
            PacketListView.Items.Refresh();


            Console.WriteLine($"Added packet item: {packetItem.SourceIpAddress}, {packetItem.DestinationIpAddress}, {packetItem.SourceMacAddress}, {packetItem.Protocol}, {packetItem.PacketType}, {packetItem.DestinationHostName}");
            
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop the packet capture
            packetCapture.StopCapture();
            StatusUpdate.Content = "Status: Idle";
            MessageBox.Show("Packet Capturing has stopped");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

            Visibility = Visibility.Hidden;
        }
    }

    public class PacketItem
    {
        public string SourceIpAddress { get; set; }
        public string DestinationIpAddress { get; set; }
        public string SourceMacAddress { get; set; }
        public string Protocol { get; set; }
        public string PacketType { get; set; }
        public string DestinationHostName{ get; set; }
    }



}