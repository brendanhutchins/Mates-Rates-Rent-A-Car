using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    /// <summary>
    ///
    /// Menu driven program that allows the user to navigate between 
    /// customer, fleet, and rental operations. The program can read
    /// CSV files and save instances of CRM and Fleet lists.
    ///
    /// This class was created to output nicely created tables to 
    /// effectively convey the information from the CRM, Fleet, and
    /// Rental lists as well as show a preview of how the information 
    /// is going to be saved once the program is correctly terminated 
    /// using the Escape key in any menu.
    /// 
    /// Inspired by Patrick McDonald:
    /// https://stackoverflow.com/a/856918
    /// 
    /// Author Brendan Hutchins May 2020
    ///
    ///
    /// </summary>
    public class Table
    {
        private int[] colWidths;
        private string[] rows;
        private int tableWidth;

        // Prints top header for table
        public void PrintHeader<T> (string[] headers, List<T> data)
        {
            int numCols = GetNumberOfColumns(data);

            colWidths = GetTableWidths(headers, data);
            string row = "│";
            int colNum = 0;
            foreach (string header in headers)
            {
                row += AlignLeft(header, colWidths[colNum]) + "│";
                tableWidth += colWidths[colNum];
                colNum++;
            }
            tableWidth += (numCols * 2) + 1;
            AddLine(tableWidth);
            Console.WriteLine(row);
            AddLine(tableWidth);
        }

        // Gets each row of the respective lists, needs a header for the table width
        public string[] GetRows<T>(string[] headers, List<T> data)
        {
            int numRows = data.Count;

            colWidths = GetTableWidths(headers, data);
            rows = new string[numRows];
            string row;
            int rowNum = 0;

            foreach (T item in data)
            {
                string line = GetLine(item);
                string[] fields = line.Split('│');
                row = "│";
                int colNum = 0;
                foreach (string field in fields)
                {
                    row += AlignLeft(field, colWidths[colNum]) + "│";
                    colNum++;
                }
                rows[rowNum] += row;
                rowNum++;
            }
            return rows;
        }

        // Get the number of columns for abstract or non abstract lists
        private int GetNumberOfColumns<T> (List<T> data)
        {
            int numCols = typeof(T).GetProperties().Count();
            // If the list is abstract get fields
            if (numCols == 0 || numCols == 1)
            {
                numCols = typeof(T).GetFields().Count();
            }
            return numCols;
        }

        // Add a line of '-' character for the header
        private void AddLine(int width)
        {
            Console.WriteLine(new string('-', width));
        }

        // Get the maximum width of all of the columns, uses list T and a string of headers for comparison
        private int[] GetTableWidths<T> (string[] headers, List<T> data)
        {
            //int numRows = data.Count;
            int numCols = GetNumberOfColumns(data);

            colWidths = new int[numCols];
            // Loop through Headers first
            int colNum = 0;
            foreach (string header in headers)
            {
                if (colWidths[colNum] < header.Length)
                {
                    colWidths[colNum] = header.Length;
                }
                colNum++;
            }
            // Loop through each item in the list to find maximum item width
            foreach (T item in data)
            {
                string line = GetLine(item);
                string[] fields = line.Split('│');
                colNum = 0;
                foreach (string field in fields)
                {
                    if (colWidths[colNum] < field.Length)
                    {
                        colWidths[colNum] = field.Length;
                    }
                    colNum++;
                }
            }
            return colWidths;
        }
        
        // Get each item from list to convert to string line
        private string GetLine <T> (T item)
        {
            string line = item.ToString();
            if (typeof(List<T>) == typeof(List<KeyValuePair<string, int>>))
            {
                line = line.Trim('[', ']');
                line = line.Replace(",", "│").Replace(" ", "");
            }
            return line;
        }

        // Align all items in the columns to the left with padding equivalent to the widths calculated from 
        // GetTableWidths method
        private string AlignLeft(string content, int width)
        {
            return content.PadRight(width + 1);
        }
    }
}
