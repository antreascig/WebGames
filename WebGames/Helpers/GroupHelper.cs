using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using WebGames.Libs;
using WebGames.Libs.Games.Games;
using WebGames.Libs.Games.GameTypes;
using WebGames.Models;
using Excel = Microsoft.Office.Interop.Excel;


namespace WebGames.Helpers
{
    public class GroupHelper
    {
        public static void Generate()
        {
            Application excel;
            Workbook worKbooK;
            Worksheet worKsheeT;
            Range celLrangE; 

            var fileLocation = @"D:\Development\Repos\WebGames\WebGames\App_Data\omades.xlsx";
            try
            {
                excel = new Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                worKbooK = excel.Workbooks.Add(Type.Missing);


                worKsheeT = (Worksheet)worKbooK.ActiveSheet;
                worKsheeT.Name = "Βαθμολογία παιχτών";

                worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[1, 8]].Merge();
                //worKsheeT.Cells[1, 1] = "Student Report Card";
                worKsheeT.Cells.Font.Size = 15;


                int rowcount = 2;

                var dataTable = ExportToExcel();

                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;
                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {

                        if (rowcount == 3)
                        {
                            worKsheeT.Cells[2, i] = dataTable.Columns[i - 1].ColumnName;
                            worKsheeT.Cells.Font.Color = System.Drawing.Color.Black;

                        }

                        worKsheeT.Cells[rowcount, i] = datarow[i - 1].ToString();

                        if (rowcount > 3)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    celLrangE = worKsheeT.Range[worKsheeT.Cells[rowcount, 1], worKsheeT.Cells[rowcount, dataTable.Columns.Count]];
                                }
                            }
                        }

                    }

                }

                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[rowcount, dataTable.Columns.Count]];
                celLrangE.EntireColumn.AutoFit();
                Borders border = celLrangE.Borders;
                border.LineStyle = XlLineStyle.xlContinuous;
                border.Weight = 2d;

                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[2, dataTable.Columns.Count]];

                worKbooK.SaveAs(fileLocation); ;
                worKbooK.Close();
                excel.Quit();

            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }
            finally
            {
                worKsheeT = null;
                celLrangE = null;
                worKbooK = null;
            }

        }

        private static System.Data.DataTable ExportToExcel( )
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("Θέση", typeof(int));
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Ονομα", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Σκορ", typeof(double));
            table.Columns.Add("Ομάδα", typeof(int));

            var rankings = Group_Manager.GetRankingsBeforeGroups().ToList();

            var Users = new List<ApplicationUser>();

            using (var db = ApplicationDbContext.Create())
            {
                Users.AddRange(db.Users.ToList());
            }
            var UserDict = Users.ToDictionary(u => u.Id);

            foreach (var user in rankings)
            {
                table.Rows.Add(user.Rank, user.UserId, user.User_FullName, UserDict[user.UserId].Email, user.Score, user.Group);
            }
            return table;
        }

        public static void SetFromFile()
        {
            Excel.Range xlRange = null;
            Excel._Worksheet xlWorksheet = null;
            Excel.Workbook xlWorkbook = null;
            Excel.Application xlApp = null;
            try
            {
                //Create COM Objects. Create a COM object for everything that is referenced
                xlApp = new Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(@"D:\Development\Repos\WebGames\WebGames\App_Data\omades.xlsx");
                xlWorksheet = xlWorkbook.Sheets[1];
                xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                var User_Groups = new List<User_Group>();
                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 2; i <= rowCount; i++) // start from 2nd line - first has the headers
                {
                    User_Groups.Add(new User_Group()
                    {
                        UserId = xlRange[i][1],
                        GroupNumber = xlRange[i][5],
                    });
                }

                Group_Manager.SetGroups(User_Groups);

            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }
            finally
            {
                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (xlRange != null)
                    Marshal.ReleaseComObject(xlRange);

                if (xlWorksheet != null)
                    Marshal.ReleaseComObject(xlWorksheet);

                //close and release
                if (xlWorkbook != null)
                {
                    xlWorkbook.Close();
                    Marshal.ReleaseComObject(xlWorkbook);
                }

                if (xlApp != null)
                {
                    //quit and release
                    xlApp.Quit();
                    Marshal.ReleaseComObject(xlApp);
                }

            }
        }
    }
}