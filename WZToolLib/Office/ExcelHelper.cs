using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Office.Interop.Excel;

namespace WZToolLib.Office
{
    public static class ExcelHelper
    {
        public static DataSet ReadFromExcel(string filename)
        {
            object missing = Type.Missing;
            Application application = new ApplicationClass();
            Workbook workbook = application.Workbooks.Open(filename.Trim(), missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            DataSet dataSet = new DataSet();
            for (int i = 0; i < workbook.Sheets.Count; i++)
            {
                System.Data.DataTable dataTable = dataSet.Tables.Add();
                Worksheet worksheet = workbook.Sheets[i + 1] as Worksheet;
                int j;
                for (j = 1; j <= 200000; j++)
                {
                    string text = (worksheet.Cells[1, j] as Range).Text.ToString();
                    if (string.IsNullOrEmpty(text.Trim()))
                    {
                        break;
                    }
                    dataTable.Columns.Add(text);
                }

                for (int k = 2; k < 200000; k++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    int num = 0;
                    for (int l = 1; l < j; l++)
                    {
                        string text = (worksheet.Cells[k, l] as Range).Text.ToString();
                        if (string.IsNullOrEmpty(text.Trim()))
                        {
                            num++;
                        }
                        else
                        {
                            dataRow[l - 1] = text;
                        }
                    }
                    if (num >= j - 1)
                    {
                        break;
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            application.ActiveWorkbook.Saved = false;
            workbook.Close(false, null, null);
            application.Quit();

            return dataSet;
        }
    }
}
