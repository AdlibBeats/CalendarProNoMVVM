using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ProCalendar.UI.Controls.ControlsExtensions
{
    public static class ContentControlsExtension
    {
        public static ContentControl GetDefaultStyle(this ContentControl value, string content = null) => new ContentControl
        {
            Content = content,
            FontSize = 14,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Bottom,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
    }
}
