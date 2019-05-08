using Assignment6AirlineReservation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace CS3280_Group1_Invoice.Items
{
    /// <summary>
    /// Interaction logic for wndItems.xaml
    /// </summary>
    public partial class wndItems : Window
    {
        /// <summary>
        /// A logice item used to bridge the UI with business logic
        /// </summary>
        clsItemsLogic logic = new clsItemsLogic();
        /// <summary>
        /// Used to bridge queries and UI
        /// </summary>
        clsItemsSQL iSql = new clsItemsSQL();
        /// <summary>
        /// A global Datatable, used to display items
        /// </summary>
        DataTable dt = new DataTable();
        /// <summary>
        /// A global dataset, usually passed to the UI
        /// </summary>
        DataSet dataSet = new DataSet();
        /// <summary>
        /// An item that keeps track of which item is currently selected
        /// </summary>
        clsItem SelectedItem = new clsItem();
        
        /// <summary>
        /// Initializes the window
        /// </summary>
        public wndItems()
        {

            try
            {
                InitializeComponent();
                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        /// <summary>
        /// An action listener on the delete button, calls corresponding business logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDelete(object sender, RoutedEventArgs e)
        {
            try {
                if (ItemCodeTxt.Text != "")
                {
                    Console.WriteLine("Item code not empty");
                    if (logic.DeleteItem(ItemCodeTxt.Text, ItemDescriptionTxt.Text))
                    {
                        NoticeLbl.Visibility = Visibility.Visible;
                        NoticeLbl.Content = $"{ItemDescriptionTxt.Text} Succesfully deleted!";
                    }
                    else
                    {
                        NoticeLbl.Visibility = Visibility.Visible;
                        NoticeLbl.Content = $"{ItemDescriptionTxt.Text} Could not be deleted";
                    }
                    RefreshDataGrid();
                    ItemCodeTxt.Text = "";
                    ItemDescriptionTxt.Text = "";
                    ItemCostTxt.Text = "";
                    NoticeLbl.Visibility = Visibility.Visible;
                } else
                {
                    NoticeLbl.Content = "No Item Selected";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// An action listener for the edit button, calls corresponding business logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_EditItem(object sender, RoutedEventArgs e)
        {
            try { 
            if (ItemCodeTxt.Text != "")
            {
                    NoticeLbl.Visibility = Visibility.Hidden;

                    cmdEditItem.Visibility = Visibility.Hidden;
                SaveChanges.Visibility = Visibility.Visible;
                CancelChanges.Visibility = Visibility.Visible;
                DisableUI();
            }
            else
            {
                Console.WriteLine("Select Item To Edit");
            }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// An action listener for the Add Item button, calls corresponding business logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_AddItem(object sender, RoutedEventArgs e)
        {
            try {
            NoticeLbl.Visibility = Visibility.Hidden;

            cmdEditItem.Visibility = Visibility.Hidden;
            SaveChanges.Visibility = Visibility.Visible;
            CancelChanges.Visibility = Visibility.Visible;
            ItemCodeTxt.Text = logic.NextItemCode();
            ItemDescriptionTxt.Text = "";
            ItemCostTxt.Text = "";
            DisableUI();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// An action listener for the additem button, calls corresponding business logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_SaveChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                NoticeLbl.Visibility = Visibility.Hidden;
                if (!logic.GetAllItemCodes().Contains(ItemCodeTxt.Text))
                {
                    logic.AddItem(ItemCodeTxt.Text, ItemDescriptionTxt.Text, ItemCostTxt.Text);
                    RefreshDataGrid();
                    cmdEditItem.Visibility = Visibility.Visible;
                    SaveChanges.Visibility = Visibility.Hidden;
                    CancelChanges.Visibility = Visibility.Hidden;
                    NoticeLbl.Visibility = Visibility.Visible;
                    NoticeLbl.Content =  ItemDescriptionTxt.Text + " Succesfully Added!";
                    EnableUI();
                }
                else
                {
                    logic.ChangedItem(ItemCodeTxt.Text, ItemDescriptionTxt.Text, ItemCostTxt.Text, SelectedItem, NoticeLbl);
                    RefreshDataGrid();
                    cmdEditItem.Visibility = Visibility.Visible;
                    SaveChanges.Visibility = Visibility.Hidden;
                    CancelChanges.Visibility = Visibility.Hidden;
                    NoticeLbl.Visibility = Visibility.Visible;
                    NoticeLbl.Content =  ItemDescriptionTxt.Text + " Succesfully Changed!";
                    EnableUI();
                }
                ItemCodeTxt.Text = "";
                ItemDescriptionTxt.Text = "";
                ItemCostTxt.Text = "";
                RefreshDataGrid();
                EnableUI();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);                
            }
        }
        /// <summary>
        /// An action listener on the cancel button, calls corresponding business logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_CancelChanges(object sender, RoutedEventArgs e)
        {
            try {
                NoticeLbl.Visibility = Visibility.Hidden;

                EnableUI();
            cmdEditItem.Visibility = Visibility.Visible;
            SaveChanges.Visibility = Visibility.Hidden;
            CancelChanges.Visibility = Visibility.Hidden;
                NoticeLbl.Visibility = Visibility.Visible;
                NoticeLbl.Content = "Changes Canceled";
                ItemCodeTxt.Text = "";
            ItemCostTxt.Text = "";
            ItemDescriptionTxt.Text = "";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Responsible for populating and refreshing the datagrid
        /// </summary>
        private void RefreshDataGrid()
        {
            try { 
            ItemList.ItemsSource = iSql.PopulateItemList();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// An action listener for each item in the datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cell_Clicked(object sender, RoutedEventArgs e)
        {
            try {
                NoticeLbl.Visibility = Visibility.Hidden;

                if (ItemList.SelectedValue != null && ItemList.SelectedValue.ToString() != "{NewItemPlaceholder}")
            {
                clsItem item = (clsItem)ItemList.SelectedValue;
                ItemCodeTxt.Text = item.itemCode;
                ItemDescriptionTxt.Text = item.description;
                ItemCostTxt.Text = item.cost.ToString();
                SelectedItem = item;
            }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

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
        /// <summary>
        /// Helper method to disable UI controls
        /// </summary>
        private void DisableUI() 
        {     
            try { 
            ItemList.IsEnabled = false;                                
            AddItem.IsEnabled = false;                                
            DeleteItem.IsEnabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }                                                              
        /// <summary>
        /// Helper method to enable UI Controls
        /// </summary>
        private void EnableUI()
        {      
            try { 
            ItemList.IsEnabled = true;                             
            AddItem.IsEnabled = true;                            
            DeleteItem.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
