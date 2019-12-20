using GestionStock.Front.com.App.Pages;
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

namespace GestionStock.Front.com.App
{
    /// <summary>
    /// Logique d'interaction pour StartApp.xaml
    /// </summary>
    public partial class StartApp : Window
    {
        ProviderControl ProviderControl = new ProviderControl();
        ClientControl ClientControl = new ClientControl();
        public StartApp()
        {
            InitializeComponent();
        }
        public void resizing(double newWindowHeight, double newWindowWidth)
        {
            double Grid_Height = newWindowHeight - 240;
            double Grid_With = newWindowWidth -30;
            //*********

            ProviderControl.G_Provider.Height = (Grid_Height -70) /2;
            ProviderControl.G_Provider.Width = Grid_With;

            ClientControl.G_Client.Height = (Grid_Height - 70) / 2;
            ClientControl.G_Client.Width = Grid_With;
        }

            private void ProductBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ProductBtn.Width = 90;
            ProductBtn.Height = 90;
        }

        private void ProductBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ProductBtn.Width = 70;
            ProductBtn.Height = 70;
        }

        private void ProductBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(MainPanel.Children.Count == 0)
            {
                MainPanel.Children.Add(ClientControl);
                MainPanel.Children.Add(ProviderControl);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double newWindowHeight = e.NewSize.Height - 20;
            double newWindowWidth = e.NewSize.Width;
            resizing(newWindowHeight, newWindowWidth);
        }
    }
}
