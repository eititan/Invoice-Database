using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;

namespace CS3280_Group1_Invoice.Items
{
    class clsItemsLogic
    {
        /// <summary>
        /// Initiates a query variable to perform query logic.
        /// </summary>
        clsItemsSQL query = new clsItemsSQL();

        /// <summary>
        /// Gathers Item cod and description to validate whether the item has been used in an invoice and deletes it if not
        /// </summary>
        /// <param name="code"></param>
        /// <param name="desc"></param>
        public bool DeleteItem(string code, string desc)
        {

            List<string> ItemCodes = GetUsedItemCodes();
            if (ItemCodes.Contains(code))
            {
                return false;
            } else
            {
                var result = MessageBox.Show($"Delete the {desc}?", "Delete Item", MessageBoxButton.YesNo);
                if(result.ToString() == "Yes")
                {
                    query.DeleteItem(code);
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// Gathers necessary variables to change an item in the database
        /// </summary>
        /// <param name="itemcode"></param>
        /// <param name="desc"></param>
        /// <param name="cost"></param>
        /// <param name="Item"></param>
        public void ChangedItem(string itemcode, string desc, string cost, clsItem Item, System.Windows.Controls.Label notice)
        {
            try { 
            string Changes = "";
            if (GetAllItemCodes().Contains(itemcode))
            {
                if (desc != Item.description || cost != Item.cost.ToString())
                {
                    Console.WriteLine("Old Item");
                    if (desc != Item.description)
                    {
                        Changes += $@"Description: {Item.description} being changed to {desc}";
                        Changes += "\n";
                    }
                    if (cost != Item.cost.ToString())
                    {
                        Changes += $@"Cost: {Item.cost} adjusted to {cost}";
                        Changes += "\n";
                    }
                    Changes += "\nSave Changes?";
                    var Result = MessageBox.Show(Changes, "Save Changes", MessageBoxButton.YesNoCancel);
                    if(Result.ToString() == "Yes")
                    {
                        query.SaveChanges(itemcode, desc, cost);
                    }
                    else
                    {
                            notice.Content = "Changes not saved";
                    }                    
                }
                else
                {
                    notice.Content = "No Changes To Save";
                }
            }
            else
            {
                AddItem(itemcode, desc, cost);
            }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gathers varibales necessary to add item to the database
        /// </summary>
        /// <param name="code"></param>
        /// <param name="desc"></param>
        /// <param name="cost"></param>
        public void AddItem(string code, string desc, string cost)
        {
            try { 
            if (!GetAllItemCodes().Contains(code))
            { 
                if (desc != "" && CheckIfNumeric(cost))
                {
                    query.InsertItem(code, desc, cost);
                }
                else
                {
                    MessageBox.Show("Description Cannot be Empty and Cost must be numeric");
                }
            }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Validates that the cost input is numeric
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool CheckIfNumeric(string cost)
        {
            try { 
            return int.TryParse(cost, out int price);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets the item codes used in invoices
        /// </summary>
        /// <returns></returns>
        public List<string> GetUsedItemCodes()
        {
            try { 
            DataSet LineItemCodes = query.GetLineItemCodes();
            List<string> ItemCodes = new List<string>();
            for (int i = 0; i < query.index; i++)
            {
                ItemCodes.Add(LineItemCodes.Tables[0].Rows[i][0].ToString());
            }
            return ItemCodes;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets all the itemcodes from the database
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllItemCodes()
        {
            try { 
            DataSet ItemCodes = query.GetAllItemCodes();
            List<string> ItemCode = new List<string>();
            for (int i = 0; i < query.index; i++)
            {
                    if (ItemCodes.Tables[0].Rows[i][0].ToString().Length == 1)
                    {
                        ItemCode.Add(ItemCodes.Tables[0].Rows[i][0].ToString());
                    }
            }
                for (int i = 0; i < query.index; i++)
                {
                    if (ItemCodes.Tables[0].Rows[i][0].ToString().Length > 1)
                    {
                        ItemCode.Add(ItemCodes.Tables[0].Rows[i][0].ToString());
                    }
                }

                return ItemCode;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Responsible for assigning a new item code without passing a viarable
        /// </summary>
        /// <returns></returns>
        public string NextItemCode()
        {
            try {
                List<string> UsedCodes = GetAllItemCodes();
                string LastCode = UsedCodes.ElementAt((UsedCodes.Count - 1));
                string NextCode = GenerateNextItemCode(LastCode);
                return NextCode;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Generates the new item code based on the last item itemcode in the database
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GenerateNextItemCode(string col)
        {
            try { 
            if (col == "") return "A";
            string fPart = col.Substring(0, col.Length - 1);
            char lChar = col[col.Length - 1];
            if (lChar == 'Z') return GenerateNextItemCode(fPart) + "A";
            return fPart + ++lChar;
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

    }
}
