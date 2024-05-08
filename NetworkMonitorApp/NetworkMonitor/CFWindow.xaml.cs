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
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Numerics;
using System.Diagnostics;

namespace NetworkMonitor
{
    /// <summary>
    /// Interaction logic for CFWindow.xaml
    /// </summary>



    public partial class CFWindow : Window
    { 
        // Power consumption of the device in watts
        private double powerConsumption;

        private double PhoneRatedConsumption = 5;
        private double DesktopRatedConsumption = 200;

        private const string macApiBaseUrl = "https://api.macvendors.com/";

        //private DateTime buildTime;

        private string runtime;

        private double carbonIntensity = 0.667;

        private string cfWindowIP;
        private string cfWindowMAC;
        private string cfWindowVendor;

        public CFWindow(string IP, string MAC, string vendor)
        {
            InitializeComponent();

            // Start a timer to update the displayed time every second
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            cfWindowIP = IP;
            cfWindowMAC = MAC;
            cfWindowVendor = vendor;

            Console.WriteLine("CFWINDOWIP:" + cfWindowIP);
            Console.WriteLine("CFWINDOWMAC:" + cfWindowMAC);


            ipLbl.Content = cfWindowIP;
            macLbl.Content = cfWindowMAC;

            string devtype = LoadVendorInformation(cfWindowVendor);
            powercons(devtype);

            Console.WriteLine("LVI:" + this.powerConsumption);
            // Initialize values (you can retrieve these from user input or configuration)
            /*  powerConsumption = 50;*/ // Example: 50 watts
            //buildTime = RetrieveBuildTime();
            //TimeSpan runtimeSinceBuild = DateTime.Now - buildTime;
            //double hoursSinceBuild = runtimeSinceBuild.TotalHours;

            //Console.WriteLine($"Runtime since build: {runtimeSinceBuild}");
            //Console.WriteLine($"Runtime since build: {hoursSinceBuild} hours");

            //runtimePerDay = 4; // Example: 4 hours

            //runtimeLbl.Content = hoursSinceBuild;

            // Calculate carbon footprint
            Console.WriteLine($"{carbonIntensity}, {this.powerConsumption}");

            double dailyCarbonFootprint = CalculateDailyCarbonFootprint(this.powerConsumption, this.carbonIntensity);
            double annualCarbonFootprint = CalculateAnnualCarbonFootprint(dailyCarbonFootprint);

            
            Console.WriteLine($"Annual Carbon Footprint: {annualCarbonFootprint} kgCO2");
            Console.WriteLine($"Daily Carbon Footprint: {dailyCarbonFootprint} kgCO2");

            annualCFLbl.Content = annualCarbonFootprint;
            dailyCFLbl.Content = dailyCarbonFootprint;
           

        }

        private string LoadVendorInformation(string vendor)
        {
                
                Console.WriteLine($"Vendor for MAC address {cfWindowMAC}: {vendor}");
                string devtype = InferDeviceType(vendor);
                devtypeLbl.Content = devtype;
                return devtype;

        }

        private void powercons(string devtype)
        {
            if (devtype.Contains("Phone"))
            {
                Console.WriteLine("Phone adasdsasa");
                this.powerConsumption = this.PhoneRatedConsumption;
            }
            else if (devtype.Contains("Desktop"))
            {
                Console.WriteLine("Desktop");
                Console.WriteLine("powerconsbefore" + powerConsumption);
                Console.WriteLine("deskpowerconsbefore" + DesktopRatedConsumption);
                this.powerConsumption = this.DesktopRatedConsumption;
                Console.WriteLine("powerconsafetrr" + powerConsumption);
                Console.WriteLine("deskpowerconsbeafter" + DesktopRatedConsumption);
            }
            else
            {
                Console.WriteLine("Else asdsasagasgsa");
                this.powerConsumption = this.PhoneRatedConsumption;
            }

            rateLbl.Content = this.powerConsumption;
        }



