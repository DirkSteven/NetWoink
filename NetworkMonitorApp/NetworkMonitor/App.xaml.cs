using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace NetworkMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Static Stopwatch instance to track application runtime
        private static Stopwatch applicationStopwatch = new Stopwatch();

        public static Stopwatch ApplicationStopwatch
        {
            get { return applicationStopwatch; }
        }

        public App()
        {
            // Start the application stopwatch if it's not already running
            if (!ApplicationStopwatch.IsRunning)
            {
                ApplicationStopwatch.Start();
            }

            // Start a timer to update the displayed time every second
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the UI with the elapsed time
            TimeSpan elapsed = ApplicationStopwatch.Elapsed;
            // Assuming there's a method to update UI with elapsed time
            UpdateElapsedTimeUI(elapsed);
        }

        private void UpdateElapsedTimeUI(TimeSpan elapsed)
        {
            // This method updates the UI with the elapsed time
            // For example, you can raise an event to notify UI elements to update
            // Or you can directly update UI controls if you have access to them
        }
    }

}