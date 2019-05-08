using CS3280_Group1_Invoice.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;

namespace CS3280_Group1_Invoice
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        #region Attributes
        /// <summary>
        /// Initializing the Search WIndow logic
        /// </summary>
        private clsSearchLogic searchLogic;

        /// <summary>
        /// Initializing the Invoice Class
        /// </summary>
        private clsInvoice invoice;

        /// <summary>
        /// Initializes a collection that houses all of the invoices returned from the database.
        /// </summary>
        private ObservableCollection<clsInvoice> invoices;

        /// <summary>
        /// Initializes a list of invoice numbers that is used to populate the Select Invoice Combo Box
        /// </summary>
        private List<int> invoiceNumbers;

        /// <summary>
        /// Initializes a list of invoice dates that is used to populate the Select Date Combo Box
        /// </summary>
        private List<String> invoiceDates;

        /// <summary>
        /// Initializes a list of invoice charges that is used to populate the Select Charge Combo Box
        /// </summary>
        private List<int> invoiceCharges;

        /// <summary>
        /// Stores the selected invoice number used to filter the displayed invoices
        /// </summary>
        private int invoiceNumberFilter = -1;

        /// <summary>
        /// Stores the selected invoice date used to filter the displayed invoices
        /// </summary>
        private string invoiceDateFilter = null;

        /// <summary>
        /// Stores the selected invoice charge used to filter the displayed invoices
        /// </summary>
        private int invoiceChargeFilter = -1;

        /// <summary>
        /// Stores the selected invoice number that the user is wanting to open in the main window
        /// </summary>
        public int selectedInvoiceNumber;
        #endregion

        #region Methods
        public wndSearch()
        {
            try
            {
                InitializeComponent();
                searchLogic = new clsSearchLogic();
                lblErrorMessage.Content = "";
            }
            catch (Exception ex)
            {
                //this is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        #region Event Handlers
        /// <summary>
        /// Event that populates the DataGrid with all invoices upon the loading of the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                InitalizeObjectsAtStartUp();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Event that apples the filtering of the displayed invoices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmitFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (invoiceChargeFilter < 0 && invoiceDateFilter == null)
                {
                    //Populate the DataGrid per selected invoice number
                    invoice = new clsInvoice();
                    invoices = searchLogic.GetInvoicesPerNumber(Convert.ToInt32(
                        cmbInvoiceNums.SelectedValue));
                    dgInvoiceContents.ItemsSource = invoices;
                    return;
                }

                if (invoiceNumberFilter < 0 && invoiceDateFilter == null)
                {
                    //Populate the DataGrid per selected charge amount
                    invoice = new clsInvoice();
                    invoices = searchLogic.GetInvoicesPerTotal(Convert.ToInt32(
                        cmbInvoiceCharges.SelectedValue));
                    dgInvoiceContents.ItemsSource = invoices;
                    return;
                }

                if (invoiceNumberFilter < 0 && invoiceChargeFilter < 0)
                {
                    //Populate the DataGrid per selected charge amount
                    invoice = new clsInvoice();
                    invoices = searchLogic.GetInvoicePerDate(cmbInvoiceDates.SelectedValue.ToString());
                    dgInvoiceContents.ItemsSource = invoices;
                    return;
                }

                if (invoiceNumberFilter < 0)
                {
                    //Populate the DataGrid per selected charge amount and date
                    invoice = new clsInvoice();
                    invoices = searchLogic.GetInvoicesPerDateTotal(cmbInvoiceDates.SelectedValue.ToString(),
                        Convert.ToInt32(cmbInvoiceCharges.SelectedValue));
                    dgInvoiceContents.ItemsSource = invoices;
                    return;
                }

                if (invoiceChargeFilter < 0)
                {
                    //Populate the DataGrid per selected invoice number and date
                    invoice = new clsInvoice();
                    invoices = searchLogic.GetInvoicesPerNumberDate(cmbInvoiceDates.SelectedValue.ToString(),
                        Convert.ToInt32(cmbInvoiceNums.SelectedValue));
                    dgInvoiceContents.ItemsSource = invoices;
                    return;
                }

                if (invoiceDateFilter != null && invoiceNumberFilter >= 0 && invoiceChargeFilter >= 0)
                {
                    //Populate the DataGrid per selected invoice number and Total
                    invoice = new clsInvoice();
                    invoices = searchLogic.GetInvoicesAllFilterOptions(cmbInvoiceDates.SelectedValue.ToString(),
                        Convert.ToInt32(cmbInvoiceCharges.SelectedValue),
                        Convert.ToInt32(cmbInvoiceNums.SelectedValue));
                    dgInvoiceContents.ItemsSource = invoices;
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Event that clears all of the input filter of the invoices and reverts back to initial state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearFields_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PopulateDefaultDataGrid();
                ResetFilterOptions();
                cmbInvoiceNums.Text = "";
                cmbInvoiceCharges.Text = "";
                cmbInvoiceDates.Text = "";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Event that will pass the selected Invoice back to the main window. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelectInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgInvoiceContents.SelectedItem != null)
                {
                    clsInvoice invoice = (clsInvoice)dgInvoiceContents.SelectedItem;
                    selectedInvoiceNumber = invoice.InvoiceNumber;
                    this.Close();
                }
                else
                {
                    lblErrorMessage.Content = "User didn't select an invoice.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Event Handler that sets the selected index number for the Invoice Number Combo Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbInvoiceNums_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                invoiceNumberFilter = Convert.ToInt32(cmbInvoiceNums.SelectedIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// Event Handler that sets the selected index number for the Invoice Date Combo Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbInvoiceDates_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                invoiceDateFilter = cmbInvoiceDates.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Event Handler that sets the selected index number for the Invoice Total Combo Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbInvoiceCharges_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                invoiceChargeFilter = Convert.ToInt32(cmbInvoiceCharges.SelectedIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Helper method that is called to initialize the Data Grid and all of the combo boxes in the form
        /// </summary>
        private void InitalizeObjectsAtStartUp()
        {
            try
            {
                PopulateDefaultDataGrid();
                PopulateComboBoxes();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Helper method that populates all the filter combo boxes
        /// </summary>
        private void PopulateComboBoxes()
        {
            try
            {

                //Populate the Invoice Cbo
                invoiceNumbers = new List<int>();
                invoiceNumbers = searchLogic.GetInvoiceNumbers();
                cmbInvoiceNums.ItemsSource = invoiceNumbers;
                //Populate the Date Cbo
                invoiceDates = new List<string>();
                invoiceDates = searchLogic.GetInvoiceDates();
                cmbInvoiceDates.ItemsSource = invoiceDates;
                //Populate the Charges Cbo
                invoiceCharges = new List<int>();
                invoiceCharges = searchLogic.GetInvoiceTotals();
                cmbInvoiceCharges.ItemsSource = invoiceCharges;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Helper method that populates the DataGrid with all invoices.
        /// </summary>
        private void PopulateDefaultDataGrid()
        {
            try
            {
                invoice = new clsInvoice();
                invoices = searchLogic.GetAllInvoices();
                dgInvoiceContents.ItemsSource = invoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Helper method that resets the filter option. Primarily used when the Clear Button is clicked.
        /// </summary>
        private void ResetFilterOptions()
        {
            try
            {
                invoiceDateFilter = null;
                invoiceChargeFilter = -1;
                invoiceNumberFilter = -1;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        #region Error Handle Method(s)
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
        #endregion

        #endregion
    }
}