using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CS3280_Group1_Invoice.Main
{
    
    /// <summary>
    /// THis class hold all of the business logic for the main window
    /// </summary>
    class clsMainLogic
    {
        /// <summary>
        /// c-tor
        /// </summary>
        public clsMainLogic()
        {

        }

        /// <summary>
        /// Trims our incoming date as well as validates it and returns the proper string
        /// </summary>
        /// <param name="lblDate"></param>
        /// <returns>a string represenation of a date or "error" which is used to output an error message through a label</returns>
        public string SanitizeValidateDate(string lblDate)
        {
            //sanitizes string before we add it to database
            string date = lblDate.Trim();
            //usd to tryparse our date and if it doesn't work, our entered date is wrong.
            DateTime Test;

            //is 9/7/2020 case
            if (date.Substring(0, 1) != "0" && date.Length == 8)
            {
                date = date.Insert(0, "0");
                date = date.Insert(3, "0");

                if (DateTime.TryParseExact(date, "MM/dd/yyyy", null, DateTimeStyles.None, out Test) == true)
                {
                    return date;
                }
                else
                {
                    return "error";
                }
            }
            //is 09/7/2019 case
            else if (date.Substring(0, 1) == "0" && date.Length == 9)
            {
                date = date.Insert(3, "0");

                if (DateTime.TryParseExact(date, "MM/dd/yyyy", null, DateTimeStyles.None, out Test) == true)
                {
                    return date;
                }
                else
                {
                    return "error";
                }
            }
            //is 12/7/2019 case
            else if (date.Substring(0, 1) == "1" && date.Length == 9)
            {
                date = date.Insert(3, "0");

                if (DateTime.TryParseExact(date, "MM/dd/yyyy", null, DateTimeStyles.None, out Test) == true)
                {
                    return date;
                }
                else
                {
                    return "error";
                }
            }
            //is 9/17/2019 case
            else if (date.Substring(0, 1) != "0" && date.Length != 10)
            {
                date = date.Insert(0, "0");

                if (DateTime.TryParseExact(date, "MM/dd/yyyy", null, DateTimeStyles.None, out Test) == true)
                {
                    return date;
                }
                else
                {
                    return "error";
                }

            }
            //is 09/17/2019 case
            else
            {

                if (DateTime.TryParseExact(date, "MM/dd/yyyy", null, DateTimeStyles.None, out Test) == true)
                {
                    return date;
                }
                else
                {
                    return "error";
                }
            }

        }

        /// <summary>
        /// This method parses our date from our database to only show the date in MM/dd/yyyy and takes of the hour and minutes portion
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public string GetDateFromString(string inputText)
        {
            string date = "";
            
            //if our date is 2/2/2010
            if (inputText.Substring(1, 1) == "/")
            {
                date = inputText.Substring(0, 9);
            }
            //if our date is 2/13/2010 
            else
            {
                date = inputText.Substring(0, 10);
            }

            return date;
        }

    }
}
