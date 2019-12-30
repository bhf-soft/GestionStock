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
    }
}
