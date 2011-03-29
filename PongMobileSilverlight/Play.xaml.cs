using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace PongMobile
{
    public partial class Play : PhoneApplicationPage
    {
        public Play()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Default_Loaded);
        }
        private void Default_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.Count > 0)
            {
                try
                {
                    Uri temp = new Uri(NavigationContext.QueryString.Values.First(), UriKind.RelativeOrAbsolute);
                    GameTypeText.Text = temp.ToString();
                }
                catch (Exception ex)
                {
                    //handle the exception;
                }
            }
        }
    }
}