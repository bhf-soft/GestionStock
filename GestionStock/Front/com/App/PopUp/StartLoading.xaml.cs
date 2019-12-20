using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestionStock.Front.com.App.PopUp
{
    /// <summary>
    /// Logique d'interaction pour StartLoading.xaml
    /// </summary>
    public partial class StartLoading : Window
    {
        public StartLoading()
        {
            InitializeComponent();
            LoadingBar.Value = 0;
            Task.Run(() =>
            {

                for (int i = 0; i <= 100; i++)
                {
                    Thread.Sleep(10);
                    this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                    {
                        LoadingBar.Value = i;
                    });
                }
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    MainWindow login = new MainWindow();
                    login.Show();
                    this.Close();
                });
            });
        }
    }
}
