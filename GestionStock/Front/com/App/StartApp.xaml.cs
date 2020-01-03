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
        ProductControl ProductControl = new ProductControl();
        CategoryControl CategoryControl = new CategoryControl();
        ProviderCommandControl ProviderCommandControl = new ProviderCommandControl();
        ClientCommandControl ClientCommandControl = new ClientCommandControl();
        CmdCancelControl CmdCancelControl = new CmdCancelControl();
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

            ProductControl.G_Product.Height = (Grid_Height - 70) / 2;
            ProductControl.G_Product.Width = Grid_With;

            CategoryControl.G_Category.Height = (Grid_Height - 70) / 2;
            CategoryControl.G_Category.Width = Grid_With;

            ProviderCommandControl.G_ProviderCommand.Height = (Grid_Height - 70) / 2;
            ProviderCommandControl.G_ProviderCommand.Width = Grid_With;

            ClientCommandControl.G_StockValable.Height = (Grid_Height - 100) / 2;
            ClientCommandControl.G_StockValable.Width = Grid_With;

            ClientCommandControl.G_CL_CMD.Height = (Grid_Height - 90) / 2;
            ClientCommandControl.G_CL_CMD.Width = Grid_With;

            CmdCancelControl.G_CL_CMD.Height = (Grid_Height - 70) / 2;
            CmdCancelControl.G_CL_CMD.Width = Grid_With;
        }
        private void ProductBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPanel.Children.Clear();
            if (MainPanel.Children.Count == 0)
            {
                MainPanel.Children.Add(ProductControl);
                MainPanel.Children.Add(CategoryControl);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double newWindowHeight = e.NewSize.Height - 20;
            double newWindowWidth = e.NewSize.Width;
            resizing(newWindowHeight, newWindowWidth);
        }

        private void ClientsBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPanel.Children.Clear();
            if (MainPanel.Children.Count == 0)
            {
                MainPanel.Children.Add(ClientControl);
                MainPanel.Children.Add(ProviderControl);
            }
        }
        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void ClientsBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ClientsBtn.Width = 90;
            ClientsBtn.Height = 90;
        }

        private void ClientsBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ClientsBtn.Width = 70;
            ClientsBtn.Height = 70;
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

        private void CommandBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPanel.Children.Clear();
            if (MainPanel.Children.Count == 0)
            {
                MainPanel.Children.Add(ProviderCommandControl);
                //MainPanel.Children.Add(ProviderControl);
            }
        }

        private void CommandBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            CommandBtn.Width = 90;
            CommandBtn.Height = 90;
        }

        private void CommandBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            CommandBtn.Width = 70;
            CommandBtn.Height = 70;
        }

        private void ClientCommandBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPanel.Children.Clear();
            if (MainPanel.Children.Count == 0)
            {
                MainPanel.Children.Add(ClientCommandControl);
                //MainPanel.Children.Add(CategoryControl);
            }
        }

        private void ClientCommandBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ClientCommandBtn.Width = 90;
            ClientCommandBtn.Height = 90;
        }

        private void ClientCommandBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ClientCommandBtn.Width = 70;
            ClientCommandBtn.Height = 70;
        }

        private void ClientCommandCancelBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPanel.Children.Clear();
            if (MainPanel.Children.Count == 0)
            {
                MainPanel.Children.Add(CmdCancelControl);
                //MainPanel.Children.Add(CategoryControl);
            }
        }

        private void ClientCommandCancelBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ClientCommandCancelBtn.Width = 90;
            ClientCommandCancelBtn.Height = 90;
        }

        private void ClientCommandCancelBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ClientCommandCancelBtn.Width = 70;
            ClientCommandCancelBtn.Height = 70;
        }
    }
}
