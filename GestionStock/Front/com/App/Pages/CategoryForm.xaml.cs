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
    /// Logique d'interaction pour CategoryForm.xaml
    /// </summary>
    public partial class CategoryForm : Window
    {
        FormControlHelper formControlHelper = new FormControlHelper();
        public CategoryForm()
        {
            InitializeComponent();
            Random r = new Random();
            Id.Text = r.Next().ToString();
        }
        private void Enregistrer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (formControlHelper.CheckNotNull(Id) && formControlHelper.IsNumber(Id.Text) && formControlHelper.IsNumeric(Id.Text))
                {
                    CategoryController ctx = new CategoryController();
                    Category bac = new Category();


                    bac.Id = Convert.ToInt32(Id.Text);

                    bac.Designation = Designation.Text;

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
}
