using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace NetworkMonitor
{
    /// <summary>
    /// Interaction logic for CFWindow.xaml
    /// </summary>
    public partial class CFWindow : Window
    {
        private Dictionary<string, string> macManufacturerMap;

        // Power consumption of the device in watts
        private double powerConsumption;

        // Runtime of the device in hours per day
        private double runtimePerDay;

        // Carbon intensity of electricity in kgCO2/kWh
        private double carbonIntensity;

        public CFWindow()
        {
            InitializeComponent();
            LoadOuiDatabase(); // Load OUI database on startup
            Version clrVersion = Environment.Version;
           
            // Initialize values (you can retrieve these from user input or configuration)
            powerConsumption = 50; // Example: 50 watts
            runtimePerDay = 4; // Example: 4 hours
            carbonIntensity = 0.5; // Example: 0.5 kgCO2/kWh

            // Calculate annual carbon footprint
            double annualEnergyConsumption = CalculateAnnualEnergyConsumption(powerConsumption, runtimePerDay);
            double annualCarbonFootprint = CalculateAnnualCarbonFootprint(annualEnergyConsumption, carbonIntensity);

            string runtime = GetRuntimeName(clrVersion);
            runtimeLbl.Content = runtime;


        }
        private void LoadOuiDatabase()
        {
            macManufacturerMap = new Dictionary<string, string>();

            // Assuming oui.txt is the downloaded OUI database
            string[] lines = File.ReadAllLines("Resources/ieee-oui-database.txt");
            foreach (string line in lines)
            {
                // Sample line: "00:00:00   (hex)       XEROX CORPORATION"
                string[] parts = line.Split('\t');
                if (parts.Length >= 3)
                {
                    string oui = parts[0].Replace(":", "").Substring(0, 6).ToUpper();
                    string manufacturer = parts[2];
                    macManufacturerMap[oui] = manufacturer;
                }
            }
        }
        private string InferDeviceType(string manufacturer)
        {
            // Example: You might have a mapping of manufacturers to device types
            // This is a simplistic example and would need to be expanded
            if (manufacturer.Contains("Apple"))
            {
                return "iPhone"; // Example: Apple devices
            }
            else if (manufacturer.Contains("Microsoft"))
            {
                return "Surface"; // Example: Microsoft devices
            }
            else
            {
                return "Unknown"; // Unknown device type
            }
        }
        private string GetRuntimeName(Version version)
        {
            // Check the major and minor version numbers to determine the CLR version
            if (version.Major == 2 && version.Minor == 0)
            {
                return ".NET Framework 2.0";
            }
            else if (version.Major == 4 && version.Minor == 0)
            {
                return ".NET Framework 4.0";
            }
            else if (version.Major == 4 && version.Minor == 5)
            {
                return ".NET Framework 4.5";
            }
            else if (version.Major == 4 && version.Minor == 6)
            {
                return ".NET Framework 4.6";
            }
            else
            {
                return $"Unknown CLR Version: {version}";
            }
        }

        // Calculate the annual energy consumption in kWh
        private double CalculateAnnualEnergyConsumption(double powerConsumption, double runtimePerDay)
        {
            // Convert power consumption from watts to kilowatts
            double powerConsumptionKw = powerConsumption / 1000;

            // Calculate daily energy consumption in kWh
            double dailyEnergyConsumption = powerConsumptionKw * runtimePerDay;

            // Calculate annual energy consumption assuming 365 days in a year
            double annualEnergyConsumption = dailyEnergyConsumption * 365;

            return annualEnergyConsumption;
        }

        // Calculate the annual carbon footprint in kgCO2
        private double CalculateAnnualCarbonFootprint(double annualEnergyConsumption, double carbonIntensity)
        {
            // Calculate annual carbon footprint
            double annualCarbonFootprint = annualEnergyConsumption * carbonIntensity;
            return annualCarbonFootprint;
        }
        private void CFBackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
