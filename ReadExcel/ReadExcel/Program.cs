using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
namespace ReadExcel
{
    class Program
    {
        static void Main(string[] args)
        {

            //Create COM Objects.
            Application excelApp = new Application();


            if (excelApp == null)
            {
                Console.WriteLine("Excel is not installed!!");
                return;
            }
            // stopwatch starts counting time
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Workbook excelBook = excelApp.Workbooks.Open(@"C:\Users\tim.su.ES\Desktop\exportFileSim.csv");
            _Worksheet excelSheet = excelBook.Sheets[1];
            Range excelRange = excelSheet.UsedRange;

            int rows = excelRange.Rows.Count;
            int cols = excelRange.Columns.Count;
            Hashtable res = new Hashtable();
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 2; j <= cols; j++)
                {
                    // the column  Conversation ID  is never empty,  but just in case
                    if ( excelRange.Cells[i, j] != null && excelRange.Cells[i, j].Value2 != null)
                    {
                        if (excelRange.Cells[i, j+1] != null && excelRange.Cells[i, j+1].Value2 != null)
                        {
                            if (res.ContainsKey(excelRange.Cells[i, j].Value2.ToString()))
                            {
                                // prepend Activity ID with Tags at activity time
                                res[excelRange.Cells[i, j].Value2.ToString()] = excelRange.Cells[i, j - 1].Value2.ToString() + ";" + excelRange.Cells[i, j+1].Value2.ToString();
                            }
                            else {
                                // prepend Activity ID with Tags at activity time
                                res.Add(excelRange.Cells[i, j].Value2.ToString(), excelRange.Cells[i, j - 1].Value2.ToString() + ";" + excelRange.Cells[i, j+1].Value2.ToString());
                            }
                        }
                    }

                }
            }
            //generating a csv file and pull all data from hashtable
            using (StreamWriter sw = File.CreateText(@"C:\Users\tim.su.ES\Desktop\list.csv"))
            {
                foreach (string key in res.Keys)
                {
                    var line = (String.Format("{0},{1}", key, res[key]));
                    sw.WriteLine(line);
                }
            }
            //after reading, relase the excel project
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("total amount of time: " + Convert.ToDecimal(elapsedMs)/60000 + "min");
            Console.ReadLine();
        }
    }
}
