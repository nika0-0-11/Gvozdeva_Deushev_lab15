using System.Windows;
using System.Windows.Threading;

namespace FocusTimer_WPF
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private int totalSeconds;
        private int remaningSeconds;
        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            InitializeTimeBox();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remaningSeconds--;
            
            if (remaningSeconds <= 0)
            {
                timer.Stop();
                isRunning = false;
                TimerText.Text = "00:00";
                MessageBox.Show("Время вышло!", "Focus Timer",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            UpdateTimeDisplay();
        }

        private void UpdateTimeDisplay()
        {
            int minutes = remaningSeconds / 60;
            int seconds = remaningSeconds % 60;
            TimerText.Text = $"{minutes:00}:{seconds:00}";
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                if (TimeBox.SelectedItem == null)
                    return;

                totalSeconds = (int)TimeBox.SelectedItem * 60;
                remaningSeconds = totalSeconds;

                timer.Start();
                isRunning = true;
                StartButton.Content = "Пауза";
                UpdateTimeDisplay();

            }
            else
            {
                timer.Stop();
                isRunning = false;
                StartButton.Content = "Старт";
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            isRunning = false;
            StartButton.Content = "Старт";

            if (TimeBox.SelectedItem != null)
            {
                remaningSeconds = (int)TimeBox.SelectedItem * 60;
                UpdateTimeDisplay();
            }
        }

        private void InitializeTimeBox()
        {
            TimeBox.Items.Add(15);
            TimeBox.Items.Add(25);
            TimeBox.Items.Add(45);
            TimeBox.SelectedIndex = 1;

        }
    }
}