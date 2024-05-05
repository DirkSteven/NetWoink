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

            // Create a new instance of PacketCapture with the provided NetworkInterface
            packetCapture = new PacketCapture(selectedAdapter, this);

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
       public void UpdatePacketListView(string sourceIp, string destinationIp, string sourceMac, string protocol, string packetType)
        {
            // Create a new PacketItem
            var packetItem = new PacketItem
            {
                SourceIpAddress = sourceIp,
                DestinationIpAddress = destinationIp,
                SourceMacAddress = sourceMac,
                Protocol = protocol,
                PacketType = packetType
            };

            // Add the PacketItem to the ObservableCollection
            packetItems.Add(packetItem);
        }






    }

    public class PacketItem
    {
        public string SourceIpAddress { get; set; }
        public string DestinationIpAddress { get; set; }
        public string SourceMacAddress { get; set; }
        public string Protocol { get; set; }
        public string PacketType { get; set; }
    }



}