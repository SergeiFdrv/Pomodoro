using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Pomodoro.Resources;

namespace Pomodoro
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetTimer();
            Random random = new Random();
            App.Repaint("BlackGlass");
            DispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                TimerText.Text = TimeSpan.ToString("c");
                if (TimeSpan == TimeSpan.Zero) StopTimer().ConfigureAwait(false);
                TimeSpan = TimeSpan.Add(TimeSpan.FromSeconds(-1));
            }, App.Current.Dispatcher)
            {
                IsEnabled = false
            };
        }

        private readonly DispatcherTimer DispatcherTimer;
        private TimeSpan TimeSpan;

        private int PomodoroNumber { get; set; }
        private async Task StopTimer()
        {
            DispatcherTimer.Stop();
            TimerText.Foreground = Brushes.White;
            ResetTimer();
            SystemSounds.Asterisk.Play();
            await Task.Delay(500);
            SystemSounds.Beep.Play();
            await Task.Delay(500);
            SystemSounds.Exclamation.Play();
            await Task.Delay(500);
            SystemSounds.Hand.Play();
            await Task.Delay(500);
            SystemSounds.Question.Play();
            if (PomodoroNumber == 1)
            {
                await Task.Delay(60000 * 15);
                if (!DispatcherTimer.IsEnabled)
                {
                    WindowState = WindowState.Normal;
                    Activate();
                }
            }
            else
            {
                await Task.Delay(60000 * 3);
                if (!DispatcherTimer.IsEnabled)
                {
                    WindowState = WindowState.Normal;
                    Activate();
                }
            }
        }
        
        private void SetTimer()
        {
            PomodoroNumber = (PomodoroNumber < 4) ? PomodoroNumber + 1 : 1;
            CheckText.Text = Lang.PomodoroXOfY.Replace("{x}", PomodoroNumber.ToString());
            TimeSpan = TimeSpan.FromMinutes(25);
            TimerText.Text = TimeSpan.ToString("c");
            StartButton.Content = "Start";
        }

        private void ResetTimer()
        {
            if (PomodoroNumber < 4)
            {
                HintText.Text = Lang.ShortBreak.Replace("{time}", DateTime.Now.AddMinutes(4).ToString("t"));
            }
            else
            {
                HintText.Text = Lang.LongBreak
                    .Replace("{time0}", DateTime.Now.AddMinutes(15).ToString("t"))
                    .Replace("{time1}", DateTime.Now.AddMinutes(30).ToString("t"));
            }
            SetTimer();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (DispatcherTimer.IsEnabled = !DispatcherTimer.IsEnabled)
            {
                TimerText.Foreground = Brushes.Yellow;
                StartButton.Content = "Stop";
                HintText.Text = Lang.DefaultHintText;
            }
            else
            {
                TimerText.Foreground = Brushes.White;
                StartButton.Content = "Start";
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hl = (Hyperlink)sender;
            string navigateUri = hl.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
        }

        private bool IsCompactPrv;
        private bool IsCompact
        {
            get => IsCompactPrv;
            set
            {
                if (IsCompactPrv = value)
                {
                    Top += Height / 2;
                    Left += Width / 2;
                    TaskStack.Visibility = 
                    AboutSection.Visibility = 
                    HintText.Visibility = Visibility.Collapsed;
                    MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
                    SizeToContent = SizeToContent.WidthAndHeight;
                    Top -= Height / 2;
                    Left -= Width / 2;
                }
                else
                {
                    Top += Height / 2;
                    Left += Width / 2;
                    TaskStack.Visibility =
                    AboutSection.Visibility =
                    HintText.Visibility = Visibility.Visible;
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    SizeToContent = SizeToContent.Height;
                    Width = 800;
                    Top -= Height / 2;
                    Left -= Width / 2;
                }
            }
        }

        private void CompactButton_Click(object sender, RoutedEventArgs e)
        {
            IsCompact = !IsCompact;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Minimized) ? WindowState.Normal : WindowState.Minimized;
        }
    }
}
