using GestionStock.Back.com.App.Controllers;
using GestionStock.Back.com.App.HelpersModels;
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

namespace GestionStock.Front.com.App.Pages
{
    /// <summary>
    /// Logique d'interaction pour ProviderCommandForm.xaml
    /// </summary>
    public partial class ProviderCommandForm : Window
    {
        FormControlHelper formControlHelper = new FormControlHelper();
        public ProviderCommandForm()
        {
            InitializeComponent();
            Random r = new Random();
            Id.Text = r.Next().ToString();
        }

        private void Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (formControlHelper.CheckNotNull(Id) && formControlHelper.IsNumber(Id.Text) && formControlHelper.IsNumeric(Id.Text) &&
                    formControlHelper.IsNumber(Qte.Text) && formControlHelper.IsNumeric(Qte.Text))
                {
                    ProviderCommandController ctx = new ProviderCommandController();
                    ProviderCommand bac = new ProviderCommand();


                    bac.Id = Convert.ToInt32(Id.Text);

                    bac.Product_id = Convert.ToInt32(product_id.SelectedValue);
                    bac.Qte = Convert.ToInt32(Qte.Text);

                    bac.Provider_id = Convert.ToInt32(Provider_id.SelectedValue);
                    bac.Users_id = Convert.ToInt32(Users_id.SelectedValue);
                    bac.IsDelivred = IsDelivred.IsChecked.Value;
                    bac.IsCancled = IsCancled.IsChecked.Value;

                    bac.CmdDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                    if (IsDelivred.IsChecked == true)
                    {
                        bac.ConfirmationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    }

                    if (IsCancled.IsChecked == true)
                    {
                        bac.CancelDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    }

                    ctx.InsertOrUpdate(bac);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : \n \n " + ex.Message + " \n \n " + ex.StackTrace);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

        private void IsDelivred_Checked(object sender, RoutedEventArgs e)
        {
            CheckCommand();
        }

        private void IsCancled_Checked(object sender, RoutedEventArgs e)
        {
            CheckCommand();
        }
        public void CheckCommand()
        {
            if(IsDelivred.IsChecked == true)
                IsCancled.IsChecked = false;
            else if(IsCancled.IsChecked == true)
                IsDelivred.IsChecked = false;
            else if (IsDelivred.IsChecked == false)
                IsCancled.IsChecked = false;
            else if(IsCancled.IsChecked == false)
                IsDelivred.IsChecked = true;
        }
    }
    public static class CollectionDataProviderCommand
    {
        public static Dictionary<string, string> GetProducts()
        {
            return ProductController.getProduct();
        }
        public static Dictionary<string, string> GetProviders()
        {
            return ProviderController.getProvider();
        }
        public static Dictionary<string, string> GetUsers()
        {
            return UsersController.GetUsers();
        }
    }
}
