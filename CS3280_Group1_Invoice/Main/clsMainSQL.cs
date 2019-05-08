using Assignment6AirlineReservation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CS3280_Group1_Invoice.Main
{
   
    /// <summary>
    /// This class handles all of the queries to my database.
    /// </summary>
    class clsMainSQL
    {
        #region Attributes
        /// <summary>
        /// declaring and instantiaing a new dataset class
        /// </summary>
        private DataSet ds = new DataSet();
        /// <summary>
        /// declaring and instantiaing a new DataAccess class
        /// </summary>
        private clsDataAccess db = new clsDataAccess();
        /// <summary>
        /// Declaring a new item object class
        /// </summary>
        private clsItem item;
        /// <summary>
        /// Declaring a new observable collection of type clsItem which is used to hold our list of items
        /// </summary>
        private ObservableCollection<clsItem> itemList;
        /// <summary>
        /// string thats temp db queries.
        /// </summary>
        private string databaseQuery;
        /// <summary>
        /// holds a temps number of return values after executing a query
        /// </summary>
        private int numOfReturnVal = 0;
        #endregion

        #region c-tor
        /// <summary>
        /// ctor
        /// </summary>
        public clsMainSQL()
        {

        }
        #endregion

        #region Select Statements
        /*-----------------------------------------------------------(Get) Select Statement Queries------------------------------------*/

        /// <summary>
        /// Populates the item list with the currect items from our database
        /// </summary>
        public void PopulateItemList()
        {
            try
            {
                itemList = new ObservableCollection<clsItem>();
                int intCost = 0;

                databaseQuery = "SELECT ItemCode, ItemDesc, Cost " +
                    "FROM ItemDesc";

                ds = db.ExecuteSQLStatement(databaseQuery, ref numOfReturnVal);

                for (int i = 0; i < numOfReturnVal; i++)
                {
                    string code = ds.Tables[0].Rows[i][0].ToString();
                    string desc = ds.Tables[0].Rows[i][1].ToString();
                    string cost = ds.Tables[0].Rows[i][2].ToString();

                    //parse string to int 
                    Int32.TryParse(cost, out intCost);

                    //uses each of the column info to create an item object and adds it to our OC list
                    item = new clsItem();
                    item.description = desc;
                    item.cost = intCost;
                    item.itemCode = code;

                    itemList.Add(item);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
           
        }
        
        /// <summary>
        /// Gets the invoice number accociated with a given date and cost
        /// </summary>
        /// <param name="date">string representation of the date</param>
        /// <param name="cost">total cost of the invoice items</param>
        /// <returns></returns>
        public int GetInvoiceNumber(string date, int cost)
        {
            try
            {
                int invoiceNum;
                string temp = "";

                databaseQuery = "SELECT InvoiceNum " +
                        "FROM Invoices WHERE TotalCost = " + cost +
                        " AND InvoiceDate = #" + date + "#";

                ds = db.ExecuteSQLStatement(databaseQuery, ref numOfReturnVal);
                for (int i = 0; i < numOfReturnVal; i++)
                {
                    temp = ds.Tables[0].Rows[i][0].ToString();
                }

                //parse string to int 
                Int32.TryParse(temp, out invoiceNum);

                return invoiceNum;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Populates a list that holds all of the items for a given invoice.
        /// </summary>
        /// <param name="invoiceNum">Invoice number</param>
        /// <returns>item list associated with an invoice num </returns>
        public ObservableCollection<clsItem> GetInvoiceItems(int invoiceNum)
        {
            try
            {
                int intCost;
                ObservableCollection<string> codeList = new ObservableCollection<string>();
                ObservableCollection<clsItem> custList = new ObservableCollection<clsItem>();


                //gets all of the item codes from the LineItems
                databaseQuery = "SELECT ItemCode " +
                   "FROM LineItems WHERE InvoiceNum = " + invoiceNum + "";

                ds = db.ExecuteSQLStatement(databaseQuery, ref numOfReturnVal);

                //adds each item from an invoice to the code list.
                for (int i = 0; i < numOfReturnVal; i++)
                {
                    string code = ds.Tables[0].Rows[i][0].ToString();
                    codeList.Add(code);
                }

                //for each code we got from our LineItems with the given invoiceNum,
                //we query the ItemDesc table and get the item associated with the code and 
                //add it to a list of the invoces items.
                for (int i = 0; i < codeList.Count; i++)
                {
                    //gets all of the items from ItemDesc and creates the list
                    databaseQuery = "SELECT ItemCode, ItemDesc, Cost " +
                            "FROM ItemDesc WHERE ItemCode = " + "'" + codeList.ElementAt(i).ToString() + "'";

                    ds = db.ExecuteSQLStatement(databaseQuery, ref numOfReturnVal);

                    string code = ds.Tables[0].Rows[0][0].ToString();
                    string desc = ds.Tables[0].Rows[0][1].ToString();
                    string cost = ds.Tables[0].Rows[0][2].ToString();

                    //parse string to int 
                    Int32.TryParse(cost, out intCost);

                    item = new clsItem();
                    item.description = desc;
                    item.cost = intCost;
                    item.itemCode = code;

                    custList.Add(item);

                }

                return custList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
           
        }

        /// <summary>
        /// Queries the Invoices table for the date given an invoice number.
        /// </summary>
        /// <returns>return the date for a given invoice number.</returns>
        public string GetInvoiceDate(int invoiceNum)
        {
            try
            {
                string date = "";

                databaseQuery = "SELECT InvoiceDate " +
                        "FROM Invoices WHERE InvoiceNum = " + invoiceNum + "";

                ds = db.ExecuteSQLStatement(databaseQuery, ref numOfReturnVal);

                for (int i = 0; i < numOfReturnVal; i++)
                {
                    date = ds.Tables[0].Rows[i][0].ToString();

                }

                return date;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
        #endregion

        #region Insert Statements
        /*-----------------------------------------------------------Insert Statement Queries------------------------------------*/
        /// <summary>
        /// inserts a new invoice into the invoices table
        /// </summary>
        /// <param name="date">string representation of the date</param>
        /// <param name="cost">total cost of the items in an invoice</param>
        public void InsertIntoInvoices(string date, int cost)
        {
            try
            {
                databaseQuery = "INSERT INTO Invoices (InvoiceDate, TotalCost) " +
                   "VALUES ('" + date + "', '" + cost + "')";

                db.ExecuteNonQuery(databaseQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
           
        }


        /// <summary>
        /// Inserts the items from the passed list into LineItems with the given invoice number.
        /// </summary>
        /// <param name="invoiceNum">invoice number to be inseted</param>
        /// <param name="custList">List of the items in the customers list</param>
        public void InsertIntoLineItems(int invoiceNum, ObservableCollection<clsItem> custList)
        {
            try
            {
                for (int i = 0; i < custList.Count; i++)
                {
                    clsItem item = custList.ElementAt(i);
                    databaseQuery = "INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) " +
                        "VALUES ('" + invoiceNum + "', '" + i + 1 + "', '" + item.itemCode + "')";

                    db.ExecuteNonQuery(databaseQuery);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
           

        }

        /// <summary>
        /// Updates the list of items in the LineItems table associated to the given invoice number if it is being editing, not created
        /// </summary>
        /// <param name="custList">list of customer items</param>
        /// <param name="invoiceNum">invoice number associated to the list</param>
        public void UpdateEditedInvoices(ObservableCollection<clsItem> custList, int invoiceNum)
        {
            try
            {
                //located in helper methods
                DeleteLineItems(invoiceNum);

                for (int i = 0; i < custList.Count; i++)
                {
                    clsItem myItem = custList.ElementAt(i);

                    databaseQuery = "INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) " +
                        "VALUES ('" + invoiceNum + "', '" + i + 1 + "', '" + myItem.itemCode + "')";

                    db.ExecuteNonQuery(databaseQuery);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// updates the invoices table's date and total cost
        /// </summary>
        /// <param name="invoiceNum">Invoice number</param>
        /// <param name="date">string representation of the date</param>
        /// <param name="totalCost">total cost of the added up items</param>
        public void UpdateInvoiceInfo(int invoiceNum, string date, int totalCost)
        {
            try
            {
                databaseQuery = "UPDATE Invoices " +
               "SET InvoiceDate = #" + date + "#, " + "TotalCost = " + totalCost + " " +
               "WHERE InvoiceNum = " + invoiceNum;


                db.ExecuteNonQuery(databaseQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            
        }
        #endregion

        #region Delete Statements
        /*-----------------------------------------------------------Delete Statement Queries------------------------------------*/

        /// <summary>
        /// Deletes the items from the LineItems table with the given invoice, then deletes the invoice from the Invoices table
        /// </summary>
        /// <param name="invoiceNum">Invoice number that is to be deleted from the database</param>
        public void DeleteInvoice(int invoiceNum)
        {
            try
            {
                //deletes all of the lineItems with the given invoice number
                databaseQuery = "DELETE " +
                   "FROM LineItems WHERE InvoiceNum = " + invoiceNum + "";
                db.ExecuteNonQuery(databaseQuery);


                //deltes the invoice in Invoices
                databaseQuery = "DELETE " +
                    "FROM Invoices WHERE InvoiceNum = " + invoiceNum + "";
                db.ExecuteNonQuery(databaseQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            

        }

        #endregion

        #region List Getters
        /*-----------------------------------------------------------------List Getters------------------------------------*/

        public ObservableCollection<clsItem> GetItemList()
        {
            try
            {
                return itemList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        #region Helper Methods
        /*-----------------------------------------------------------------Helper Methods----------------------------------------------*/

        /// <summary>
        /// deletes all of the items with a given invoice number 
        /// </summary>
        /// <param name="invoiceNum">invoice number for desired updated invoice</param>
        private void DeleteLineItems(int invoiceNum)
        {
            try
            {
                databaseQuery = "DELETE " +
                   "FROM LineItems WHERE InvoiceNum = " + invoiceNum + "";

                db.ExecuteNonQuery(databaseQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            
        }

        #endregion

        #region Other Commented-Out Code

        /*-----------------------------------------------------Failed Experiment-------------------------------------------

        //very hard (UpdateEditedInvoice doesn't work) way to updated lists. 

        public void UpdateEditedInvoice(ObservableCollection<clsItem> addedItems, ObservableCollection<clsItem> removedItems, int invoiceNum, int lineItemNum)
        {
            for (int i = 0; i < addedItems.Count; i++)
            {
                MessageBox.Show( "INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) " +
                   "VALUES ('" + invoiceNum + "', '" + lineItemNum++ + "', '" + item.itemCode + "')");


                databaseQuery = "INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) " +
                    "VALUES ('" + invoiceNum + "', '" + lineItemNum++ + "', '" + item.itemCode + "')";

                db.ExecuteNonQuery(databaseQuery);
            }

            for (int i = 0; i < removedItems.Count; i++)
            {

                clsItem item = removedItems.ElementAt(i);

                databaseQuery = "DELETE " +
                    "FROM LineItems WHERE InvoiceNum = " + invoiceNum + " " +
                    "AND ItemCode = " + "'" + item.itemCode + "'";

                db.ExecuteNonQuery(databaseQuery);
            }
            
        }

        public void UpdateTotal(int invoiceNum, int total)
        {
            databaseQuery = "UPDATE Invoices " +
               "SET TotalCost = " + total + " " +
               "WHERE InvoiceNum = '" + invoiceNum + "'";

            db.ExecuteNonQuery(databaseQuery);
        }
        */
        #endregion
    }
}
