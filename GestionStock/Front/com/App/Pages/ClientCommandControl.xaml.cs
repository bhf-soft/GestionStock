using GestionStock.Back.com.App.Controllers;
using GestionStock.Back.com.App.HelpersModels;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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
            ClientCommandinitial(0);
            client_id.SelectedValue = 1;
            clientCmd_id.SelectedValue = 1;
            PurchaseMethod.SelectedValue = "Cash";
        }
        public void initial()
        {
            ClientCommandLists = new ClientCommandController();
            EnterStk = ClientCommandLists.ENTERSTK.ToList<EnterStock>();
            myList = EnterStk;
            pagy();
        }

        public void ClientCommandinitial(int ClientId)
        {
            ClientCommandLists = new ClientCommandController();
            if(ClientId == 0)
                ClientCmd = ClientCommandLists.CLIENTCMD.ToList<ClientCommand>();
            else 
            ClientCmd = ClientCommandLists.CLIENTCMD.Where(a => a.Client_id == ClientId).ToList<ClientCommand>();
            myListCmd = ClientCmd;
            G_CL_CMD.ItemsSource = myListCmd;
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

                        if(!ProductNewPrice.Text.Equals("0"))
                        {
                            string NewPrices = ProductNewPrice.Text;
                            Nullable<float> f;
                            if (NewPrices == null || NewPrices.Equals("")) f = null;
                            else f = float.Parse(ProductNewPrice.Text);
                            clientCommand.NewPrice = f;
                        }else
                        {
                            clientCommand.NewPrice = 0;
                        }
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

                        G_CL_CMD.ItemsSource = crudctx.ClientCommand.ToList();
                    }
                }
                else
                {
                    MessageBox.Show(Messages.SelectLineAlerte.Value);
                }
                ClientCommandinitial(0);
                initial();
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erreur : \n "+ex.StackTrace + " \n \n "+ex.Message);
            }
        }

        private void GenerateFacture_Click(object sender, RoutedEventArgs e)
        {
           try
           {

                DataTable dt = ClientCommandController.GetClientCmd();

                string MethodPay = PurchaseMethod.SelectedValue.ToString();
                string ChequeNum = ChequeNumber.Text;
                string ClientForPay = clientCmd_id.SelectedValue.ToString();
                double TotalPayed=0;

                string extension = "pdf";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(@"img/Facture.jpeg");
                jpg.ScaleToFit(2700, 770);
                jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                jpg.SetAbsolutePosition(20, 35);
                MessageBoxResult M = MessageBox.Show("Etes-vous sûr de vouloir Sauvgarder la facture ?", "Avertissement", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (M == MessageBoxResult.Yes)
                {
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        DefaultExt = extension,
                        Filter = String.Format("{1} PDF file | *.pdf", extension, "Pdf"),
                        FilterIndex = 1
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, 0f, 0f, 140f, 100f);

                        try
                        {
                            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(dialog.FileName, FileMode.Create));
                            doc.Open();
                            doc.Add(jpg);
                            PdfPTable table = new PdfPTable(10);
                            addCell(table, "Produit", 0,"");
                            addCell(table, "Qte", 0, "");
                            addCell(table, "Client", 0, "");

                            addCell(table, "Confirmée", 0, "");
                            addCell(table, "Annulée", 0, "");
                            addCell(table, "Date Commande", 0, "");

                            addCell(table, "Date Confirmation", 0, "");
                            addCell(table, "Date Annulation", 0, "");
                            addCell(table, "Méthode de paiment", 0, "");

                            addCell(table, "N° Chèque", 0, "");

                            foreach (DataRow row in dt.Rows)
                            {
                                string Id = row["Id"].ToString();
                                string Product = row["CMD_Product"].ToString();
                                string Qte = row["CMD_Qte"].ToString();
                                string Client = row["CMD_Client"].ToString();
                                string ProductPrice = row["CMD_ProductPrice"].ToString();
                                string Isdelivred = row["CMDIsDelivred"].ToString();
                                string Iscancled = row["CMDIsCancled"].ToString();
                                string CmdDate = row["CMD_CmdDate"].ToString();
                                string ConfirmDate = row["CMD_ConfirmationDate"].ToString();
                                string CancelDate = row["CMD_CancelDate"].ToString();

                                
                                if (Client.Equals(ClientCommandController.GetClientByID(Convert.ToInt32(ClientForPay))))
                                {
                                    string state = "";
                                    if (Iscancled.Equals("True"))
                                    {
                                        state = "True";
                                    }
                                    addCell(table, Product, 0,state);
                                    addCell(table, Qte, 0, state);
                                    addCell(table, Client, 0, state);

                                    addCell(table, Isdelivred, 0, state);
                                    addCell(table, Iscancled, 0, state);
                                    addCell(table, CmdDate, 0, state);

                                    addCell(table, ConfirmDate, 0, state);
                                    addCell(table, CancelDate, 0, state);
                                    addCell(table, MethodPay, 0, state);

                                    addCell(table, ChequeNum, 0, state);

                                    Console.WriteLine(" Id :" + Id + " Product : " + Product + " ProductPrice : "+ ProductPrice + " Qte : " + Qte + " Client :" + Client + " Isdelivred : " + Isdelivred + " Iscancled :" + Iscancled + " CmdDate : " + CmdDate + " ConfirmDate :" + ConfirmDate + " CancelDate :" + CancelDate + " MethodPay :" + MethodPay + " ChequeNum :" + ChequeNum + " ClientForPay :" + ClientForPay);
                                    if(!String.IsNullOrEmpty(ProductPrice) && !String.IsNullOrEmpty(Qte) && Iscancled.Equals("False"))
                                    TotalPayed = TotalPayed + ((float.Parse(ProductPrice)) * (Convert.ToInt32(Qte)));
                                }
                            }
                            Font DateFont = new Font(Font.FontFamily.TIMES_ROMAN, 09, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                            doc.Add(new Phrase("      Salé Le  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), DateFont));
                            doc.Add(table);

                            doc.Add(new Phrase(" \n        TVA  :   0.00 DH TTC"));
                            doc.Add(new Phrase(" \n \n         Total Facture à payer :     "+ Math.Round(TotalPayed, 2) + "      DH TTC"));

                            doc.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message");

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Message");
                }

                
                    
                ClientCommandinitial(Convert.ToInt32(ClientForPay));
                initial();
                MessageBox.Show("Facture Bien Enregistrée");
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erreur : \n " + ex.StackTrace + " \n \n " + ex.Message);
            } 
        }

        private static void addCell(PdfPTable table, string text, int rowspan,string IsCancled)
        {
            BaseColor myColor;
            
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            PdfPCell cell = new PdfPCell(new Phrase(text, times));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            if (IsCancled.Equals("True"))
            {
                myColor = WebColors.GetRGBColor("#e35252");
                cell.BackgroundColor = myColor;
            }            
            table.AddCell(cell);
        }

        private void PurchaseMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(PurchaseMethod.SelectedValue.ToString().Equals("Chèque"))
            {
                ChequeNumber.Visibility = Visibility.Visible;
            }else
            {
                ChequeNumber.Visibility = Visibility.Hidden;
            }
        }

        private void ClientCmd_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ClientForPay = clientCmd_id.SelectedValue.ToString();
            Console.WriteLine("Client :" + ClientForPay);
            ClientCommandinitial(Convert.ToInt32(ClientForPay));
        }

        private void GenerateBonCmd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DataTable dt = ClientCommandController.GetClientCmd();

                string MethodPay = PurchaseMethod.SelectedValue.ToString();
                string ChequeNum = ChequeNumber.Text;
                string ClientForPay = clientCmd_id.SelectedValue.ToString();
                double TotalPayed = 0;
                string CmdClient = "";

                string extension = "pdf";

                iTextSharp.text.Image Footer = iTextSharp.text.Image.GetInstance(@"img/footer.png");
                Footer.ScaleToFit(550, 36);
                Footer.Alignment = iTextSharp.text.Image.UNDERLYING;
                Footer.SetAbsolutePosition(0,0);


                iTextSharp.text.Image CmdLogo = iTextSharp.text.Image.GetInstance(@"img/LogoTest.png");
                CmdLogo.ScaleToFit(30,30);
                CmdLogo.Alignment = iTextSharp.text.Image.UNDERLYING;
                CmdLogo.SetAbsolutePosition(0,0);
                MessageBoxResult M = MessageBox.Show("Etes-vous sûr de vouloir Sauvgarder la facture ?", "Avertissement", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (M == MessageBoxResult.Yes)
                {
                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        DefaultExt = extension,
                        Filter = String.Format("{1} PDF file | *.pdf", extension, "Pdf"),
                        FilterIndex = 1
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A5, 10f, 10f, 10f, 10f);

                        try

                        {
                            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(dialog.FileName, FileMode.Create));
                            doc.Open();
                            doc.Add(Footer);

                            PdfPTable Headertable = new PdfPTable(3);
                            Headertable.TotalWidth = 400f;
                            Headertable.LockedWidth = true;
                            Headertable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            float[] widths = new float[] { 100f, 200f, 200f };
                            Headertable.SetWidths(widths);

                            PdfPCell LogoCell = new PdfPCell(CmdLogo);
                            PdfPCell cell1 = new PdfPCell(new Phrase("BM ZAY SARL"));
                            cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            cell1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;


                            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                            iTextSharp.text.Font times2 = new iTextSharp.text.Font(bfTimes, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                            Font DateFont = new Font(Font.FontFamily.TIMES_ROMAN, 09, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                            PdfPCell cell2 = new PdfPCell(new Phrase("Comptoir de vente Profilés et accessoires pour menuiserie aluminium", times));
                            cell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            cell2.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;


                            PdfPCell cell4 = new PdfPCell(new Phrase("Client  :  "+ ClientCommandController.GetClientByID(Convert.ToInt32(ClientForPay)) , times2));
                            cell4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            cell4.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;

                            PdfPCell cell5 = new PdfPCell(new Phrase("Salé Le  " + DateTime.Now.ToString("dd-MM-yyyy HH:mm"), DateFont));
                            cell5.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            cell5.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;


                            Headertable.AddCell(LogoCell);
                            Headertable.AddCell(cell1);
                            Headertable.AddCell(cell2);

                            Headertable.AddCell(new PdfPCell(new Phrase(" ")));
                            Headertable.AddCell(cell4);
                            Headertable.AddCell(cell5);
                            

                            doc.Add(Headertable);

                            PdfPTable table = new PdfPTable(4);

                            table.TotalWidth = 400f;
                            table.LockedWidth = true;
                            float[] widthsTable = new float[] { 250f, 50f, 50f, 50f };
                            table.SetWidths(widthsTable);

                            addCell(table, "Désignation", 0, "");
                            addCell(table, "Qte", 0, "");
                            addCell(table, "Prix unit", 0, "");

                            addCell(table, "Total", 0, "");

                            foreach (DataRow row in dt.Rows)
                            {
                                string Id = row["Id"].ToString();
                                string Product = row["CMD_Product"].ToString();
                                string Qte = row["CMD_Qte"].ToString();
                                string Client = row["CMD_Client"].ToString();
                                string ProductPrice = row["CMD_ProductPrice"].ToString();
                                string Isdelivred = row["CMDIsDelivred"].ToString();
                                string Iscancled = row["CMDIsCancled"].ToString();
                                string CmdDate = row["CMD_CmdDate"].ToString();
                                string ConfirmDate = row["CMD_ConfirmationDate"].ToString();
                                string CancelDate = row["CMD_CancelDate"].ToString();
                                string NewPrice = row["CMD_NewPrice"].ToString();

                                float? ProductPrix = ClientCommandController.GetPriceByName(Product);
                                float? ProductTotal = ProductPrix*Convert.ToInt32(Qte);

                                float? ProductPrixNew;
                                float? ProductTotalNew;                                

                                if (Client.Equals(ClientCommandController.GetClientByID(Convert.ToInt32(ClientForPay))))
                                {
                                    string state = "";
                                    if (Iscancled.Equals("True"))
                                    {
                                        state = "True";
                                    }
                                    addCell(table, Product, 0, state);
                                    addCell(table, Qte, 0, state);
                                    if (NewPrice.Equals("0"))
                                    {
                                        addCell(table, ProductPrix.ToString(), 0, state);
                                        addCell(table, ProductTotal.ToString(), 0, state);

                                        if (!String.IsNullOrEmpty(ProductPrice) && !String.IsNullOrEmpty(Qte) && Iscancled.Equals("False"))
                                            TotalPayed = TotalPayed + ((float.Parse(ProductPrice)) * (Convert.ToInt32(Qte)));

                                    }
                                    else
                                    {
                                        ProductPrixNew = float.Parse(NewPrice);
                                        ProductTotalNew = ProductPrixNew * Convert.ToInt32(Qte);

                                        addCell(table, ProductPrixNew.ToString(), 0, state);
                                        addCell(table, ProductTotalNew.ToString(), 0, state);

                                        if (!String.IsNullOrEmpty(ProductPrixNew.ToString()) && !String.IsNullOrEmpty(Qte) && Iscancled.Equals("False"))
                                            TotalPayed = TotalPayed + (float.Parse(ProductTotalNew.ToString()));
                                    }
                                        
                                    Console.WriteLine(" Id :" + Id + " Product : " + Product + " ProductPrice : " + ProductPrice + " Qte : " + Qte + " Client :" + Client + " Isdelivred : " + Isdelivred + " Iscancled :" + Iscancled + " CmdDate : " + CmdDate + " ConfirmDate :" + ConfirmDate + " CancelDate :" + CancelDate + " MethodPay :" + MethodPay + " ChequeNum :" + ChequeNum + " ClientForPay :" + ClientForPay);
                                    
                                }
                            }
                            Font lightblue = new Font(Font.FontFamily.COURIER, 20, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                            //Chunk TitleChunk = new Chunk("Environment", lightblue);
                            iTextSharp.text.Paragraph Title = new iTextSharp.text.Paragraph(" BON DE COMMANDE ", lightblue);
                            Title.Font = new Font(FontFactory.GetFont("Arial", 16, Font.BOLD));
                            Title.Alignment = Element.ALIGN_CENTER; 
                            doc.Add(Title);
                            doc.Add(new Phrase(" \n"));
                            doc.Add(table);

                            doc.Add(new Phrase(" \n \n         Total Facture à payer :     " + Math.Round(TotalPayed, 2) + "      DH TTC"));

                            doc.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + " \n \n "+ex.StackTrace, "Message");

                        }
                    }
                }
                else
                {
                    
                }



                ClientCommandinitial(Convert.ToInt32(ClientForPay));
                initial();
                MessageBox.Show("Bon de Commande Bien Enregistrée");
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Erreur : \n " + ex.StackTrace + " \n \n " + ex.Message);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ClientCommandController.delete(Convert.ToInt32(clientCmd_id.SelectedValue.ToString()));
            initial();
            ClientCommandinitial(0);
        }

        private void CancelCmd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (G_CL_CMD.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < G_CL_CMD.SelectedItems.Count; i++)
                    {
                        System.Data.DataRowView selectedFile = (System.Data.DataRowView)G_CL_CMD.SelectedItems[i];
                        string str = Convert.ToString(selectedFile.Row.ItemArray[0]);
                        Console.WriteLine(" my row :" + str);
                    }
                }

                DataRowView row = G_CL_CMD.SelectedItem as DataRowView;

                if (row != null)
                {
                    MessageBoxResult d = MessageBox.Show("Voulez-vous vraiment supprimer la(les) ligne(s) sélectionnée(s) ?", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (d == MessageBoxResult.Yes)
                    {
                        foreach (DataRowView selectedrows in G_CL_CMD.SelectedItems)
                        {
                            string PK = selectedrows[0].ToString();

                            Console.WriteLine("my pk : " + PK);
                        }
                        MessageBox.Show("Paramétrage Supprimé");
                        G_CL_CMD.ItemsSource = crudctx.ClientCommand.ToList();
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionnez une ligne");
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Veuillez sélectionnez une ligne");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur :" + ex.Message + "  \n " + ex.StackTrace);
            }
        }

        private void G_ClientCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CancelCmd_Click_1(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility =
                    row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        private void G_CL_CMD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClientCommandNameSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(ClientCommandNameSearch.Text != null || !ClientCommandNameSearch.Text.Equals(""))
            {
                G_StockValable.ItemsSource = myList.Where(a => a.Product.Designation.ToLower().Contains(ClientCommandNameSearch.Text.ToLower())).ToList();
            }
        }
    }
    public static class CollectionDataClient
    {
        public static Dictionary<string, string> GetClient()
        {
            return ClientCommandController.GetClient();
        }
        public static Dictionary<string, string> GetPurchaseMethod()
        {
            return ProviderCommandController.getPurchaseMethod();
        }
    }
}
