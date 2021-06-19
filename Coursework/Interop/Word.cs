using word = Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Coursework.Model;
namespace Coursework.Interop
{
    public static class Word
    {
        internal static void Create(University university)
        {
            object missingObj = System.Reflection.Missing.Value;
            object trueObj = true;
            object falseObj = false;
            try
            {
                DirectoryInfo exePath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                if (!Directory.Exists(exePath + $@"Отчеты"))
                    exePath.CreateSubdirectory($"Отчеты");
                DirectoryInfo workPath = new DirectoryInfo(exePath + $@"Отчеты");
                var wordApp = new word.Application();
                var document = wordApp.Documents.Add(Template: $@"{exePath}TemplateUSpecs.dotx");
                var bookmarks = document.Bookmarks;
                bookmarks["UniversityName"].Range.Text = university.FullName;
                bookmarks["Address"].Range.Text =
                    university.Region + " г. " + university.City + " ул. " + university.Address;

                var table = document.Tables[1];
                int rowCount = 2;
                List<int> rowsSpan = new List<int>();
                foreach (var specialty in university.Specialty)
                {
                    bool firstSpec = true;
                    int specCount = 0;
                    rowsSpan.Add(specialty.NowSpec.Count);
                    table.Rows.Add(missingObj);
                    table.Rows[rowCount].Cells[1].Range.Text = specialty.Name;
                    table.Rows[rowCount].Cells[2].Range.Text = specialty.Code;
                    table.Rows[rowCount].Cells[3].Range.Text = specialty.Duration;
                    foreach (var specSpecialty in specialty.NowSpec)
                    {
                        if (!firstSpec)
                            table.Rows.Add(missingObj);
                        else
                            firstSpec = false;
                        table.Rows[rowCount].Cells[4].Range.Text = specSpecialty.Form;
                        table.Rows[rowCount].Cells[5].Range.Text = specSpecialty.Type;
                        table.Rows[rowCount].Cells[6].Range.Text = specSpecialty.Count.ToString();
                        table.Rows[rowCount].Cells[7].Range.Text = specSpecialty.AvgScore;
                        if (specSpecialty.Type == "Бюджет")
                            table.Rows[rowCount].Cells[8].Range.Text = "бесплатно";
                        else
                            table.Rows[rowCount].Cells[8].Range.Text = specSpecialty.Price.ToString() + " руб. в год";
                        rowCount++;
                        specCount++;
                    }
                }
                word.Cell cel = null;
                for (int i = university.Specialty.Count; i > 0; i--)
                {
                    if (rowsSpan[i - 1] != 1)
                    {
                        cel = table.Cell(rowCount - rowsSpan[i - 1], 1);
                        cel.Merge(table.Cell(rowCount - 1, 1));
                        cel = table.Cell(rowCount - rowsSpan[i - 1], 2);
                        cel.Merge(table.Cell(rowCount - 1, 2));
                        cel = table.Cell(rowCount - rowsSpan[i - 1], 3);
                        cel.Merge(table.Cell(rowCount - 1, 3));
                        rowCount -= rowsSpan[i - 1];
                    }
                }

                object filename = $@"{workPath}\Отчет {university.Name}.docx";
                document.SaveAs(ref filename);
                document.Close(ref missingObj, ref missingObj, ref missingObj);
                document = null;
                wordApp.Quit(ref missingObj, ref missingObj, ref missingObj);
                wordApp = null;
                MessageBox.Show($"Отчет по {university.Name} сделан!\nРасположен в {exePath}\\Отчеты");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}