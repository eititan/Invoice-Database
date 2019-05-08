
using System;
using System.Reflection;

namespace CS3280_Group1_Invoice.Search
{
    class clsInvoice
    {
        #region Attributes
        /// <summary>
        /// Represents the invoice number as an int
        /// </summary>
        public int InvoiceNumber { get; set; }

        /// <summary>
        /// Represents the invoice date as a string
        /// </summary>
        public string InvoiceDate { get; set; }

        /// <summary>
        /// Represents the Invoice Total as a int
        /// </summary>
        public int TotalCost { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Overridden ToString method that returns a formated string of: InvoiceNumber InvoiceDate
        /// TotalCost
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {

                return string.Format("InvoiceNumber {0} InvoiceDate {1} TotalCost {2}",
                    InvoiceNumber, InvoiceDate, TotalCost);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        #endregion
    }
}
