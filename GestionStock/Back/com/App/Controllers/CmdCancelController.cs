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
    class CmdCancelController
    {
        readonly StockDATAEntities crudctx = new StockDATAEntities();
        private List<ClientCommand> cmd;

        public CmdCancelController()
        {
            FillCategories();
        }

        private void FillCategories()
        {

            var q = (from a in crudctx.ClientCommand
                     select a).Distinct().ToList();
            this.cmd = q;
        }
        public List<ClientCommand> CMD
        {
            get
            {
                return cmd;
            }
            set
            {
                cmd = value;
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

            var UpdatedorInserted =
                crudctx.ClientCommand.Where(c => c.Id == acc.Id).FirstOrDefault();

            Console.WriteLine("my id :" + acc.Id +"  --- "+ UpdatedorInserted.Id);
            if (crudctx.ClientCommand.Any(c => c.Id == acc.Id))
            {

                UpdatedorInserted.Id = acc.Id;
                UpdatedorInserted.IsDelivred = acc.IsDelivred;
                UpdatedorInserted.IsCancled = acc.IsCancled;
                UpdatedorInserted.CancelDate = acc.CancelDate;

                


                MessageBox.Show("Commande Annulée");
            }

            else
            {
                MessageBox.Show("Erreur Interne : Commande n'existe pas !");
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
                            Console.WriteLine("my pk " + PK);

                            var deleteCategory = crudctx.ClientCommand.Where(m => m.Id.ToString().Equals(PK)).Single();
                            crudctx.ClientCommand.Remove(deleteCategory);
                            crudctx.SaveChanges();
                        }
                        MessageBox.Show("Commande Modifiée");
                        g.ItemsSource = crudctx.ClientCommand.ToList();
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
    }
}
