using GestionStock.Back.com.App.Controllers;
using GestionStock.Back.com.App.HelpersModels;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logique d'interaction pour ProviderCommandControl.xaml
    /// </summary>
    public partial class ProviderCommandControl : UserControl
    {
        private int numberOfRecPerPage;
        public List<string> Items = new List<string>();
        static Paging PagedTable = new Paging();
        public Boolean selected = false;
        public DataRowView SelectedRow = null;

        static ProviderCommandController ProviderCommandLists;
        public List<string> analyseSession = new List<string>();
        /*static string BankCode = Globals.getAnalyse()[1];
        static string dateArrete = Globals.getAnalyse()[3];
        static string version = Globals.getAnalyse()[5];*/
        private static List<ProviderCommand> ProviderCommand;
        public static string groupe = "";
        private static List<ProviderCommand> myList = new List<ProviderCommand>();

        public ProviderCommandControl()
        {
            InitializeComponent();
            PagedTable.type = typeof(ProviderCommand);
            initial();
        }
        public void initial()
        {
            ProviderCommandLists = new ProviderCommandController();
            ProviderCommand = ProviderCommandLists.PROVIDERCOMMAND.ToList<ProviderCommand>();
            myList = ProviderCommand;
            pagy();
        }
        private void Backwards_Click(object sender, RoutedEventArgs e)
        {
            G_ProviderCommand.ItemsSource = PagedTable.Previous(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void First_Click(object sender, RoutedEventArgs e)
        {
            G_ProviderCommand.ItemsSource = PagedTable.First(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void Last_Click(object sender, RoutedEventArgs e)
        {
            G_ProviderCommand.ItemsSource = PagedTable.Last(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)    //For each of these you call the direction you want and pass in the List and ComboBox output
        {                                                               //and use the above function to output the Record number to the Label
            G_ProviderCommand.ItemsSource = PagedTable.Next(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)  //I couldn't get this function to update in place (if the grid showed 20 and I selected 100 it would jump to 200)
        {                                                                                          //So instead I had it call the First function and that does an acceptable job.
            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            G_ProviderCommand.ItemsSource = PagedTable.First(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        public string PageNumberDisplay()
        {
            int PagedNumber = numberOfRecPerPage * (PagedTable.PageIndex + 1);
            if (PagedNumber > myList.Count)
            {
                PagedNumber = myList.Count;
            }
            return "Montrant " + PagedNumber + " de " + myList.Count; //This dramatically reduced the number of times I had to write this string statement
        }
        private void pagy()
        {

            PagedTable.PageIndex = 1; //Sets the Initial Index to a default value

            int[] RecordsToShow = { 10, 20, 30, 50, 100 }; //This Array can be any number groups
            NumberOfRecords.Items.Clear();

            foreach (int RecordGroup in RecordsToShow)
            {
                NumberOfRecords.Items.Add(RecordGroup); //Fill the ComboBox with the Array
            }

            NumberOfRecords.SelectedItem = 100; //Initialize the ComboBox

            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem); //Convert the Combox Output to type int

            // PagedTable.PageIndex = 0;

            PagedTable.type = typeof(ProviderCommand);

            DataTable firstTable = PagedTable.SetPaging(myList.Cast<Object>().ToList(), numberOfRecPerPage); //Fill a DataTable with the First set based on the numberOfRecPerPage

            G_ProviderCommand.ItemsSource = firstTable.DefaultView;
        }
        private void Nouveau_Click(object sender, RoutedEventArgs e)
        {
            ProviderCommandForm providerCommandForm = new ProviderCommandForm();
            providerCommandForm.Show();
            providerCommandForm.Closed += delegate
            {
                initial();
            };
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = G_ProviderCommand.SelectedItem as DataRowView;
                if (row != null)
                {
                    ProviderCommandForm providerCommandForm = new ProviderCommandForm();
                    providerCommandForm.Show();
                    providerCommandForm.Id.Text = row.Row.ItemArray[0].ToString();
                    providerCommandForm.Id.IsEnabled = false;


                    providerCommandForm.product_id.SelectedValue = row.Row.ItemArray[1].ToString();
                    providerCommandForm.Qte.Text = row.Row.ItemArray[2].ToString();
                    providerCommandForm.Provider_id.SelectedValue = row.Row.ItemArray[3].ToString();
                    providerCommandForm.Users_id.SelectedValue = row.Row.ItemArray[4].ToString();

                    if (row.Row.ItemArray[5] == null || row.Row.ItemArray[5].ToString() == "" || row.Row.ItemArray[5].ToString().StartsWith("F"))
                        providerCommandForm.IsDelivred.IsChecked = false;
                    else providerCommandForm.IsDelivred.IsChecked = true;

                    if (row.Row.ItemArray[6] == null || row.Row.ItemArray[6].ToString() == "" || row.Row.ItemArray[6].ToString().StartsWith("F"))
                        providerCommandForm.IsCancled.IsChecked = false;
                    else providerCommandForm.IsCancled.IsChecked = true;

                    providerCommandForm.Closed += delegate {
                        initial();
                    };
                }
                else
                {
                    MessageBox.Show(Messages.SelectLineAlerte.Value);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(Messages.SelectLineAlerte.Value);
            }
            catch (Exception ex)
            {
                //log.Info(Loggers.isUknowenError.Value);
                MessageBox.Show("Erreur Interne : \n \n " + ex.Message + " \n " + ex.StackTrace);
            }
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProviderCommandLists.delete(G_ProviderCommand);
            }catch(Exception ex)
            {
                MessageBox.Show("Erreur : \n \n " + ex.Message + " \n " + ex.StackTrace);
            }
            initial();
        }

        /*private void ProviderCommandNameSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GlobalHellper.search(G_ProviderCommand, ProviderCommandNameSearch, "ProviderCommand", "Product_id");
        }*/

        private void Stocker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProviderCommandLists.Stocker(G_ProviderCommand, PayedAmount.Text, PurchaseMethod.SelectedValue.ToString(), ChequeNumber.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : \n \n " + ex.Message + " \n " + ex.StackTrace);
            }
            initial();
            ChequeNumber.Text = "";
            PayedAmount.Text = "";
        }

        private void G_ProviderCommand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                AmountForPay.Content = ProviderCommandLists.SetAmount(G_ProviderCommand);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur :" + ex.Message + "  \n " + ex.StackTrace);
            }
        }

        public void WirteIntoDoc()
        {

        }
    }

    public static class CollectionDataBill
    {
        public static Dictionary<string, string> GetPurchaseMethod()
        {
            return ProviderCommandController.getPurchaseMethod();
        }
    }
}
