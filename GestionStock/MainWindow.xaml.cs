using GestionStock.Front.com.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GestionStock
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly StockDATAEntities crudctx = new StockDATAEntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        public bool CheckEmailFormat(TextBox Email)
        {
            Regex RX = new Regex(@"(?<email>\w+@\w+\.[a-z]{0,3})");
            Match match = RX.Match(Email.Text);
            if (match.Success)
            {
                Email.BorderBrush = Brushes.Green;
                Email.ToolTip = "Format Email valide !";
                return true;
            }
            else
            {
                Email.BorderBrush = Brushes.Red;
                Email.ToolTip = "Format Email invalide !";
                return false;
            }
        }

        public bool Connect_User(TextBox SG_Login, PasswordBox SG_Pass)
        {
            var map = new Dictionary<string, string>();
            string MotDePasse = SG_Pass.Password.ToString();
            var logedOne =
                crudctx.Users.Where(c => c.Email.Equals(SG_Login.Text) && c.Pass.Equals(MotDePasse)).FirstOrDefault();
            if (logedOne != null)
            {
                StartApp app = new StartApp();
                app.Show();
                return true;
            }
            else
            {
                MessageBox.Show("Le login ou Mot de passe Invalide !", "Connexion Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            if (CheckEmailFormat(Login))
            {
                if (Connect_User(Login, Password))
                {
                    this.Close();
                }
                else
                {
                    Password.Password = "";
                }
            }            
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
