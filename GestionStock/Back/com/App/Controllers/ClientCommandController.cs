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
    class ClientCommandController
    {
        readonly StockDATAEntities crudctx = new StockDATAEntities();
        public static StockDATAEntities StkInfo = new StockDATAEntities();
        private List<EnterStock> enterStock;
        private List<ClientCommand> clientCommand;

        public ClientCommandController()
        {
            FillCategories();
        }

        private void FillCategories()
        {

            var q = (from a in crudctx.EnterStock
                     select a).Where(c => c.Qte != 0).Distinct().ToList();
            this.enterStock = q;
            ////
            ///
            var qq = (from a in crudctx.ClientCommand
                     select a).Distinct().ToList();
            this.clientCommand = qq;
        }
        public List<EnterStock> ENTERSTK
        {
            get
            {
                return enterStock;
            }
            set
            {
                enterStock = value;
                NotifyPropertyChanged();
            }
        }

        public List<ClientCommand> CLIENTCMD
        {
            get
            {
                return clientCommand;
            }
            set
            {
                clientCommand = value;
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
        public void InsertOrUpdate(ClientCommand acc)
        {
                crudctx.ClientCommand.Add(acc);
                crudctx.SaveChanges();
                MessageBox.Show(Messages.ProductAdded.Value);

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
                            Console.WriteLine("my pk " + PK);

                            var deleteenterStock = crudctx.EnterStock.Where(m => m.Id.ToString().Equals(PK)).Single();
                            crudctx.EnterStock.Remove(deleteenterStock);
                            crudctx.SaveChanges();
                        }
                        MessageBox.Show(Messages.ProductDeleted.Value);
                        g.ItemsSource = crudctx.EnterStock.ToList();
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

        public static Dictionary<string, string> GetClient()
        {
            var map = new Dictionary<string, string>();
            var q = (from a in StkInfo.Clients
                     select new
                     {
                         id = a.Id,
                         name = a.FullName
                     }).Distinct().ToList();

            for (int i = 0; i < q.Count; i++)
            {
                map.Add(q[i].id.ToString(), q[i].name);
            }
            return map;
        }

        public static DataTable GetClientCmd()
        {
            DataTable table = new DataTable();
            DataColumn dc = table.Columns.Add("Id", typeof(int));
            table.Columns.Add("CMD_Product", typeof(string));
            table.Columns.Add("CMD_Qte", typeof(int));
            table.Columns.Add("CMD_Client", typeof(string));
            //table.Columns.Add("CMD_User", typeof(string));
            table.Columns.Add("CMDIsDelivred", typeof(Boolean));
            table.Columns.Add("CMDIsCancled", typeof(Boolean));
            table.Columns.Add("CMD_CmdDate", typeof(DateTime));
            table.Columns.Add("CMD_ConfirmationDate", typeof(DateTime));
            table.Columns.Add("CMD_CancelDate", typeof(DateTime));

            using (StockDATAEntities context = new StockDATAEntities())
            {
                var query2 = from i in context.ClientCommand
                             select new { i.Id,
                                          i.Product.Designation,
                                          i.Qte,
                                          i.Clients.FullName,
                                          //i.Users.FullName
                                          i.IsDelivred,
                                          i.IsCancled,
                                          i.CmdDate,
                                          i.ConfirmationDate,
                                          i.CancelDate
                                          };

                query2.ToList().ForEach((n) =>
                {
                    DataRow row = table.NewRow();

                    row.SetField<int>("Id", n.Id);
                    row.SetField<string>("CMD_Product", n.Designation);
                    row.SetField<int?>("CMD_Qte", n.Qte);
                    row.SetField<string>("CMD_Client", n.FullName);
                    row.SetField<bool?>("CMDIsDelivred", n.IsDelivred);
                    row.SetField<bool?>("CMDIsCancled", n.IsCancled);
                    row.SetField<DateTime?>("CMD_CmdDate", n.CmdDate);
                    row.SetField<DateTime?>("CMD_ConfirmationDate", n.ConfirmationDate);
                    row.SetField<DateTime?>("CMD_CancelDate", n.CancelDate);

                    table.Rows.Add(row);
                });

            }
            return table;
        }

        public static string GetClientByID(int id)
        {
            string clientName = "";
            var q = (from a in StkInfo.Clients.Where(a => a.Id == id)
                     select new
                     {
                         name = a.FullName
                     }).Distinct().ToList();

            clientName = q[0].name;
            return clientName;
        }
    }
}
