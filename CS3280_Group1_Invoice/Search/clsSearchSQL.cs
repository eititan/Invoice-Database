using System;
using System.Reflection;

namespace CS3280_Group1_Invoice.Search
{
    /// <summary>
    /// SQL Query Statements
    /// </summary>
    class clsSearchSQL
    {
        #region Attributes
        /// <summary>
        /// String representation of the query
        /// </summary>
        private string sql;
        #endregion

        #region Methods
        /// <summary>
        /// Query to gather a list all invoices.
        /// </summary>
        /// <returns></returns>
        public string GetInvoices()
        {
            try
            {
                sql = "SELECT Invoices.InvoiceNum, Invoices.InvoiceDate, " +
                    "Invoices.TotalCost FROM Invoices ";

                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list all invoices of the passed in Invoice Number.
        /// </summary>
        /// <param name="InvoiceNum"> filtering Invoice Number</param>
        /// <returns> the query string of the requested invoice number</returns>
        public string GetInvoicesPerNumber(int InvoiceNum)
        {
            try
            {
                sql = "SELECT Invoices.InvoiceNum, Invoices.InvoiceDate, " +
                    "Invoices.TotalCost FROM Invoices " +
                    "WHERE Invoices.InvoiceNum = " + InvoiceNum;

                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list all invoices of the passed in Invoice Date.
        /// </summary>
        /// <param name="InvoiceDate"> filtering Invoice Date</param>
        /// <returns> the query string of the requested invoice Date</returns>
        public string GetInvoicesPerDate(string InvoiceDate)
        {
            try
            {
                sql = "SELECT Invoices.InvoiceNum, Invoices.InvoiceDate, " +
                    "Invoices.TotalCost FROM Invoices " +
                    "WHERE Invoices.InvoiceDate = #" + InvoiceDate + "#";

                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list all invoices of the passed in Invoice Total.
        /// </summary>
        /// <param name="InvoiceTotal"> filtering Invoice Total</param>
        /// <returns> the query string of the requested invoice Total</returns>
        public string GetInvoicesPerTotal(int InvoiceTotal)
        {
            try
            {
                sql = "SELECT Invoices.InvoiceNum, Invoices.InvoiceDate, " +
                    "Invoices.TotalCost FROM Invoices " +
                    "WHERE Invoices.TotalCost = " + InvoiceTotal;

                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list all invoices of the passed in Invoice Number and Date.
        /// </summary>
        /// <param name="InvoiceNum">Filtering Invoice Number</param>
        /// <param name="InvoiceDate">Filtering Invoice Date</param>
        /// <returns> the query string of the requested invoice Number and Date</returns>
        public string GetInvoicesPerNumberDate(int InvoiceNum, string InvoiceDate)
        {
            try
            {
                sql = "SELECT Invoices.InvoiceNum, Invoices.InvoiceDate, " +
                    "Invoices.TotalCost FROM Invoices " +
                    "WHERE Invoices.InvoiceNum = " + InvoiceNum +
                    "and Invoices.InvoiceDate = #" + InvoiceDate + "#";

                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list all invoices of the passed in Invoice Number and Total.
        /// </summary>
        /// <param name="InvoiceNum">Filtering Invoice Number</param>
        /// <param name="InvoiceTotal">Filtering Invoice Total</param>
        /// <returns> the query string of the requested invoice Number and Total</returns>
        public string GetInvoicesPerNumberTotal(int InvoiceNum, int InvoiceTotal)
        {
            try
            {
                sql = "SELECT Invoices.InvoiceNum, Invoices.InvoiceDate, " +
                    "Invoices.TotalCost FROM Invoices " +
                    "WHERE Invoices.InvoiceNum = " +
                    InvoiceNum + " and Invoices.TotalCost = " + InvoiceTotal;
                
                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list all invoices of the passed in Invoice Date and Total.
        /// </summary>
        /// <param name="InvoiceDate">Filtering Invoice Date</param>
        /// <param name="InvoiceTotal">Filtering Invoice Total</param>
        /// <returns> the query string of the requested invoice Date and Total</returns>
        public string GetInvoicesPerDateTotal(string InvoiceDate, int InvoiceTotal)
        {
            try
            {
                sql = "SELECT Invoices.InvoiceNum, Invoices.InvoiceDate, " +
                    "Invoices.TotalCost FROM Invoices " +
                    "WHERE Invoices.InvoiceDate = #" + InvoiceDate + "#" +
                    "and Invoices.TotalCost = " + InvoiceTotal;

                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list all Invoice Totals.
        /// </summary>
        /// <returns> the query string of the requested Invoice Totals</returns>
        public string GetInvoiceTotals()
        {
            try
            {
                sql = "SELECT DISTINCT Invoices.TotalCost FROM Invoices " +
                    "ORDER BY Invoices.TotalCost ASC";

                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list of all Invoice Dates.
        /// </summary>
        /// <returns> the query string of the requested Invoice Dates</returns>
        public string GetInvoiceDates()
        {
            try
            {
                sql = "SELECT DISTINCT Invoices.InvoiceDate FROM Invoices";
                
                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list of all Invoice Numbers.
        /// </summary>
        /// <returns> the query string of the requested Invoice Numbers</returns>
        public string GetInvoiceNumbers()
        {
            try
            {
                sql = "SELECT Invoices.InvoiceNum FROM Invoices " +
                    "ORDER BY Invoices.InvoiceNum ASC";

                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Query to gather a list all invoices of the passed in Invoice Date, Invoice Number, and Total.
        /// </summary>
        /// <param name="InvoiceDate">Filtering Invoice Date</param>
        /// <param name="InvoiceTotal">Filtering Invoice Total</param>
        /// <param name="InvoiceNumber">Filtering Invoice Number</param>
        /// <returns> the query string of the requested invoice Date, Invoice Number, and Total</returns>
        public string GetInvoicesAllFilterOptions(string InvoiceDate, 
            int InvoiceTotal, int InvoiceNumber)
        {
            try
            {
                sql = "SELECT Invoices.InvoiceNum, Invoices.InvoiceDate, " +
                    "Invoices.TotalCost FROM Invoices " +
                    "WHERE Invoices.InvoiceDate = #" + InvoiceDate + "#" +
                    " and Invoices.TotalCost = " + InvoiceTotal +
                    " and Invoices.InvoiceNum = " + InvoiceNumber;

                return sql;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "."
                    + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion
    }
}
