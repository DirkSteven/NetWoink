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

        private DateTime buildTime;

        private string cfWindowIP;
        private string cfWindowMAC;
        private string cfWindowVendor;

        public CFWindow(string IP, string MAC, string vendor)
        {
            InitializeComponent();
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
            buildTime = RetrieveBuildTime();
            TimeSpan runtimeSinceBuild = DateTime.Now - buildTime;
            double hoursSinceBuild = runtimeSinceBuild.TotalHours;

            Console.WriteLine($"Runtime since build: {runtimeSinceBuild}");
            Console.WriteLine($"Runtime since build: {hoursSinceBuild} hours");

            //runtimePerDay = 4; // Example: 4 hours

            runtimeLbl.Content = hoursSinceBuild;
            double carbonIntensity = 0.667;

            // Calculate carbon footprint
            Console.WriteLine($"{carbonIntensity}, {this.powerConsumption}");

            double dailyCarbonFootprint = CalculateDailyCarbonFootprint(this.powerConsumption, carbonIntensity);
            double annualCarbonFootprint = CalculateAnnualCarbonFootprint(dailyCarbonFootprint);
            double totalCarbonFootprint = CalculateTotalCarbonFootprint(powerConsumption, hoursSinceBuild, carbonIntensity);

            
            Console.WriteLine($"Annual Carbon Footprint: {annualCarbonFootprint} kgCO2");
            Console.WriteLine($"Daily Carbon Footprint: {dailyCarbonFootprint} kgCO2");
            Console.WriteLine($"Total Carbon Footprint: {totalCarbonFootprint} kgCO2");

            annualCFLbl.Content = annualCarbonFootprint;
            dailyCFLbl.Content = dailyCarbonFootprint;
            totalCFLbl.Content = totalCarbonFootprint;

        }

        private string LoadVendorInformation(string vendor)
        {
            //try
            //{
                
                Console.WriteLine($"Vendor for MAC address {cfWindowMAC}: {vendor}");
                string devtype = InferDeviceType(vendor);
                devtypeLbl.Content = devtype;
                return devtype;


            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //    Console.WriteLine ("LVI Exception");
            //    devtypeLbl.Content = "Phone";
            //    powerConsumption = PhoneRatedConsumption;
            //    rateLbl.Content = powerConsumption;
                
            //}
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


        //private async Task<string> GetVendor(string macAddress)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(macApiBaseUrl);

        //        HttpResponseMessage response = await client.GetAsync(macAddress);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return await response.Content.ReadAsStringAsync();
        //        }
        //        else if (response.StatusCode == HttpStatusCode.NotFound)
        //        {
        //            throw new Exception("MAC address not found.");
        //        }
        //        else
        //        {
        //            throw new Exception($"Unexpected response: {response.StatusCode}");
        //        }
        //    }
        //}


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
        private DateTime RetrieveBuildTime()
        {
            // Get the creation time of the application executable
            string appPath = Process.GetCurrentProcess().MainModule.FileName;
            DateTime creationTime = System.IO.File.GetCreationTime(appPath);
            return creationTime;
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

        public static double CalculateTotalCarbonFootprint(double powerConsumption, double runtimehrs, double carbonIntensity)
        {
            double totalEnergyConsumption = (powerConsumption * runtimehrs) / 1000; // Convert watts to kW
            return totalEnergyConsumption * carbonIntensity;
        }

        private void CFBackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
  