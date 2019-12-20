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
    /// Logique d'interaction pour ClientForm.xaml
    /// </summary>
    public partial class ClientForm : Window
    {
        FormControlHelper formControlHelper = new FormControlHelper();
        public ClientForm()
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
                    ClientController ctx = new ClientController();
                    Clients bac = new Clients();

                    
                    bac.Id = Convert.ToInt32(Id.Text);

                    bac.FullName = FullName.Text;
                    bac.Email = Email.Text;
                    bac.Patente = Patente.Text;
                    bac.Adress = Adress.Text;
                    bac.Tel = Tel.Text;
                    bac.City = City.Text;
                    bac.TypeC = TypeC.Text;
                    bac.Establishment = Establishment.Text;
                    bac.Created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));


                    ctx.InsertOrUpdate(bac);
                    //log.Info(Loggers.isInsertOrEdit.Value);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                //log.Info(Loggers.isUknowenError.Value);
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
