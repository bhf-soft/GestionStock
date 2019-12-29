using GestionStock.Back.com.App.HelpersModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GestionStock.Back.com.App.Controllers
{
    class ProviderCommandController
    {
        readonly StockDATAEntities crudctx = new StockDATAEntities();
        private List<ProviderCommand> providerCommand;
        
        public ProviderCommandController()
        {
            FillCategories();
        }

        private void FillCategories()
        {

            var q = (from a in crudctx.ProviderCommand
                     select a).Distinct().ToList();
            this.providerCommand = q;
        }
        public List<ProviderCommand> PROVIDERCOMMAND
        {
            get
            {
                return providerCommand;
            }
            set
            {
                providerCommand = value;
                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void InsertOrUpdate(ProviderCommand acc)
        {

            var UpdatedorInserted =
                crudctx.ProviderCommand.Where(c => c.Id.Equals(acc.Id)).FirstOrDefault();

            if (crudctx.ProviderCommand.Any(c => c.Id.Equals(acc.Id)))
            {

                UpdatedorInserted.Id = acc.Id;

                UpdatedorInserted.Product_id = acc.Product_id;
                UpdatedorInserted.Qte = acc.Qte;
                UpdatedorInserted.Provider_id = acc.Provider_id;
                UpdatedorInserted.Users_id = acc.Users_id;
                UpdatedorInserted.IsDelivred = acc.IsDelivred;
                UpdatedorInserted.IsCancled = acc.IsCancled;
                UpdatedorInserted.CmdDate = acc.CmdDate;
                UpdatedorInserted.ConfirmationDate = acc.ConfirmationDate;
                UpdatedorInserted.CancelDate = acc.CancelDate;

                MessageBox.Show(Messages.ProviderCommandModified.Value);
            }

            else
            {
                crudctx.ProviderCommand.Add(acc);
                MessageBox.Show(Messages.ProviderCommandAdded.Value);
            }

            crudctx.SaveChanges();
        }

        public void delete(DataGrid g)
        {
            try
            {
                DataRowView row = g.SelectedItem as DataRowView;
                if (row != null)
                {
                    MessageBoxResult d = MessageBox.Show(Messages.DeleteAlerteText.Value, Messages.DeleteAlerteTitle.Value, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (d == MessageBoxResult.Yes)
                    {
                        foreach (DataRowView selectedrows in g.SelectedItems)
                        {
                            string PK = selectedrows[0].ToString();

                            var deleteProviderCmd = crudctx.ProviderCommand.Where(m => m.Id.ToString().Equals(PK)).Single();
                            crudctx.ProviderCommand.Remove(deleteProviderCmd);
                            crudctx.SaveChanges();
                        }
                        MessageBox.Show(Messages.ProviderCommandDeleted.Value);
                        g.ItemsSource = crudctx.ProviderCommand.ToList();
                    }
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
                MessageBox.Show("Erreur :" + ex.Message + "  \n " + ex.StackTrace);
            }
        }

        public void Stocker(DataGrid g,string PayedAmount, string PurchaseMethod, string PurchaseNumber)
        {
            try
            {
                DataRowView row = g.SelectedItem as DataRowView;
                if (row != null)
                {
                    MessageBoxResult d = MessageBox.Show(Messages.DeleteAlerteText.Value, Messages.DeleteAlerteTitle.Value, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (d == MessageBoxResult.Yes)
                    {
                        float TotalBill = 0;
                        foreach (DataRowView selectedrows in g.SelectedItems)
                        {
                            Random r = new Random();
                            ProviderBill providerBill = new ProviderBill();
                            EnterStock enterStock = new EnterStock();
                            string Cmd_PK = selectedrows[0].ToString();
                            string Cmd_Product = selectedrows[1].ToString();
                            string Cmd_Qte = selectedrows[2].ToString();
                            string Cmd_Provider = selectedrows[3].ToString();
                            string Cmd_User = selectedrows[4].ToString();

                            Console.WriteLine(" Cmd_PK  : " + Cmd_PK + " Cmd_Product :" + Cmd_Product
                                + " Cmd_Qte :" + Cmd_Qte + " Cmd_Provider : " + Cmd_Provider + " Cmd_User :" + Cmd_User);
                            /////
                            ///
                            providerBill.Id = r.Next();
                            providerBill.ProviderCommand_id = Convert.ToInt32(Cmd_PK);
                            providerBill.Users_id = Convert.ToInt32(Cmd_User);
                            providerBill.PurchaseDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            providerBill.PurchaseMethod = PurchaseMethod;

                            if (PurchaseMethod.Equals("Chèque"))
                                providerBill.PurchaseNumber = PurchaseNumber;

                            var q = crudctx.Product.Single(_ => _.Id.ToString().Equals(Cmd_Product)).Price.Value.ToString();

                            TotalBill = TotalBill + (float.Parse(q)* float.Parse(Cmd_Qte));

                            Console.WriteLine(" Total bill  : " + TotalBill);

                            Console.WriteLine(" providerBill.Id  : " + providerBill.Id + " providerBill.ProviderCommand_id :" + providerBill.ProviderCommand_id
                                + " providerBill.PurchaseMethod :" + providerBill.PurchaseMethod + " providerBill.PurchaseNumber : " + providerBill.PurchaseNumber +
                                " providerBill.Users_id :" + providerBill.Users_id + " providerBill.PurchaseDate : " + providerBill.PurchaseDate +
                                " providerBill.Total : " + providerBill.Total + " providerBill.Rest : " + providerBill.Rest);

                            crudctx.ProviderBill.Add(providerBill);
                            crudctx.SaveChanges();

                            // Stock enter 
                            enterStock.Id = r.Next();
                            enterStock.Product_id = Convert.ToInt32(Cmd_Product);
                            enterStock.Qte = Convert.ToInt32(Cmd_Qte);
                            enterStock.Provider_id = Convert.ToInt32(Cmd_Provider);
                            enterStock.Users_id = Convert.ToInt32(Cmd_User);
                            enterStock.EnterDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                            crudctx.EnterStock.Add(enterStock);
                            crudctx.SaveChanges();

                        }

                        var result = crudctx.ProviderBill.ToList();
                        for(int i = 0; i < result.Count; i++)
                        {
                            if (result != null)
                            {
                                result[i].Total = TotalBill;
                                float rest = TotalBill - float.Parse(PayedAmount);
                                if (float.Parse(PayedAmount) > result[i].Total)
                                    result[i].Rest = 0;
                                else
                                result[i].Rest = TotalBill - float.Parse(PayedAmount);
                                crudctx.SaveChanges();
                            }
                        }
                        
                        MessageBox.Show(Messages.ProviderCommandBillAdded.Value);
                        g.ItemsSource = crudctx.ProviderCommand.ToList();
                    }
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
            /*catch (Exception ex)
            {
                MessageBox.Show("Erreur :" + ex.Message + "  \n " + ex.StackTrace);
            }*/
        }
        public static Dictionary<string, string> getPurchaseMethod()
        {
            var map = new Dictionary<string, string>();

                map.Add("Chèque", "Chèque");
                map.Add("Cash", "Cash");
            return map;
        }
    }
}
