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
            initWindow.Show();
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {

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
            MessageBox.Show("About");
        }

        private void DevicesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Devices");
        }
    }
}