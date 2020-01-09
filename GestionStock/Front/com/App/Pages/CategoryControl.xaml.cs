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
    /// Logique d'interaction pour CategoryControl.xaml
    /// </summary>
    public partial class CategoryControl : UserControl
    {
        private int numberOfRecPerPage;
        public List<string> Items = new List<string>();
        static Paging PagedTable = new Paging();
        public Boolean selected = false;
        public DataRowView SelectedRow = null;

        static CategoryController CategoryLists;
        public List<string> analyseSession = new List<string>();
        private static List<Category> Categorie;
        public static string groupe = "";
        private static List<Category> myList = new List<Category>();

        public CategoryControl()
        {
            InitializeComponent();
            PagedTable.type = typeof(Category);
            initial();
        }
        public void initial()
        {
            CategoryLists = new CategoryController();
            Categorie = CategoryLists.CATEGORIES.ToList<Category>();
            myList = Categorie;
            pagy();
        }
        private void Backwards_Click(object sender, RoutedEventArgs e)
        {
            G_Category.ItemsSource = PagedTable.Previous(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void First_Click(object sender, RoutedEventArgs e)
        {
            G_Category.ItemsSource = PagedTable.First(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void Last_Click(object sender, RoutedEventArgs e)
        {
            G_Category.ItemsSource = PagedTable.Last(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)    //For each of these you call the direction you want and pass in the List and ComboBox output
        {                                                               //and use the above function to output the Record number to the Label
            G_Category.ItemsSource = PagedTable.Next(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }
        private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)  //I couldn't get this function to update in place (if the grid showed 20 and I selected 100 it would jump to 200)
        {                                                                                          //So instead I had it call the First function and that does an acceptable job.
            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            G_Category.ItemsSource = PagedTable.First(myList.Cast<Object>().ToList(), numberOfRecPerPage).DefaultView;
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

            PagedTable.type = typeof(Category);

            DataTable firstTable = PagedTable.SetPaging(myList.Cast<Object>().ToList(), numberOfRecPerPage); //Fill a DataTable with the First set based on the numberOfRecPerPage

            G_Category.ItemsSource = firstTable.DefaultView;
        }

        private void Nouveau_Click(object sender, RoutedEventArgs e)
        {
            CategoryForm categoryForm = new CategoryForm();
            categoryForm.Show();
            categoryForm.Closed += delegate
            {
                initial();
            };
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = G_Category.SelectedItem as DataRowView;
                if (row != null)
                {
                    CategoryForm categoryForm = new CategoryForm();
                    categoryForm.Show();
                    categoryForm.Id.Text = row.Row.ItemArray[0].ToString();
                    categoryForm.Id.IsEnabled = false;

                    categoryForm.Designation.Text = row.Row.ItemArray[1].ToString();

                    categoryForm.Closed += delegate {
                        initial();
                    };
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
                //log.Info(Loggers.isUknowenError.Value);
                MessageBox.Show("Erreur Interne : \n \n " + ex.Message + " \n " + ex.StackTrace);
            }
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            CategoryLists.delete(G_Category);
            initial();
        }
        private void CategoryNameSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GlobalHellper.search(G_Category, CategoryNameSearch, "Category", "Designation");
        }
    }
}
