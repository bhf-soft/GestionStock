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
    /// Logique d'interaction pour ProductForm.xaml
    /// </summary>
    public partial class ProductForm : Window
    {
        FormControlHelper formControlHelper = new FormControlHelper();
        public ProductForm()
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
                    formControlHelper.CheckNoString(Price))
                {
                    ProductController ctx = new ProductController();
                    Product bac = new Product();


                    bac.Id = Convert.ToInt32(Id.Text);

                    bac.Designation = Designation.Text;

                    string Prices = Price.Text;
                    Nullable<float> f;
                    if (Prices == null || Prices.Equals("")) f = null;
                    else f = float.Parse(Price.Text);
                    bac.Price = f;

                    bac.category_id = Convert.ToInt32(category_id.SelectedValue);
                    bac.Provider_id = Convert.ToInt32(Provider_id.SelectedValue);

                    bac.created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));


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
    }
    public static class CollectionData
    {
        public static Dictionary<string, string> GetCategories()
        {
            return ProductController.getCategory();
        }
        public static Dictionary<string, string> GetProviders()
        {
            return ProviderController.getProvider();
        }
    }
}
