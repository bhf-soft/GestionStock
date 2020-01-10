using GestionStock.Back.com.App.Controllers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestionStock.Front.com.App.Pages
{
    /// <summary>
    /// Logique d'interaction pour StatisticsControl.xaml
    /// </summary>
    public partial class StatisticsControl : UserControl
    {
        public StatisticsControl()
        {
            InitializeComponent();
        }

        private void NumberOfdays_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    public static class CollectionDays
    {
        public static Dictionary<string, string> GetDays()
        {
            return StatisticsController.getDays();
        }
    }
}
