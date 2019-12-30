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
    /// Logique d'interaction pour ClientCommandControl.xaml
    /// </summary>
    public partial class ClientCommandControl : UserControl
    {
        readonly StockDATAEntities crudctx = new StockDATAEntities();
        private int numberOfRecPerPage;
        public List<string> Items = new List<string>();
        static Paging PagedTable = new Paging();
        public Boolean selected = false;
        public DataRowView SelectedRow = null;

        static ClientCommandController ClientCommandLists;
        private static List<EnterStock> EnterStk;
        private static List<EnterStock> myList = new List<EnterStock>();

        private static List<ClientCommand> ClientCmd;
        private static List<ClientCommand> myListCmd = new List<ClientCommand>();

        public ClientCommandControl()
        {
            InitializeComponent();
            PagedTable.type = typeof(EnterStock);
            initial();
            ClientCommandinitial();
        }
        public void initial()
        {
            ClientCommandLists = new ClientCommandController();
            EnterStk = ClientCommandLists.ENTERSTK.ToList<EnterStock>();
            myList = EnterStk;
            pagy();
        }

        public void ClientCommandinitial()
        {
            ClientCommandLists = new ClientCommandController();
            ClientCmd = ClientCommandLists.CLIENTCMD.ToList<ClientCommand>();
            myListCmd = ClientCmd;
            G_ClientCommand.ItemsSource = myListCmd;
            client_id.SelectedValue = 1;
        }

        private void Backwards_Click(object sender, RoutedEventArgs e)
        {
            G_StockValable.ItemsSource = PagedTable.Previous(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void First_Click(object sender, RoutedEventArgs e)
        {
            G_StockValable.ItemsSource = PagedTable.First(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void Last_Click(object sender, RoutedEventArgs e)
        {
            G_StockValable.ItemsSource = PagedTable.Last(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)    //For each of these you call the direction you want and pass in the List and ComboBox output
        {                                                               //and use the above function to output the Record number to the Label
            G_StockValable.ItemsSource = PagedTable.Next(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)  //I couldn't get this function to update in place (if the grid showed 20 and I selected 100 it would jump to 200)
        {                                                                                          //So instead I had it call the First function and that does an acceptable job.
            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            G_StockValable.ItemsSource = PagedTable.First(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
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

            PagedTable.type = typeof(EnterStock);

            DataTable firstTable = PagedTable.SetPaging(myList.Cast<Object>().ToList(), numberOfRecPerPage); //Fill a DataTable with the First set based on the numberOfRecPerPage

            G_StockValable.ItemsSource = firstTable.DefaultView;
        }

        private void G_StockValable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddToBasket_Click(object sender, RoutedEventArgs e)
        {
            ClientCommand clientCommand = new ClientCommand();
            EnterStock enterStock = new EnterStock();
            Random r = new Random();
            try
            {
                DataRowView row = G_StockValable.SelectedItem as DataRowView;
                if (row != null)
                {
                    foreach (DataRowView selectedrows in G_StockValable.SelectedItems)
                    {
                        string STKENTER_PK = selectedrows[0].ToString();
                        string STKENTER_ProductID = selectedrows[1].ToString();
                        string STKENTER_QTE = selectedrows[2].ToString();
                        string STKENTER_PROVIDERID = selectedrows[3].ToString();
                        string STKENTER_USERID = selectedrows[4].ToString();
                        string STKENTER_ENTERDATE = selectedrows[5].ToString();

                        /////
                        ///
                        clientCommand.Id = r.Next();
                        clientCommand.Product_id = Convert.ToInt32(STKENTER_ProductID);
                        clientCommand.Qte = Convert.ToInt32(ProductNbrPcs.Text);
                        clientCommand.Client_id = Convert.ToInt32(client_id.SelectedValue.ToString());
                        clientCommand.Users_id = Convert.ToInt32(STKENTER_USERID);
                        clientCommand.IsDelivred = true;
                        clientCommand.IsCancled = false;
                        clientCommand.CmdDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        clientCommand.ConfirmationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                        var UpdatedorInserted = crudctx.EnterStock.Where(c => c.Id.ToString().Equals(STKENTER_PK)).FirstOrDefault();

                        if (UpdatedorInserted.Qte >= clientCommand.Qte)
                        {
                            UpdatedorInserted.Qte = UpdatedorInserted.Qte - clientCommand.Qte;
                            crudctx.ClientCommand.Add(clientCommand);
                            crudctx.SaveChanges();
                            MessageBox.Show("Commande Client Ajoutée ");
                        }
                        else
                        {
                            MessageBox.Show("Stock Insifusant pour cette opération");
                        }


                        Console.WriteLine(" clientCommand.Id  : " + clientCommand.Id + " clientCommand.Product_id :" + clientCommand.Product_id
                            + " clientCommand.Qte :" + clientCommand.Qte + " clientCommand.Client_id : " + clientCommand.Client_id +
                            " clientCommand.Users_id :" + clientCommand.Users_id + " clientCommand.IsDelivred : " + clientCommand.IsDelivred +
                            " clientCommand.CmdDate : " + clientCommand.CmdDate + " clientCommand.ConfirmationDate : " + clientCommand.ConfirmationDate);

                        G_ClientCommand.ItemsSource = crudctx.ClientCommand.ToList();
                    }
                }
                else
                {
                    MessageBox.Show(Messages.SelectLineAlerte.Value);
                }
                ClientCommandinitial();
                initial();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erreur : \n "+ex.StackTrace + " \n \n "+ex.Message);
            }
        }

        private void Stocker_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void G_ClientCommand_LoadingRow(object sender, DataGridRowEventArgs e)
        {
           /* var row = e.Row;
           row.Background = new SolidColorBrush(Colors.Red);*/
        }

        private void GenerateFacture_Click(object sender, RoutedEventArgs e)
        {
           /* try
            {
                DataRowView row = G_StockValable.SelectedItem as DataRowView;
                if (row != null)
                {
                    for(int i = 0; i < G_ClientCommand.Items.Count; i++)
                    {
                        string STKENTER_PK = G_ClientCommand.Items[0][0].ToString();
                    }
                    foreach (DataRowView selectedrows in G_StockValable.Items)
                    {
                        string STKENTER_PK = selectedrows[0].ToString();
                        string STKENTER_ProductID = selectedrows[1].ToString();
                        string STKENTER_QTE = selectedrows[2].ToString();
                        string STKENTER_PROVIDERID = selectedrows[3].ToString();
                        string STKENTER_USERID = selectedrows[4].ToString();
                        string STKENTER_ENTERDATE = selectedrows[5].ToString();

                        /////
                        ///
                        clientCommand.Id = r.Next();
                        clientCommand.Product_id = Convert.ToInt32(STKENTER_ProductID);
                        clientCommand.Qte = Convert.ToInt32(ProductNbrPcs.Text);
                        clientCommand.Client_id = Convert.ToInt32(client_id.SelectedValue.ToString());
                        clientCommand.Users_id = Convert.ToInt32(STKENTER_USERID);
                        clientCommand.IsDelivred = true;
                        clientCommand.IsCancled = false;
                        clientCommand.CmdDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        clientCommand.ConfirmationDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                        var UpdatedorInserted = crudctx.EnterStock.Where(c => c.Id.ToString().Equals(STKENTER_PK)).FirstOrDefault();

                        if (UpdatedorInserted.Qte >= clientCommand.Qte)
                        {
                            UpdatedorInserted.Qte = UpdatedorInserted.Qte - clientCommand.Qte;
                            crudctx.ClientCommand.Add(clientCommand);
                            crudctx.SaveChanges();
                            MessageBox.Show("Commande Client Ajoutée ");
                        }
                        else
                        {
                            MessageBox.Show("Stock Insifusant pour cette opération");
                        }


                        Console.WriteLine(" clientCommand.Id  : " + clientCommand.Id + " clientCommand.Product_id :" + clientCommand.Product_id
                            + " clientCommand.Qte :" + clientCommand.Qte + " clientCommand.Client_id : " + clientCommand.Client_id +
                            " clientCommand.Users_id :" + clientCommand.Users_id + " clientCommand.IsDelivred : " + clientCommand.IsDelivred +
                            " clientCommand.CmdDate : " + clientCommand.CmdDate + " clientCommand.ConfirmationDate : " + clientCommand.ConfirmationDate);

                        G_ClientCommand.ItemsSource = crudctx.ClientCommand.ToList();
                    }
                }
                else
                {
                    MessageBox.Show(Messages.SelectLineAlerte.Value);
                }
                ClientCommandinitial();
                initial();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erreur : \n " + ex.StackTrace + " \n \n " + ex.Message);
            } */
        }
    }
    public static class CollectionDataClient
    {
        public static Dictionary<string, string> GetClient()
        {
            return ClientCommandController.GetClient();
        }
    }
}
