using Assignment6AirlineReservation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace CS3280_Group1_Invoice.Search
{
    /// <summary>
    /// Logic of the Search Window
    /// </summary>
    class clsSearchLogic
    {
        #region Attributes
        /// <summary>
        /// A dataset to hold the data after the query is executed.
        /// </summary>
        private DataSet ds;

        /// <summary>
        /// Declare the DataAccess class that is used to execute queries.
        /// </summary>
        private clsDataAccess db;

        /// <summary>
        /// Declare the Query Statement class that is used to set the queries.
        /// </summary>
        private clsSearchSQL query;

        /// <summary>
        /// String that is used to hold the Query gathered from the QueryStatements class
        /// </summary>
        public string sSQL;

        /// <summary>
        /// a count of rows of the executed query returns
        /// </summary>
        public int iRows;

        /// <summary>
        /// List of strings of Invoice Dates. Ordered by ascending (Smallest to Largest)
        /// </summary>
        public List<string> lstInvoiceDates;

        /// <summary>
        /// List of strings of Invoice Numbers. Ordered by ascending (Smallest to Largest)
        /// </summary>
        public List<int> lstInvoiceNumbers;

        /// <summary>
        /// List of integers of Invoice Totals. Ordered by ascending (Smallest to Largest)
        /// </summary>
        public List<int> lstInvoiceTotals;

        /// <summary>
        /// List of invoices gathered from the access database.
        /// </summary>
        public ObservableCollection<clsInvoice> lstInvoices;

        /// <summary>
        /// Initializes a invoice object to populate the List of Invoices.
        /// </summary>
        private clsInvoice Invoice;
        #endregion

        #region Methods
        /// <summary>
        /// Constructor initializes all of the required objects to query the access database
        /// to generate the invoice lists.
        /// </summary>
        public clsSearchLogic()
        {
            try
            {
                lstInvoiceDates = new List<string>();
                lstInvoiceTotals = new List<int>();
                lstInvoiceNumbers = new List<int>();
                db = new clsDataAccess();
                query = new clsSearchSQL();
                ds = new DataSet();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #region Combo Box Methods
        /// <summary>
        /// Gathers a list of all of the invoice totals orders them by smallest to largest
        /// </summary>
        /// <returns> a list of invoice totals</returns>
        public List<int> GetInvoiceTotals()
        {
            try
            {
                sSQL = query.GetInvoiceTotals();
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    lstInvoiceTotals.Add(Convert.ToInt32(ds.Tables[0].Rows[i][0]));
                }
                return lstInvoiceTotals;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers a list of invoice numbers and orders them by smallest to largest
        /// </summary>
        /// <returns>a list of invoice numbers</returns>
        public List<int> GetInvoiceNumbers()
        {
            try
            {
                sSQL = query.GetInvoiceNumbers();
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    lstInvoiceNumbers.Add(Convert.ToInt32(ds.Tables[0].Rows[i][0]));
                }
                return lstInvoiceNumbers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers a list of invoice dates ordered by the invoice numbers
        /// </summary>
        /// <returns>a list of string values that represent the invoice dates</returns>
        public List<string> GetInvoiceDates()
        {
            try
            {
                sSQL = query.GetInvoiceDates();
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    lstInvoiceDates.Add(ds.Tables[0].Rows[i][0].ToString());
                }
                return lstInvoiceDates;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        #region Filter Methods
        /// <summary>
        /// Gathers a list of all invoices to display on load of window
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<clsInvoice> GetAllInvoices()
        {
            try
            {
                lstInvoices = new ObservableCollection<clsInvoice>();
                sSQL = query.GetInvoices();
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);
                for (int i = 0; i < iRows; i++)
                {
                    Invoice = new clsInvoice
                    {
                        InvoiceNumber = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        InvoiceDate = ds.Tables[0].Rows[i][1].ToString(),
                        TotalCost = Convert.ToInt32(ds.Tables[0].Rows[i][2])
                    };
                    lstInvoices.Add(Invoice);
                }
                return lstInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers a list of invoice with the passed in invoice number
        /// </summary>
        /// <returns>a list of string values that represent the invoice dates</returns>
        public ObservableCollection<clsInvoice> GetInvoicesPerNumber(int InvoiceNumber)
        {
            try
            {
                lstInvoices = new ObservableCollection<clsInvoice>();
                sSQL = query.GetInvoicesPerNumber(InvoiceNumber);
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    Invoice = new clsInvoice
                    {
                        InvoiceNumber = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        InvoiceDate = ds.Tables[0].Rows[i][1].ToString(),
                        TotalCost = Convert.ToInt32(ds.Tables[0].Rows[i][2])
                    };
                    lstInvoices.Add(Invoice);
                }
                return lstInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers a list of invoice(s) with the passed in invoice total
        /// </summary>
        /// <returns>a list of string values that represent the invoice dates</returns>
        public ObservableCollection<clsInvoice> GetInvoicesPerTotal(int InvoiceTotal)
        {
            try
            {
                lstInvoices = new ObservableCollection<clsInvoice>();
                sSQL = query.GetInvoicesPerTotal(InvoiceTotal);
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    Invoice = new clsInvoice
                    {
                        InvoiceNumber = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        InvoiceDate = ds.Tables[0].Rows[i][1].ToString(),
                        TotalCost = Convert.ToInt32(ds.Tables[0].Rows[i][2])
                    };
                    lstInvoices.Add(Invoice);
                }
                return lstInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers a list of invoice(s) with the passed in invoice date
        /// </summary>
        /// <returns>a list of string values that represent the invoice dates</returns>
        public ObservableCollection<clsInvoice> GetInvoicePerDate(string InvoiceDate)
        {
            try
            {
                lstInvoices = new ObservableCollection<clsInvoice>();
                sSQL = query.GetInvoicesPerDate(InvoiceDate);
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    Invoice = new clsInvoice
                    {
                        InvoiceNumber = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        InvoiceDate = ds.Tables[0].Rows[i][1].ToString(),
                        TotalCost = Convert.ToInt32(ds.Tables[0].Rows[i][2])
                    };
                    lstInvoices.Add(Invoice);
                }
                return lstInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers a list of invoice(s) with the passed in invoice total and date
        /// </summary>
        /// <returns>a list of string values that represent the invoice dates</returns>
        public ObservableCollection<clsInvoice> GetInvoicesPerDateTotal(string InvoiceDate, int InvoiceTotal)
        {
            try
            {
                lstInvoices = new ObservableCollection<clsInvoice>();
                sSQL = query.GetInvoicesPerDateTotal(InvoiceDate, InvoiceTotal);
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    Invoice = new clsInvoice
                    {
                        InvoiceNumber = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        InvoiceDate = ds.Tables[0].Rows[i][1].ToString(),
                        TotalCost = Convert.ToInt32(ds.Tables[0].Rows[i][2])
                    };
                    lstInvoices.Add(Invoice);
                }
                return lstInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers a list of invoice(s) with the passed in invoice number and date
        /// </summary>
        /// <returns>a list of string values that represent the invoice dates</returns>
        public ObservableCollection<clsInvoice> GetInvoicesPerNumberDate(string InvoiceDate, int InvoiceNumber)
        {
            try
            {
                lstInvoices = new ObservableCollection<clsInvoice>();
                sSQL = query.GetInvoicesPerNumberDate(InvoiceNumber, InvoiceDate);
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    Invoice = new clsInvoice
                    {
                        InvoiceNumber = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        InvoiceDate = ds.Tables[0].Rows[i][1].ToString(),
                        TotalCost = Convert.ToInt32(ds.Tables[0].Rows[i][2])
                    };
                    lstInvoices.Add(Invoice);
                }
                return lstInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers a list of invoice(s) with the passed in invoice number and Total
        /// </summary>
        /// <returns>a list of string values that represent the invoice dates</returns>
        public ObservableCollection<clsInvoice> GetInvoicesPerNumberTotal(int InvoiceNumber, int InvoiceTotal)
        {
            try
            {
                lstInvoices = new ObservableCollection<clsInvoice>();
                sSQL = query.GetInvoicesPerNumberTotal(InvoiceNumber, InvoiceTotal);
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    Invoice = new clsInvoice
                    {
                        InvoiceNumber = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        InvoiceDate = ds.Tables[0].Rows[i][1].ToString(),
                        TotalCost = Convert.ToInt32(ds.Tables[0].Rows[i][2])
                    };
                    lstInvoices.Add(Invoice);
                }
                return lstInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers a list of invoice(s) with the passed in invoice number, date and total
        /// </summary>
        /// <returns>a list of string values that represent the invoice dates</returns>
        public ObservableCollection<clsInvoice> GetInvoicesAllFilterOptions(string InvoiceDate, int InvoiceTotal, int InvoiceNumber)
        {
            try
            {
                lstInvoices = new ObservableCollection<clsInvoice>();
                sSQL = query.GetInvoicesAllFilterOptions(InvoiceDate, InvoiceTotal, InvoiceNumber);
                iRows = 0;
                ds = db.ExecuteSQLStatement(sSQL, ref iRows);

                for (int i = 0; i < iRows; i++)
                {
                    Invoice = new clsInvoice
                    {
                        InvoiceNumber = Convert.ToInt32(ds.Tables[0].Rows[i][0]),
                        InvoiceDate = ds.Tables[0].Rows[i][1].ToString(),
                        TotalCost = Convert.ToInt32(ds.Tables[0].Rows[i][2])
                    };
                    lstInvoices.Add(Invoice);
                }
                return lstInvoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        #endregion
    }

}
