using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UITest.Util
{
    public class Tool
    {

        const string Crashes_Total = "Reliability-Crashes_Total-";
        const string TMAD = "OSAdoption-TMAD-";
        const string Crashes = "Reliability-Crashes-";

        const string path = "C:\\Users\\501805\\Desktop\\auto1\\TelemetryExcel1.1\\ExportData\\";
        const string suffix = ".csv";
        static string[] Name = { "rltkapou64.dll", "rltkapo64.dll", "igdkmd64.sys" };


        static Dictionary<string, string> Condition = new Dictionary<string, string> {

            {"OSVersion", "10.0.19042.572"},
            {"DriverVersion", "26.20.100.7872"},
            {"ReleaseVersion", "2004 | Vb"}
        };

        public static void QueryByJSON()
        {
            
        }




        public static void GenerateExcel()
        {
            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            //Initailize worksheet
            //workbook.CreateEmptySheets(1);
            if (File.Exists("RecentData.xlsx"))
            {
                workbook.LoadFromFile("RecentData.xlsx");
            }
            else
            {
                InitiExcel(Name);
                workbook.LoadFromFile("RecentData.xlsx");
            }
            //LoadData(workbook, "Crashes",Name ,Condition);
            LoadData(workbook, "Total_Crashes",Name,Condition);

            workbook.SaveToFile("RecentData.xlsx", ExcelVersion.Version2013);

            GenerateChart();
        }
        public static void LoadData(Spire.Xls.Workbook workbook, string sheetName,  string[] name,Dictionary<string,string> Condition)
        {
            Spire.Xls.Worksheet sheet = workbook.Worksheets[sheetName];
            int lastRow = sheet.Range.LastRow;              //2020/10/12 0:00:00

            string[] date_1 = sheet.Range["B" + lastRow].Value == "Date" ? "2020/7/1".Split("/") : sheet.Range["B" + lastRow].Value.Split(" ")[0].ToString().Split("/");
            DateTime dateTime = new DateTime(Convert.ToInt32(date_1[0]), Convert.ToInt32(date_1[1]), Convert.ToInt32(date_1[2]));

            string curr_Date = DateTime.Now.ToString("MM-dd");
            string curr_Date1 = DateTime.Now.ToString("yyyy/M/dd");
            for (int i = 1; i < 1000; i++)
            {
                string[] fileName = Directory.GetFiles("C:\\Users\\501805\\Desktop\\auto1\\TelemetryExcel1.1\\ExportData", "*.csv");

                int lastRow1 = sheet.Range.LastRow + 1;

                string new_date1 = dateTime.AddDays(i).ToString("MM-dd");
                string fullpath = path + Crashes_Total + new_date1 + suffix;
                string fullpath1 = path + TMAD + new_date1 + suffix;
                DateTime new_date = dateTime.AddDays(i);
                if (fileName.Contains(fullpath))
                {
                    sheet.Range["B" + lastRow1].DateTimeValue = new_date;
                    for (int x = 1; x <= name.Length; x++)
                    {
                        sheet.Range[Convert.ToChar('B' + x).ToString() + lastRow1].NumberValue = AnalyzeCSV(fullpath, "Crashes", name[x - 1],Condition)[0];
                    }
                    sheet.Range[Convert.ToChar('B' + (name.Length + 1)).ToString() + lastRow1].NumberValue = AnalyzeCSV(fullpath, "Crashes", name[0], Condition)[1];

                    if (fileName.Contains(fullpath1))
                    {
                        if (AnalyzeCSV(fullpath1, "TMAD", "2004 | Vb", Condition)[0] != 0)
                        {
                            sheet.Range[Convert.ToChar('B' + (name.Length + 4)).ToString() + lastRow1].NumberValue = AnalyzeCSV(fullpath1, "TMAD", "2004 | Vb", Condition)[0];
                        }
                        else
                        {
                            sheet.Range[Convert.ToChar('B' + (name.Length + 4)).ToString() + lastRow1].NumberValue = AnalyzeCSV(fullpath1, "TMAD", "Insider | Vb", Condition)[0];
                        }
                    }
                    else
                    {
                        int lastRow2 = lastRow1 - 1;
                        sheet.Range[Convert.ToChar('B' + (name.Length + 4)).ToString() + lastRow1].NumberValue = sheet.Range["I" + lastRow2].NumberValue;
                    }
                    sheet.Range[Convert.ToChar('B' + (name.Length + 3)).ToString() + lastRow1].Formula = "=(C" + lastRow1 + "+D" + lastRow1 + "+E" + lastRow1 + ")/I" + lastRow1;
                    sheet.Range[Convert.ToChar('B' + (name.Length + 3)).ToString() + lastRow1].NumberFormat = "0.000%";

                    sheet.Range[Convert.ToChar('B' + (name.Length + 2)).ToString() + lastRow1].Value = "||";

                    Console.WriteLine(new_date.ToString("yyyy/M/dd"));
                }

                if (new_date.ToString("yyyy/M/dd") == curr_Date1 || dateTime.ToString("yyyy/M/dd") == curr_Date1) { break; }
            }
            Console.WriteLine("now date :" + curr_Date);
        }
        public static void InitiExcel(string[] name)
        {
            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            Spire.Xls.Worksheet newSheet = workbook.CreateEmptySheet("Crashes");
            workbook.Worksheets.Remove("sheet1");
            workbook.Worksheets.Remove("sheet2");
            workbook.Worksheets.Remove("sheet3");

            newSheet.Range["A1:M1"].ColumnWidth = 15;
            //newSheet.Range["A:L"].HorizontalAlignment = HorizontalAlignType.Center;
            newSheet.Range["B6"].Value = "Date";
            for (int i = 1; i <= name.Length; i++)
            {
                newSheet.Range[Convert.ToChar('B' + i).ToString() + "6"].Value = name[i - 1];
            }
            newSheet.Range[Convert.ToChar('B' + (name.Length + 1)).ToString() + "6"].Value = "All Crashes";
            newSheet.Range[Convert.ToChar('B' + (name.Length + 2)).ToString() + "6"].Value = "Percent Impacted";
            newSheet.Range[Convert.ToChar('B' + (name.Length + 3)).ToString() + "6"].Value = "Crashes/20H1";
            newSheet.Range[Convert.ToChar('B' + (name.Length + 4)).ToString() + "6"].Value = "20H1";
            newSheet.Range["B6:" + Convert.ToChar('B' + (name.Length + 4)).ToString() + "6"].BorderInside(LineStyleType.Thin, Color.LightBlue);
            newSheet.Range["B6:" + Convert.ToChar('B' + (name.Length + 4)).ToString() + "6"].BorderAround(LineStyleType.Medium, Color.LightBlue);

            Spire.Xls.Worksheet newSheet1 = workbook.CreateEmptySheet("Total_Crashes");
            newSheet1.Range["A1:M1"].ColumnWidth = 15;
            newSheet1.Range.HorizontalAlignment = HorizontalAlignType.Center;
            newSheet1.Range["B6"].Value = "Date";

            for (int i = 1; i <= name.Length; i++)
            {
                newSheet1.Range[Convert.ToChar('B' + i).ToString() + "6"].Value = name[i - 1];
            }
            newSheet1.Range[Convert.ToChar('B' + (name.Length + 1)).ToString() + "6"].Value = "All Crashes";
            newSheet1.Range[Convert.ToChar('B' + (name.Length + 2)).ToString() + "6"].Value = "Percent Impacted";
            newSheet1.Range[Convert.ToChar('B' + (name.Length + 3)).ToString() + "6"].Value = "Crashes/20H1";
            newSheet1.Range[Convert.ToChar('B' + (name.Length + 4)).ToString() + "6"].Value = "20H1";
            newSheet1.Range["B6:" + Convert.ToChar('B' + (name.Length + 4)).ToString() + "6"].BorderInside(LineStyleType.Thin, Color.LightBlue);
            newSheet1.Range["B6:" + Convert.ToChar('B' + (name.Length + 4)).ToString() + "6"].BorderAround(LineStyleType.Medium, Color.LightBlue);

            workbook.SaveToFile("RecentData.xlsx", ExcelVersion.Version2013);
        }

        public static void GenerateChart()
        {
            if (File.Exists("Trend Analysis.xlsx")) File.Delete("Trend Analysis.xlsx");
            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            Spire.Xls.Worksheet newSheet = workbook.CreateEmptySheet("Crashes");
            Spire.Xls.Worksheet newSheet1 = workbook.CreateEmptySheet("Total_Crashes");
            workbook.Worksheets.Remove("sheet1");
            workbook.Worksheets.Remove("sheet2");
            workbook.Worksheets.Remove("sheet3");

            newSheet.Range["A1:M1"].ColumnWidth = 15;
            newSheet1.Range["A1:M1"].ColumnWidth = 15;
            //newSheet.Range.HorizontalAlignment = HorizontalAlignType.Center;


            Spire.Xls.Workbook workbook1 = new Spire.Xls.Workbook();
            workbook1.LoadFromFile("RecentData.xlsx");
            int lastRow = workbook1.Worksheets["Crashes"].LastRow;
            int lastRow1 = workbook1.Worksheets["Total_Crashes"].LastRow;
            workbook1.Worksheets["Crashes"].Range["B6:I6"].Copy(workbook.Worksheets["Crashes"].Range["A6:H6"]);
            workbook1.Worksheets["Crashes"].Range["B" + (lastRow - 29) + ":I" + lastRow].Copy(workbook.Worksheets["Crashes"].Range["A7:H36"]);

            workbook1.Worksheets["Total_Crashes"].Range["B6:I6"].Copy(workbook.Worksheets["Total_Crashes"].Range["A6:H6"]);
            workbook1.Worksheets["Total_Crashes"].Range["B" + (lastRow1 - 29) + ":I" + lastRow1].Copy(workbook.Worksheets["Total_Crashes"].Range["A7:H36"]);
            workbook.SaveToFile("Trend Analysis.xlsx", ExcelVersion.Version2013);


        }

        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        public static int[] AnalyzeCSV(string path, string type, string Name,Dictionary<string,string> condition)
        {
            if (type == "Crashes")
            {
                readCSV(path, out DataTable dt);
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName.Contains("["))
                    {
                        string ColumnName = column.ColumnName.Split("[")[1].Split("]")[0];
                        column.ColumnName = ColumnName.Replace(" ", "");
                    }
                }
                int[] crashes = { 0, 0 };
                //total = dt.AsEnumerable()
                //                   .Where(r => r.Field<string>("Crashes") != null)
                //                   .Sum(x => Convert.ToInt32(x.Field<string>("Crashes")));
                //object total = dt.Compute("SUM(Crashes)", "");
                double total = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (IsInt(row["Crashes"].ToString()))
                    {
                        total += double.Parse(row["Crashes"].ToString());
                    }
                    else
                    {
                        continue;
                    }
                }
                crashes[1] = Convert.ToInt32(total);
                //dt.AsEnumerable().Where(r => r.Field<string>("OSVersion") != null).Select("DriverName= '" + condition + "' AND ISNULL(OSVersion = '10.0.19042.572',0)");
                //   AND ISNULL(DriverVersion = '26.20.100.7872',0)',0)AND ISNULL(OSVersion = '26.20.100.7872',0)AND ISNULL(DriverVersion = '26.20.100.7872',0)
                //(DataRow i in dt.Select("DriverName= '" + condition + "' AND ISNULL(DriverVersion = '11.0.6000.620',0) AND ISNULL(OSVersion = '10.0.19042.572',0) "))

                //dt.AsEnumerable().Where(d=>d["DriverName"].ToString()== condition)
                foreach (DataRow i in editExp(dt,Name,condition)) //"DriverName = 'rltkapou64.dll'"
                {
                    object crash = i["Crashes"];
                    crashes[0] += Convert.ToInt32(crash);
                }
                //dt.AsEnumerable().Select(d => d.Field<string>("OSVersion")).Distinct().ToList<string>()
                return crashes;
            }
            else if (type == "TMAD")
            {
                bool flag = readCSV(path, out DataTable dt);

                dt.Columns[0].ColumnName = "OSVersion";
                int[] Tmad = { 0 };
                foreach (DataRow i in dt.Select("OSVersion= '" + condition + "'"))  //"OSVersion= '2004 | Vb'"
                {
                    object tmad = i.ItemArray[1];
                    Tmad[0] += Convert.ToInt32(tmad);
                }
                return Tmad;
            }
            return new int[2] { 0, 0 };
        }

        public static EnumerableRowCollection<DataRow> editExp(DataTable d,string name,Dictionary<string,string> condition)
        {
            
            EnumerableRowCollection<DataRow> enumerable = d.AsEnumerable().Where(d => d["DriverName"].ToString() == name).Where(d => d["DriverVersion"].ToString() == "11.0.6000.620").Where(d => d["OSVersion"].ToString() == "10.0.19042.572");

            return enumerable;
        }

        static public bool readCSV(string filePath, out DataTable dt)//从csv读取数据返回table
        {
            dt = new DataTable();
            try
            {
                System.Text.Encoding encoding = Encoding.Default;//GetType(filePath); //
                                                                 // DataTable dt = new DataTable();
                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);
                System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);
                //记录每次读取的一行记录
                string strLine = "";
                //记录每行记录中的各字段内容
                string[] aryLine = null;
                string[] tableHead = null;
                int columnCount = 0;
                //标示是否是读取的第一行
                bool IsFirst = true;
                //逐行读取CSV中的数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (IsFirst == true)
                    {
                        tableHead = strLine.Split(',');
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        for (int i = 0; i < columnCount; i++)
                        {
                            DataColumn dc = new DataColumn(tableHead[i]);
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (aryLine != null && aryLine.Length > 0)
                {
                    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                }
                sr.Close();
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
