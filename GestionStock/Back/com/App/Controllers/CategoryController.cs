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
    class CategoryController
    {
        readonly StockDATAEntities crudctx = new StockDATAEntities();
        private List<Category> category;

        public CategoryController()
        {
            FillCategories();
        }

        private void FillCategories()
        {

            var q = (from a in crudctx.Category
                     select a).Distinct().ToList();
            this.category = q;
        }
        public List<Category> CATEGORIES
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
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
        public void InsertOrUpdate(Category acc)
        {

            var UpdatedorInserted =
                crudctx.Category.Where(c => c.Id.Equals(acc.Id)).FirstOrDefault();

            if (crudctx.Category.Any(c => c.Id.Equals(acc.Id)))
            {

                UpdatedorInserted.Id = acc.Id;

                UpdatedorInserted.Designation = acc.Designation;

                UpdatedorInserted.created_at = acc.created_at;
                UpdatedorInserted.updated_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));


                MessageBox.Show(Messages.CategoryModified.Value);
            }

            else
            {
                crudctx.Category.Add(acc);
                MessageBox.Show(Messages.CategoryAdded.Value);
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

                            var deleteCategory = crudctx.Category.Where(m => m.Id.ToString().Equals(PK)).Single();
                            crudctx.Category.Remove(deleteCategory);
                            crudctx.SaveChanges();
                        }
                        MessageBox.Show(Messages.CategoryDeleted.Value);
                        g.ItemsSource = crudctx.Category.ToList();
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
