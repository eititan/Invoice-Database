using Assignment6AirlineReservation;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;

namespace CS3280_Group1_Invoice.Items
{
    class clsItemsSQL
    {
        /// <summary>
        /// Variable for querying database
        /// </summary>
        clsDataAccess access = new clsDataAccess();
        /// <summary>
        /// A global index used for all queries
        /// </summary>
        public int index = 0;
        /// <summary>
        /// Initiates an item that is used throughout the query
        /// </summary>
        private clsItem item;
        /// <summary>
        /// The dataset that is used throughout the class 
        /// </summary>
        private DataSet ds;
        /// <summary>
        /// An item list that is returned to populate the datagrid
        /// </summary>
        private ObservableCollection<clsItem> itemList;
        /// <summary>
        /// Returns all the items in the database
        /// </summary>
        /// <returns></returns>
        public DataSet GetItems()
        {
            try
            {
                string sql = @"SELECT * FROM ItemDesc";
                return access.ExecuteSQLStatement(sql, ref index);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Delets a specified item from the database
        /// </summary>
        /// <param name="code"></param>
        public void DeleteItem(string code)
        {
            try
            {
                    string sql = $"DELETE FROM ItemDesc where ItemCode = '{code}'";
                    access.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Inserts the given item from the database
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Desc"></param>
        /// <param name="Cost"></param>
        public void InsertItem(string code, string Desc, string Cost)
        {
            try { 
            string sql = $"INSERT INTO ItemDesc (ItemCode, ItemDesc, Cost) VALUES ('{code}', '{Desc}', {Cost})";
                Console.WriteLine(sql);
            access.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Saves the adjustments made to an existing item
        /// </summary>
        /// <param name="code"></param>
        /// <param name="desc"></param>
        /// <param name="cost"></param>
        public void SaveChanges(string code, string desc, string cost)
        {
            try { 
            int price = Convert.ToInt32(cost);
            string sql = $@"UPDATE ItemDesc SET ItemDesc = '{desc}', Cost = {cost} WHERE ItemCode = '{code}'";
            access.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets all the item codes used in invoices
        /// </summary>
        /// <returns></returns>
        public DataSet GetLineItemCodes()
        {
            try { 
            string sql = $@"SELECT DISTINCT ItemCode from LineItems";
            return access.ExecuteSQLStatement(sql, ref index);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets all existing item codes
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllItemCodes()
        {
            try { 
            string sql = $@"SELECT ItemCode from ItemDesc ORDER BY ItemCode";
            return access.ExecuteSQLStatement(sql, ref index);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Responsible for refreshing the datagrid.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<clsItem> PopulateItemList()
        {
            try { 
            itemList = new ObservableCollection<clsItem>();
            int intCost = 0;

            string databaseQuery = "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc";

            ds = access.ExecuteSQLStatement(databaseQuery, ref index);

            for (int i = 0; i < index; i++)
            {
                string code = ds.Tables[0].Rows[i][0].ToString();
                string desc = ds.Tables[0].Rows[i][1].ToString();
                string cost = ds.Tables[0].Rows[i][2].ToString();

                //parse string to int 
                Int32.TryParse(cost, out intCost);

                item = new clsItem();
                item.description = desc;
                item.cost = intCost;
                item.itemCode = code;

                itemList.Add(item);
            }
            return itemList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Error Handling method for the top level.
        /// </summary>
        /// <param name="sClass">Current class</param>
        /// <param name="sMethod">current method</param>
        /// <param name="sMessage">passed in exception(s)</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                //Would write to a file or database
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception ex)
            {
                File.AppendAllText("C\\Error.txt", Environment.NewLine + "HandleError Exception: "
                    + ex.Message);
            }
        }
    }
}