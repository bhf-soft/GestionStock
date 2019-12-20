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
    class ClientController
    {
        readonly StockDATAEntities crudctx = new StockDATAEntities();
        private List<Clients> client;

        public ClientController()
        {
            FillCategories();
        }

        private void FillCategories()
        {

            var q = (from a in crudctx.Clients
                     select a).Distinct().ToList();
            this.client = q;
        }
        public List<Clients> CLIENTS
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
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
        public void InsertOrUpdate(Clients acc)
        {

            var UpdatedorInserted =
                crudctx.Clients.FirstOrDefault();

            if (crudctx.Clients.Any())
            {

                UpdatedorInserted.Id = acc.Id;

                UpdatedorInserted.FullName = acc.FullName;
                UpdatedorInserted.Email = acc.Email;
                UpdatedorInserted.Patente = acc.Patente;
                UpdatedorInserted.Adress = acc.Adress;
                UpdatedorInserted.Tel = acc.Tel;
                UpdatedorInserted.City = acc.City;
                UpdatedorInserted.TypeC = acc.TypeC;
                UpdatedorInserted.Establishment = acc.Establishment;
                UpdatedorInserted.Created_at = acc.Created_at;
                UpdatedorInserted.Updated_at = acc.Updated_at;


                MessageBox.Show(Messages.ClientModified.Value);
            }

            else
            {
                crudctx.Clients.Add(acc);
                MessageBox.Show(Messages.ClientAdded.Value);
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

                            var deletePortfolio = crudctx.Clients.Where(m => m.Id.Equals(PK)).Single();
                            crudctx.Clients.Remove(deletePortfolio);
                            crudctx.SaveChanges();
                        }
                        MessageBox.Show(Messages.ClientDeleted.Value);
                        g.ItemsSource = crudctx.Clients.ToList();
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