        private string InferDeviceType(string manufacturer)
        {
            // Example: You might have a mapping of manufacturers to device types
            // This is a simplistic example and would need to be expanded
            if (manufacturer.Contains("Apple"))
            {
                return "iPhone"; // Example: Apple devices
            }
            else if (manufacturer.Contains("Microsoft") || manufacturer.Contains("Intel") || manufacturer.Contains("Liteon"))
            {
                return "Desktop"; // Example: Microsoft devices
            }
            else
            {
                return "Phone"; // Unknown device type
            }
        }
        private double GetRuntime()
        {
            // Get the elapsed time from the application stopwatch
            TimeSpan elapsed = App.ApplicationStopwatch.Elapsed;
            return elapsed.TotalHours; // Return the runtime in hours
        }
        private string FormatDouble(double value)
        {
            // Format the double value to display only two decimal places
            return value.ToString("#0.00000");
        }
        private string GetCarbonFootprintLevel(double totalCarbonFootprint)
        {
            // Define thresholds for low, medium, and high levels
            double lowThreshold = 50;
            double mediumThreshold = 100;

            // Compare the total carbon footprint against the thresholds
            if (totalCarbonFootprint < lowThreshold)
            {
                return "Low";
            }
            else if (totalCarbonFootprint < mediumThreshold)
            {
                return "Medium";
            }
            else
            {
                return "High";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the UI with the elapsed time
            TimeSpan elapsed = App.ApplicationStopwatch.Elapsed;

            this.runtime = elapsed.ToString(@"hh\:mm\:ss");

            // Update the total carbon footprint based on the runtime
            double runtime = GetRuntime(); // Get runtime from a method

            // Calculate total energy consumption based on runtime
            double totalEnergyConsumption = CalculateTotalEnergyConsumption(runtime, this.powerConsumption);

            // Calculate total carbon footprint
            double totalCarbonFootprint = totalEnergyConsumption * this.carbonIntensity;
            string formattedtotalCarbonFootprint = FormatDouble(totalCarbonFootprint);

            // Get the carbon footprint level
            string carbonFootprintLevel = GetCarbonFootprintLevel(totalCarbonFootprint);


            // Assuming there's a method to update UI with elapsed time
            UpdateElapsedTimeUI(elapsed);
            // Update UI with the total carbon footprint
            UpdateTotalCarbonFootprint(formattedtotalCarbonFootprint);
            UpdateCarbonFootprintLevel(carbonFootprintLevel);
        }

        private void UpdateElapsedTimeUI(TimeSpan elapsed)
        {
            // This method updates the UI with the elapsed time
            // For example, you can display it in a label or any other UI element
            runtimeLbl.Content = elapsed.ToString(@"hh\:mm\:ss");
        }

        public static double CalculateDailyCarbonFootprint(double energyConsumption, double carbonIntensity)
        {
            Console.WriteLine($"{energyConsumption} * {carbonIntensity}");
            return energyConsumption * carbonIntensity;
        }

        public static double CalculateAnnualCarbonFootprint(double dailyCarbonFootprint)
        {
            return dailyCarbonFootprint * 365;
        }


        private double CalculateTotalEnergyConsumption(double runtime, double powerConsumption)
        {
            // Implement your logic to calculate total energy consumption based on runtime
            // Assuming a device with a constant power consumption of 5 watts
            double totalEnergyConsumption = (powerConsumption * runtime) / 1000; // Convert watts to kW
            return totalEnergyConsumption;
        }

        private void UpdateTotalCarbonFootprint(string totalCarbonFootprint)
        {
            // This method updates the UI with the total carbon footprint
            Console.WriteLine($"Total Carbon Footprint: {totalCarbonFootprint} kgCO2");
            totalCFLbl.Content = totalCarbonFootprint;

        }

        private void UpdateCarbonFootprintLevel(string carbonFootprintLevel)
        {
            // This method updates the UI with the total carbon footprint
            Console.WriteLine($"Total Carbon Footprint Level: {carbonFootprintLevel}");
            totalCFlvlLbl.Content = carbonFootprintLevel;

        }

        private void CFBackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
  