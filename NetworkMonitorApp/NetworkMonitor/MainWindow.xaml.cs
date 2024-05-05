using System.Net.NetworkInformation;
using System.Net;
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

namespace NetworkMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindow initWindow = new InitializeWindow();
            initWindow.OKButtonClicked += InitializeWindow_OKButtonClicked; // Subscribe to the OKButtonClicked event
            initWindow.ShowDialog(); // Show as a modal dialog
        }

        private void InitializeWindow_OKButtonClicked(object sender, NetworkInterface selectedAdapter)
        {
            // This method will be called when OK button is clicked in InitializeWindow.xaml
            // You can add your logic here to handle the selected network adapter
            // For now, let's just display the selected network adapter's information
            DisplayDeviceInfo(selectedAdapter);
        }

        private void DisplayDeviceInfo(NetworkInterface adapter)
        {
            // Assuming you have Labels named ipLabel, macLabel, nameLabel, and vendorLabel in your MainWindow.xaml

            IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
            IPAddress ipAddress = adapterProperties.UnicastAddresses.FirstOrDefault()?.Address;
            PhysicalAddress macAddress = adapter.GetPhysicalAddress();
            string name = adapter.Name;
            string vendor = adapter.Description;

            ipLabel.Content = "IP: " + (ipAddress != null ? ipAddress.ToString() : "N/A");
            macLabel.Content = "MAC: " + (macAddress != null ? macAddress.ToString() : "N/A");
            nameLabel.Content = "NAME: " + name;
            vendorLabel.Content = "VENDOR: " + vendor;
        }


        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            // This method will be called when the Scan button is clicked
            // You can add your scanning logic here
            // For now, let's just display information about the selected network adapter
            // You can call DisplayDeviceInfo with the selected adapter from InitializeWindow
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
        }



        private void SnifferBuutton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sniffer");
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
            MessageBox.Show("Devices");
        }
    }
}