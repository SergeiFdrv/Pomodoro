using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Pomodoro
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void Repaint(string resource)
        {
            Random random = new Random();
            byte r, g, b;
            do
            {
                r = (byte)random.Next(96);
                g = (byte)random.Next(96);
                b = (byte)random.Next(96);
            } while ((r + g + b) / 3 > 96);
            (Current as App).Resources[resource] = 
                new SolidColorBrush(Color.FromArgb((byte)(255 * 0.8), r, g, b));
        }
    }
}
