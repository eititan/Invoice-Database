using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CS3280_Group1_Invoice
{
    /// <summary>
    /// hold our items as a 
    /// </summary>
    class clsItem
    {
        public string description { get; set; }
        public string itemCode { get; set; }
        public int cost { get; set; }

        /// <summary>
        /// Overwritten ToSting that displays the object in the following format: description - $ cost
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return description + " - $" + cost;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

    }
}
