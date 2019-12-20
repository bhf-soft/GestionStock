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
    class ProviderController
    {
        readonly StockDATAEntities crudctx = new StockDATAEntities();
        private List<Providers> provider;

        public ProviderController()
        {
            FillCategories();
        }

        private void FillCategories()
        {

            var q = (from a in crudctx.Providers
                     select a).Distinct().ToList();
            this.provider = q;
        }
        public List<Providers> PROVIDERS
        {
            get
            {
                return provider;
            }
            set
            {
                provider = value;
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
        public void InsertOrUpdate(Providers acc)
        {

            var UpdatedorInserted =
                crudctx.Providers.Where(c => c.Id.Equals(acc.Id)).FirstOrDefault();

            if (crudctx.Providers.Any(c => c.Id.Equals(acc.Id)))
            {

                UpdatedorInserted.Id = acc.Id;

                UpdatedorInserted.FullName = acc.FullName;
                UpdatedorInserted.Email = acc.Email;
                UpdatedorInserted.Patente = acc.Patente;
                UpdatedorInserted.Addres = acc.Addres;
                UpdatedorInserted.Tel = acc.Tel;
                UpdatedorInserted.City = acc.City;
                UpdatedorInserted.TypeF = acc.TypeF;
                UpdatedorInserted.Establishment = acc.Establishment;
                UpdatedorInserted.Created_at = acc.Created_at;

                UpdatedorInserted.Updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));


                MessageBox.Show(Messages.ProviderModified.Value);
            }

            else
            {
                crudctx.Providers.Add(acc);
                MessageBox.Show(Messages.ProviderAdded.Value);
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

                            var deleteProvider = crudctx.Providers.Where(m => m.Id.ToString().Equals(PK)).Single();
                            crudctx.Providers.Remove(deleteProvider);
                            crudctx.SaveChanges();
                        }
                        MessageBox.Show(Messages.ProviderDeleted.Value);
                        g.ItemsSource = crudctx.Providers.ToList();
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
