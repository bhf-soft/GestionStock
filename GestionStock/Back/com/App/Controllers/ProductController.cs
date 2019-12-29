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
    class ProductController
    {
        readonly StockDATAEntities crudctx = new StockDATAEntities();
        public static StockDATAEntities StkInfo = new StockDATAEntities();
        private List<Product> product;

        public ProductController()
        {
            FillCategories();
        }

        private void FillCategories()
        {

            var q = (from a in crudctx.Product
                     select a).Distinct().ToList();
            this.product = q;
        }
        public List<Product> PRODUCTS
        {
            get
            {
                return product;
            }
            set
            {
                product = value;
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
        public void InsertOrUpdate(Product acc)
        {

            var UpdatedorInserted =
                crudctx.Product.Where(c => c.Id.Equals(acc.Id)).FirstOrDefault();

            if (crudctx.Product.Any(c => c.Id.Equals(acc.Id)))
            {

                UpdatedorInserted.Id = acc.Id;

                UpdatedorInserted.Designation = acc.Designation;
                UpdatedorInserted.Price = acc.Price;
                UpdatedorInserted.category_id = acc.category_id;
                UpdatedorInserted.Provider_id = acc.Provider_id;

                UpdatedorInserted.created_at = acc.created_at;
                UpdatedorInserted.updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));


                MessageBox.Show(Messages.ProductModified.Value);
            }

            else
            {
                crudctx.Product.Add(acc);
                MessageBox.Show(Messages.ProductAdded.Value);
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

                            var deleteProduct = crudctx.Product.Where(m => m.Id.ToString().Equals(PK)).Single();
                            crudctx.Product.Remove(deleteProduct);
                            crudctx.SaveChanges();
                        }
                        MessageBox.Show(Messages.ProductDeleted.Value);
                        g.ItemsSource = crudctx.Product.ToList();
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

        public static Dictionary<string, string> getCategory()
        {
            var map = new Dictionary<string, string>();
            var q = (from a in StkInfo.Category
                     select new
                     {
                         id = a.Id,
                         name = a.Designation
                     }).Distinct().ToList();

            for (int i = 0; i < q.Count; i++)
            {
                map.Add(q[i].id.ToString(),q[i].name);
            }
            return map;
        }

        public static Dictionary<string, string> getProduct()
        {
            var map = new Dictionary<string, string>();
            var q = (from a in StkInfo.Product
                     select new
                     {
                         id = a.Id,
                         name = a.Designation
                     }).Distinct().ToList();

            for (int i = 0; i < q.Count; i++)
            {
                map.Add(q[i].id.ToString(), q[i].name);
            }
            return map;
        }
    }
}
