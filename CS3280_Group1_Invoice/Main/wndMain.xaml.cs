using CS3280_Group1_Invoice.Items;
using CS3280_Group1_Invoice.Main;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CS3280_Group1_Invoice
{
    /// <summary>
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        #region Attributes
        /// <summary>
        /// instantiating and creating a new datebase object
        /// </summary>
        private clsMainSQL database = new clsMainSQL();
        /// <summary>
        /// declaring a wndSearch object
        /// </summary>
        private wndSearch searchWin;
        /// <summary>
        /// declaring a wndItems object
        /// </summary>
        private wndItems itemWin;
        /// <summary>
        /// declares and instantiates a clsMainLogic class that we use for the dates
        /// </summary>
        private clsMainLogic mLogic = new clsMainLogic();
        /// <summary>
        /// declares an observablecollection with type clsItem that is used to bind to drop down combo box
        /// </summary>
        private ObservableCollection<clsItem> itemList;
        /// <summary>
        /// declares an observablecollection with type clsItem that is used to bind to data grid. Holds the invoice items
        /// </summary>
        private ObservableCollection<clsItem> customerItems;
        /// <summary>
        /// A boolean that is toggles whether the user is in editing mode or not
        /// </summary>
        private bool isEditingInv;
        /// <summary>
        /// A boolean that is toggles whether the user is in creating mode or not
        /// </summary>
        private bool isCreatingInv;
        /// <summary>
        /// Aglobal cost varible that hold the total cost in an invoice
        /// </summary>
        private int totalCost;
        #endregion

        #region c-tor
        /// <summary>
        /// c-tor that gets our cb list, enters display mode and sets our editing and creating varible to false
        /// </summary>
        public wndMain()
        {
            try
            {


                InitializeComponent();

                RefreshCBList();
                DisplayMode();

                lblError.Content = "";
                btnEditInvoice.IsEnabled = false;
                btnDeleteInvoice.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        #region Right Panel Buttons
        /* ------------------------------------------------------------ Far Right Panel buttons (top to bottom) ------------------------------------------------------*/

        /// <summary>
        /// enters editing mode when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditingMode();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        
        }

        /// <summary>
        /// deletes a given invoice if after being prompted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int invoiceNum;
                string temp = txtInvoice.Text;

                //parse string to int 
                Int32.TryParse(temp, out invoiceNum);

                //alert box that warns user the invoice cannot be recovered
                if (MessageBox.Show("Are you sure you REALLY want to delete this invoice, it cannot be recovered?", "Alert!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //deletes the invoice from the database
                    database.DeleteInvoice(invoiceNum);

                    totalCost = 0;
                    dgItems.ItemsSource = null;
                    txtInvoice.Text = "";
                    lblDate.Text = "";
                    txtItemCost.Text = "$0";
                    txtTotalCost.Text = "$" + totalCost;

                    DisplayMode();
                    btnEditInvoice.IsEnabled = false;
                    btnDeleteInvoice.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
      
        /// <summary>
        /// enteres creating mode and sets some labels/textboxes upon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreateInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //setting all of the content for create invoice
                txtInvoice.Text = "TBD";
                lblDate.Text = "";
                txtItemCost.Text = "$0";
                txtTotalCost.Text = "$" + totalCost;
                customerItems = new ObservableCollection<clsItem>();

                RefreshCBList();
                CreatingMode();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
           
        }
        #endregion Right Panel Buttons

        #region Panel buttons
        /* ------------------------------------------------------------ Editing panel buttons (left to right) ------------------------------------------------------*/

        /// <summary>
        /// adds the selected item from a drop down box to the customer list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbItemList.SelectedItem != null)
                {
                    int activeIndex = cbItemList.SelectedIndex;
                    clsItem item = itemList.ElementAt(activeIndex);

                    dgItems.ItemsSource = customerItems;
                    totalCost += item.cost;
                    txtTotalCost.Text = "$" + totalCost;

                    customerItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            

        }

        /// <summary>
        /// removes the selected item in the datagrid from the datagrid upon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsItem item = (clsItem)dgItems.SelectedItem;

                //checks if removed item is an actual item
                if (customerItems.Remove(item))
                {
                    totalCost -= item.cost;
                    txtTotalCost.Text = "$" + totalCost;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            

        }

        /// <summary>
        /// saves all the cost, items in data grid, and potentially new data on click.
        /// Enters display mode if the user succesfully saved the invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //querys the database to add to invoice
                //then add all of the items added to the datagrid to LineItems table.
                if (String.IsNullOrEmpty(lblDate.Text))
                {
                    lblError.Content = "Must enter a date for the invoice.";

                }
                else if (totalCost == 0)
                {
                    lblError.Content = "Please add items to this invoice.";

                }else if (isEditingInv)
                {
                    //get invoice number from our handy text box and parses it into an int
                    string temp = txtInvoice.Text;
                    int invoiceNum = 0;
                    Int32.TryParse(temp, out invoiceNum);

                    string date = mLogic.SanitizeValidateDate(lblDate.Text);

                    if(date == "error")
                    {
                        lblError.Content = "Please enter a valid date: MM/DD/YYYY";
                        return;
                    }

                    database.UpdateEditedInvoices(customerItems, invoiceNum);
                    database.UpdateInvoiceInfo(invoiceNum, date, totalCost);

                    DisplayMode();

                }
                else
                {
                    string date = mLogic.SanitizeValidateDate(lblDate.Text);
                    if (date == "error")
                    {
                        lblError.Content = "Please enter a valid date: MM/DD/YYYY";
                        return;
                    }

                    database.InsertIntoInvoices(date, totalCost);

                    //get invoice number and updates the "TBD" to the actual invoice number
                    int invNum = database.GetInvoiceNumber(date, totalCost);
                    txtInvoice.Text = "" + invNum;

                    database.InsertIntoLineItems(invNum, customerItems);
                    DisplayMode();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion

        #region Menu clicks
        /* ------------------------------------------------------------ Menu button clicks ------------------------------------------------------*/
        /// <summary>
        /// This opens the panel Chris West implented and if succesfffuly selected an invoice, will populate the information in the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //a cool way make sure the user doesn't search for an invoice when trying to create a new one
                //dont know if we should implement as he tends to dock points for adding not-required features
                if (isCreatingInv)
                {
                    if (MessageBox.Show("Your new invoice is not saved and will be discarded if you leave, continue?", "Alert!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        //clears old info
                        dgItems.ItemsSource = null;
                        txtInvoice.Text = "";
                        totalCost = 0;
                        txtTotalCost.Text = "$" + totalCost;
                        lblDate.Text = "";

                        DisplayMode();
                        //Opens up the window Chris West is working on and returns an invoice num

                    }
                    else
                    {
                        return;
                    }

                }

                searchWin = new wndSearch();
                searchWin.ShowDialog();

                int invoiceNum = searchWin.selectedInvoiceNumber;
                PopulateUI(invoiceNum);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Open the window Chris Narvarro implemented if the user is not in creating/editing mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string invoiceNum = txtInvoice.Text;
                int invNum = 0;
                Int32.TryParse(invoiceNum, out invNum);

                if (!isEditingInv && !isCreatingInv)
                {
                    itemWin = new wndItems();
                    itemWin.ShowDialog();
                    PopulateUI(invNum);

                    //refreshes the list of items if the user added or deleted some
                    RefreshCBList();
                    DisplayMode();
                }
                else
                {
                    lblError.Content = "You are currently creating an invoice, \nsave before you try to edit your items.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }


        }
        #endregion

        #region ComboBoxes
        /* ------------------------------------------------------------ ComboBox ------------------------------------------------------*/

        /// <summary>
        /// CAlled when the user selects a new item in the drop down combo box and updates the price in the lblCost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int activeIndex = cbItemList.SelectedIndex;

                //if there is a valid selected index --> -1 is no selected item
                if (cbItemList.SelectedItem != null)
                {
                    clsItem item = itemList.ElementAt(activeIndex);
                    txtItemCost.Text = "$" + item.cost;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            
           
        }

        #endregion

        #region Helper-Methods
        /* ------------------------------------------------------------ helper methods ------------------------------------------------------*/

        /// <summary>
        /// Gets an updated list of the items to be displayed
        /// </summary>
        private void RefreshCBList()
        {
            try
            {
                //populate cbItemList with all items in db every time we click create to refresh item list
                database.PopulateItemList();
                itemList = database.GetItemList();
                cbItemList.ItemsSource = itemList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// enteres display mode when a user hits any save button. Sets creating and editing invoices to false.
        /// </summary>
        private void DisplayMode()
        {
            try
            {
                btnAddItem.IsEnabled = false;
                btnDeleteItem.IsEnabled = false;
                btnSaveInvoice.IsEnabled = false;
                cbItemList.IsEnabled = false;

                btnCreateInvoice.IsEnabled = true;
                btnEditInvoice.IsEnabled = true;
                btnDeleteInvoice.IsEnabled = true;

                cbItemList.SelectedItem = null;
                lblDate.IsReadOnly = true;

                isCreatingInv = false;
                isEditingInv = false;

                txtItemCost.Text = "$0";
                lblError.Content = "";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// enteres editing mode by setting the proper labels to be editing and clicked on. Sets isEditing to true and isCreating to false.
        /// </summary>
        private void EditingMode()
        {
            try
            {
                lblDate.IsReadOnly = false;

                btnAddItem.IsEnabled = true;
                btnDeleteItem.IsEnabled = true;
                btnSaveInvoice.IsEnabled = true;

                cbItemList.IsEnabled = true;

                isEditingInv = true;
                isCreatingInv = false;

                btnEditInvoice.IsEnabled = false;
                btnCreateInvoice.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
          
        }

        /// <summary>
        /// enteres creating mode when called and sets the appropriate labels and buttons to be clicked. Sets isEditing to false and isCreating to true.
        /// </summary>
        private void CreatingMode()
        {
            try
            {
                totalCost = 0;
                txtTotalCost.Text = "$" + totalCost;

                dgItems.ItemsSource = null;
                lblDate.IsReadOnly = false;

                btnAddItem.IsEnabled = true;
                btnDeleteItem.IsEnabled = true;
                btnSaveInvoice.IsEnabled = true;

                cbItemList.IsEnabled = true;

                isCreatingInv = true;
                isEditingInv = false;

                btnCreateInvoice.IsEnabled = false;
                btnEditInvoice.IsEnabled = false;
                btnDeleteInvoice.IsEnabled = false;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            
        }

        /// <summary>
        /// Refreshes the UI with the invoice information from invoice number
        /// </summary>
        private void PopulateUI(int invoiceNum)
        {
            //if we actually recieved an voice of 5000 or higher
            if (invoiceNum != 0)
            {
                totalCost = 0;
                txtInvoice.Text = "" + invoiceNum;

                customerItems = database.GetInvoiceItems(invoiceNum);
                dgItems.ItemsSource = customerItems;

                btnEditInvoice.IsEnabled = true;
                btnDeleteInvoice.IsEnabled = true;


                /* populate all of the fields in my main panel with invoice information */
                foreach (var item in customerItems)
                {
                    totalCost += item.cost;
                }

                txtTotalCost.Text = "$" + totalCost;
                string date = database.GetInvoiceDate(invoiceNum);

                lblDate.Text = mLogic.GetDateFromString(date);
                DisplayMode();
            }
        }
        #endregion
    }
}
