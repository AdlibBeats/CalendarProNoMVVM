using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ProCalendar
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //lol.Loaded += Lol_Loaded;
        }

        //private void Lol_Loaded(object sender, RoutedEventArgs e)
        //{
        //    for (int i = 0; i < 99; i++)
        //    {
        //        lol.Add(new ProCalendar.UI.Controls.ProCalendarToggleButton
        //        {
        //            Foreground = new SolidColorBrush(Colors.AliceBlue),
        //            Background = new SolidColorBrush(Colors.Bisque),
        //            HorizontalAlignment = HorizontalAlignment.Center,
        //            VerticalAlignment = VerticalAlignment.Center,
        //            DateTime = DateTime.Now.AddDays(i)
        //        });
        //    }
        //}
    }
}
