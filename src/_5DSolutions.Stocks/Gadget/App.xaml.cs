using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Collections.ObjectModel;

using _5DSolutions.Stocks.Business;
using _5DSolutions.Stocks.Gadget.UI;

namespace _5DSolutions.Stocks.Gadget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            NavigationWindow mainWindow = Application.Current.MainWindow as NavigationWindow;
            mainWindow.Navigate(new Main());

        }

        
    }
}
