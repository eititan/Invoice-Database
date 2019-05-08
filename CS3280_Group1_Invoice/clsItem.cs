using System;
using System.Collections.Generic;
using System.Linq;
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

        public override string ToString()
        {
            return description + " - $" + cost;
        }

    }
}
