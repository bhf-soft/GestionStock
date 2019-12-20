using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Back.com.App.HelpersModels
{
    class Paging
    {
        /// <summary>
        /// Current Page Index Number
        /// </summary>
        ///  StudentModel.Student ==> PR_ACCOUNT

        /// 
        public int PageIndex { get; set; }

        DataTable PagedList = new DataTable(); //Initialize a DataTable Locally

        public Type type;


        /// <summary>
        /// Show the next set of Items based on page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable Next(IList<Object> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Show the previous set of items base on page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable Previous(IList<Object> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Show first the set of Items in the page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable First(IList<Object> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Show the last set of items in the page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable Last(IList<Object> ListToPage, int RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        /// Performs a LINQ Query on the List and returns a DataTable
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
		public DataTable SetPaging(IList<Object> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;

            IList<Object> PagedList = new List<Object>();

            PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList(); //This is where the Magic Happens. If you have a Specific sort or want to return ONLY a specific set of columns, add it to this LINQ Query.

            DataTable FinalPaging = PagedTable(PagedList);

            return FinalPaging;
        }

        //If youre paging say 30,000 rows and you know the processors of the users will be slow you can ASync thread both of these to allow the UI to update after they finish and prevent a hang.

        /// <summary>
        /// Internal Method: Performs the Work of converting the Passed in list to a DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SourceList"></param>
        /// <returns>DataTable</returns>
		private DataTable PagedTable<T>(IList<T> SourceList)
        {
            Type columnType = type;
            DataTable TableToReturn = new DataTable();

            foreach (var Column in columnType.GetProperties())
            {
                TableToReturn.Columns.Add(Column.Name,// Column.PropertyType);

                Nullable.GetUnderlyingType(
            Column.PropertyType) ?? Column.PropertyType);


            }

            foreach (object item in SourceList)
            {
                DataRow ReturnTableRow = TableToReturn.NewRow();
                foreach (var Column in columnType.GetProperties())
                {
                    //   ReturnTableRow[Column.Name] =  Column.GetValue(item);
                    //Console.WriteLine("" + type + " / " + columnType + " / " + columnType.GetProperties() + " / " + Column + "/ " + Column.GetValue(item));

                    ReturnTableRow[Column.Name] = Column.GetValue(item) ?? DBNull.Value;


                }
                TableToReturn.Rows.Add(ReturnTableRow);
            }
            return TableToReturn;
        }
    }
}
