using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using WebGames.Libs;
using WebGames.Libs.Games.GameTypes;
using WebGames.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace WebGames.Helpers
{
    public class QuestionsHelper
    {
        public static void ReadAndSaveQuestions()
        {
            return;

            Excel.Range xlRange = null;
            Excel._Worksheet xlWorksheet = null;
            Excel.Workbook xlWorkbook = null;
            Excel.Application xlApp = null;
            try
            {
                //Create COM Objects. Create a COM object for everything that is referenced
                xlApp = new Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(@"D:\Development\Repos\WebGames\WebGames\App_Data\questions.xlsx");
                xlWorksheet = xlWorkbook.Sheets[1];
                xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                var Questions = new List<GameQuestionModel>();
                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 2; i <= rowCount; i++) // start from 2nd line - first has the headers
                {
                    var QuestionCell = xlRange.Cells[i, 1];
                    var newQuestion = new GameQuestionModel()
                    {
                        QuestionId = i - 1,
                        Active = true,
                        QuestionText = QuestionCell.Value2.ToString(),
                        Options = new List<string>()
                    };
                    // 1st Columnt is the questions Text
                    for (int j = 2; j <= colCount; j++)
                    {
                        var cell = xlRange.Cells[i, j];
                        //write the value to the console
                        if (cell != null && cell.Value2 != null)
                        {
                            newQuestion.Options.Add(cell.Value2.ToString());
                            if (isBold(cell))
                            {
                                newQuestion.AnswerIndex = j - 1;
                            }
                        }
                    }
                    Questions.Add(newQuestion);
                }

                var Game5MetadataModel = new Game5_MetaData()
                {
                    Questions = Questions.ToDictionary(k => k.QuestionId)
                };

                // Save Questions 
                using (var db = ApplicationDbContext.Create())
                {
                    var game5 = (from game in db.Games where game.GameKey == GameKeys.GAME_5 select game).SingleOrDefault();
                    game5.MetadataJSON = Newtonsoft.Json.JsonConvert.SerializeObject(Game5MetadataModel);
                    db.SaveChanges();
                }

                //cleanup
                //GC.Collect();
                //GC.WaitForPendingFinalizers();

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

                //release com objects to fully kill excel process from running in the background
                //Marshal.ReleaseComObject(xlRange);
                //Marshal.ReleaseComObject(xlWorksheet);

                ////close and release
                //xlWorkbook.Close();
                //Marshal.ReleaseComObject(xlWorkbook);

                ////quit and release
                //xlApp.Quit();
                //Marshal.ReleaseComObject(xlApp);
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

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

                //release com objects to fully kill excel process from running in the background
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

        private static bool isBold(Range cell)
        {
            return cell.Font.Bold;
        }
    }
}

