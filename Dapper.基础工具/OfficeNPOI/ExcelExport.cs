/*
 * *
 * 2015年12月17日 导出Excel
 * *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Dapper.基础工具.OfficeNPOI
{
    /// <summary>
    /// 导出集合到Excel中
    /// </summary>
    public class ExcelExport
    {
        #region 私有

        private static void SetCellValue(ICell cell, Object value)
        {
            if (value == null || value == DBNull.Value)
            {
                cell.SetCellValue(String.Empty);
                return;
            }
            if (value is String)
            {
                cell.SetCellValue((String)value);
                cell.SetCellType(CellType.String);
            }
            else if (value is DateTime)
            {
                if ((DateTime)value != default(DateTime))
                {
                    cell.SetCellValue((DateTime)value);
                }
            }
            else if (value is Boolean)
            {
                cell.SetCellValue((Boolean)value);
                cell.SetCellType(CellType.Boolean);
            }
            else if (value is Int16 || value is Int32 || value is Int64 || value is Byte || value is Decimal || value is float || value is Double)
            {
                cell.SetCellValue(Convert.ToDouble(value));
                cell.SetCellType(CellType.Numeric);
            }
            else
            {
                cell.SetCellValue(value.ToString());
            }
        }
        private static IWorkbook CreateWorkbook(OfficeType excelType)
        {
            if (excelType == OfficeType.Office2007)
            {
                return new XSSFWorkbook();
            }
            return new HSSFWorkbook();
        }
        #endregion

        #region **** 导出单个Sheet页 ***
        /// <summary>
        /// 导出Excel,如果Excel类型为Office2003，那么数据行数不能超过65535，如果超过，则会被拆分到多个工作区中。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="dataSource">数据源</param>
        /// <param name="excelType">EXCEL格式</param>
        /// <param name="sheetName">工作区名称</param>
        /// <param name="saveStream">保存到的文件流</param>
        /// <param name="columns">导出列</param>
        /// <param name="checkNull">检查空数组，true=检查并提示空，false=不检查，导出空excel</param>
        /// <remarks>DataTable使用方法： dt.Rows.Cast《DataRow》().ToList()  new ExportColumn《DataRow》("字段名字",(o,i)=>o[i])</remarks>
        public static void ExportExcel<T>(IList<T> dataSource, OfficeType excelType, string sheetName, Stream saveStream, IList<IExportColumn<T>> columns, bool checkNull = false)
        {
            if (dataSource == null) throw new ArgumentNullException("dataSource");
            if (saveStream == null) throw new ArgumentNullException("saveStream");
            if (columns == null) throw new ArgumentNullException("columns");

            if (checkNull)
            {
                if (dataSource.Count == 0)
                {
                    var ex = new Exception("根据查询条件未找到导出excel数据")
                    {
                        Source = "ExportExcel"
                    };

                    throw ex;
                }
            }

            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "Sheet1";
            }

            var book = Workbook(dataSource, excelType, sheetName, columns);
            book.Write(saveStream);
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="dataSource">数据源</param>
        /// <param name="excelType">EXCEL格式</param>
        /// <param name="sheetName">工作区名称</param>
        ///  <param name="columns">导出列</param>
        /// <param name="workBookName">EXCEL文件名称</param>
        /// <param name="savePath">保存到的服务器相对路径</param>
        /// <param name="checkNull">检查空数组，true=检查并提示空，false=不检查，导出空excel</param>
        /// <remarks>DataTable使用方法： dt.Rows.Cast《DataRow》().ToList()  new ExportColumn《DataRow》("字段名字",(o,i)=>o[i])</remarks>
        /// <returns>返回excel在服务器的地址</returns>
        public static string ExportExcel<T>(IList<T> dataSource, OfficeType excelType, string sheetName, IList<IExportColumn<T>> columns, string workBookName = "", string savePath = "", bool checkNull = false)
        {
            if (dataSource == null) throw new ArgumentNullException("dataSource");
            if (columns == null) throw new ArgumentNullException("columns");

            if (checkNull)
            {
                if (dataSource.Count == 0)
                {
                    var ex = new Exception("根据查询条件未找到导出excel数据")
                    {
                        Source = "ExportExcel"
                    };

                    throw ex;
                }
            }

            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "Sheet1";
            }

            var book = Workbook(dataSource, excelType, sheetName, columns);

            //判断是否传入的路径
            string path = string.IsNullOrEmpty(savePath) ? "/Temporary" : savePath;
            string pathDir;
            if (HttpContext.Current != null)
            {
                pathDir = HttpContext.Current.Request.MapPath(path);
            }
            else
            {
                string temp = path.Substring(0, path.LastIndexOf('/'));
                temp = temp.Replace("/", "\\");
                temp = temp.Replace("~", "");
                if (temp.StartsWith("\\"))
                {
                    temp = temp.TrimStart('\\');
                }
                pathDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, temp);
            }

            string defaultFileName = string.IsNullOrEmpty(workBookName)
                   ? "导出的Excel-" + Guid.NewGuid().ToString("N") + (excelType == OfficeType.Office2003 ? ".xls" : ".xlsx")
                   : workBookName + (excelType == OfficeType.Office2003 ? ".xls" : ".xlsx");

            //实际的文件地址
            string filePath = pathDir + "\\" + defaultFileName;

            //服务器的地址
            string serverPath = path + "/" + defaultFileName;


            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }
            using (FileStream fs = File.Create(filePath))
            {
                book.Write(fs);
            }

            return serverPath;
        }

        /// <summary>
        /// 生成WorkBook对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="excelType"></param>
        /// <param name="sheetName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static IWorkbook Workbook<T>(IList<T> dataSource, OfficeType excelType, string sheetName, IList<IExportColumn<T>> columns)
        {
            //预定义
            var book = CreateWorkbook(excelType);
            var encode = Encoding.GetEncoding(936);
            //样式
            var headerCellStyle = book.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.Center;
            var headerFont = book.CreateFont();
            headerFont.FontHeightInPoints = 10;
            headerFont.Boldweight = 700;
            headerCellStyle.SetFont(headerFont);
            var dateStyle = book.CreateCellStyle();
            var format = book.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy.mm.dd");

            //Sheet计算
            int sheetDataCount;
            switch (excelType)
            {
                case OfficeType.Office2003:
                    sheetDataCount = 65534;
                    break;
                case OfficeType.Office2007:
                    sheetDataCount = 1048574;
                    break;
                default:
                    sheetDataCount = 65534;
                    break;
            }
            var sheetList = new List<ISheet>();

            long dataSourceCount = dataSource.Count;

            if (dataSourceCount > sheetDataCount)
            {
                var sheetCount = dataSourceCount / sheetDataCount;
                if (dataSourceCount % sheetDataCount > 0)
                {
                    sheetCount++;
                }
                for (int i = 0; i < sheetCount; i++)
                {
                    sheetList.Add(book.CreateSheet(String.Format("{0}({1})", sheetName, i + 1)));
                }
            }
            else
            {
                sheetList.Add(book.CreateSheet(sheetName));
            }
            //填充数据
            var rowIndex = 0;
            var sheetIndex = 0;
            var arrColumnWidth = new Int32[] { };

            //空数据返回表头
            if (dataSourceCount == 0)
            {
                var sheet = sheetList[sheetIndex];
                arrColumnWidth = new int[columns.Count];
                var headerRow = sheet.CreateRow(rowIndex);
                for (int i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    headerRow.CreateCell(i).SetCellValue(column.Title);
                    arrColumnWidth[i] = encode.GetByteCount(column.Title);
                }

                for (int i = 0; i < columns.Count; i++)
                {
                    var cell = sheet.GetRow(0).GetCell(i);
                    cell.CellStyle = headerCellStyle;
                    if (arrColumnWidth[i] > 100) arrColumnWidth[i] = 100;
                    sheet.SetColumnWidth(i, (arrColumnWidth[i] + 5) * 256);
                }
                return book;
            }

            foreach (var row in dataSource)
            {
                var sheet = sheetList[sheetIndex];

                //写列头
                if (rowIndex == 0)
                {
                    arrColumnWidth = new int[columns.Count];
                    var headerRow = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var column = columns[i];
                        headerRow.CreateCell(i).SetCellValue(column.Title);
                        arrColumnWidth[i] = encode.GetByteCount(column.Title);
                    }
                }
                //写内容

                var rowData = sheet.CreateRow(rowIndex + 1);
                for (int i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    var data = column.GetValue(row, i);

                    if (data == null || data == DBNull.Value || data.ToString() == "")
                    {
                        continue;
                    }

                    ICell cell = rowData.CreateCell(i);
                    SetCellValue(cell, data);
                    if (data != DBNull.Value)
                    {
                        string str = data.ToString(); //转换为字符串
                        int cellWidth = encode.GetByteCount(str);
                        if (cellWidth > arrColumnWidth[i])
                        {
                            arrColumnWidth[i] = cellWidth;
                        }
                        if (data is DateTime)
                        {
                            cell.CellStyle = dateStyle;
                        }
                    }
                }

                rowIndex++;
                if (rowIndex > sheetDataCount)
                {
                    //设置列样式
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var cell = sheet.GetRow(0).GetCell(i);
                        cell.CellStyle = headerCellStyle;
                        if (arrColumnWidth[i] > 100) arrColumnWidth[i] = 100;
                        sheet.SetColumnWidth(i, (arrColumnWidth[i] + 5) * 256);
                    }
                    rowIndex = 0;
                    sheetIndex++;

                    //一个sheet页面GC一次
                    GC.Collect();
                }
            }

            if (sheetList.Count > 0)
            {
                //针对最后一个sheet设置样式
                var sheet = sheetList[sheetList.Count - 1];
                for (int i = 0; i < columns.Count; i++)
                {
                    var cell = sheet.GetRow(0).GetCell(i);
                    cell.CellStyle = headerCellStyle;
                    if (arrColumnWidth[i] > 100) arrColumnWidth[i] = 100;
                    sheet.SetColumnWidth(i, (arrColumnWidth[i] + 5) * 256);
                }
            }

            return book;
        }
        #endregion


        #region *** 导出多个Sheet页  ***
        /// <summary>
        /// 导出一个excel多个Sheet页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelType"></param>
        /// <param name="listDataSource"></param>
        /// <param name="saveStream"></param>
        public static void ExportExcelMultity<T>(OfficeType excelType, IList<Tuple<IList<T>, IList<IExportColumn<T>>, string>> listDataSource, Stream saveStream)
        {
            var book = WorkbookMulity(excelType, listDataSource);
            book.Write(saveStream);
        }


        /// <summary>
        /// 导出一个excel多个Sheet页
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="excelType">excel格式</param>
        /// <param name="listDataSource">数据源</param>
        /// <param name="workBookName">保存到服务器的文件名</param>
        /// <param name="savePath">保存到服务器的地址</param>
        /// <returns></returns>
        public static string ExportExcelMultity<T>(OfficeType excelType, IList<Tuple<IList<T>, IList<IExportColumn<T>>, string>> listDataSource, string workBookName = "", string savePath = "")
        {
            var book = WorkbookMulity(excelType, listDataSource);

            //判断是否传入的路径
            string path = string.IsNullOrEmpty(savePath) ? "/Temporary" : savePath;
            string pathDir;
            if (HttpContext.Current != null)
            {
                pathDir = HttpContext.Current.Request.MapPath(path);
            }
            else
            {
                string temp = path.Substring(0, path.LastIndexOf('/'));
                temp = temp.Replace("/", "\\");
                temp = temp.Replace("~", "");
                if (temp.StartsWith("\\"))
                {
                    temp = temp.TrimStart('\\');
                }
                pathDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, temp);
            }

            string defaultFileName = string.IsNullOrEmpty(workBookName)
                   ? "导出的Excel-" + Guid.NewGuid().ToString("N") + (excelType == OfficeType.Office2003 ? ".xls" : ".xlsx")
                   : workBookName + (excelType == OfficeType.Office2003 ? ".xls" : ".xlsx");


            //实际的文件地址
            string filePath = pathDir + "\\" + defaultFileName;

            //服务器的地址
            string serverPath = path + "/" + defaultFileName;

            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }
            using (FileStream fs = File.Create(filePath))
            {
                book.Write(fs);
            }

            return serverPath;
        }


        /// <summary>
        /// 生成WorkBook对象
        /// </summary>
        /// <returns></returns>
        private static IWorkbook WorkbookMulity<T>(OfficeType excelType, IList<Tuple<IList<T>, IList<IExportColumn<T>>, string>> listDataSource)
        {
            //预定义
            var book = CreateWorkbook(excelType);
            var encode = Encoding.GetEncoding(936);
            //样式
            var headerCellStyle = book.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.Center;
            var headerFont = book.CreateFont();
            headerFont.FontHeightInPoints = 10;
            headerFont.Boldweight = 700;
            headerCellStyle.SetFont(headerFont);
            var dateStyle = book.CreateCellStyle();
            var format = book.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy.mm.dd");

            //Sheet计算
            int sheetDataCount;
            switch (excelType)
            {
                case OfficeType.Office2003:
                    sheetDataCount = 65534;
                    break;
                case OfficeType.Office2007:
                    sheetDataCount = 1048574;
                    break;
                default:
                    sheetDataCount = 65534;
                    break;
            }

            //循环创建多个sheet
            var sheetIndex = 0;
            foreach (Tuple<IList<T>, IList<IExportColumn<T>>, string> tuple in listDataSource)
            {
                IList<T> dataSource = tuple.Item1;
                IList<IExportColumn<T>> columns = tuple.Item2;
                string sheetName = tuple.Item3;

                var sheetList = new List<ISheet>();
                long dataSourceCount = dataSource.Count;
                if (dataSourceCount > sheetDataCount)
                {
                    var sheetCount = dataSourceCount / sheetDataCount;
                    if (dataSourceCount % sheetDataCount > 0)
                    {
                        sheetCount++;
                    }
                    for (int i = 0; i < sheetCount; i++)
                    {
                        sheetList.Add(book.CreateSheet(String.Format("{0}({1})", sheetName, i + 1)));
                    }
                }
                else
                {
                    sheetList.Add(book.CreateSheet(sheetName));
                }
                //填充数据
                var rowIndex = 0;
                //var sheetIndex = 0;
                var arrColumnWidth = new Int32[] { };

                //空数据返回表头
                if (dataSourceCount == 0)
                {
                    var sheet = sheetList[sheetIndex];
                    arrColumnWidth = new int[columns.Count];
                    var headerRow = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var column = columns[i];
                        headerRow.CreateCell(i).SetCellValue(column.Title);
                        arrColumnWidth[i] = encode.GetByteCount(column.Title);
                    }
                    continue;
                }

                foreach (var row in dataSource)
                {
                    var sheet = sheetList[sheetIndex];

                    //写列头
                    if (rowIndex == 0)
                    {
                        arrColumnWidth = new int[columns.Count];
                        var headerRow = sheet.CreateRow(rowIndex);
                        for (int i = 0; i < columns.Count; i++)
                        {
                            var column = columns[i];
                            headerRow.CreateCell(i).SetCellValue(column.Title);
                            arrColumnWidth[i] = encode.GetByteCount(column.Title);
                        }
                    }
                    //写内容
                    var rowData = sheet.CreateRow(rowIndex + 1);
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var column = columns[i];
                        var data = column.GetValue(row, i);
                        if (data == null || data == DBNull.Value || data.ToString() == "")
                        {
                            continue;
                        }
                        var cell = rowData.CreateCell(i);

                        SetCellValue(cell, data);
                        if (data != DBNull.Value)
                        {
                            var str = data.ToString(); //转换为字符串
                            var cellWidth = encode.GetByteCount(str);
                            if (cellWidth > arrColumnWidth[i])
                            {
                                arrColumnWidth[i] = cellWidth;
                            }
                            if (data is DateTime)
                            {
                                cell.CellStyle = dateStyle;
                            }
                        }
                    }

                    rowIndex++;
                    if (rowIndex > sheetDataCount)
                    {
                        //设置列样式
                        for (int i = 0; i < columns.Count; i++)
                        {
                            var cell = sheet.GetRow(0).GetCell(i);
                            cell.CellStyle = headerCellStyle;
                            if (arrColumnWidth[i] > 100) arrColumnWidth[i] = 100;
                            sheet.SetColumnWidth(i, (arrColumnWidth[i] + 5) * 256);
                        }
                        rowIndex = 0;
                        sheetIndex++;

                        //一个sheet页面GC一次
                        GC.Collect();
                    }
                }

                if (sheetList.Count > 0)
                {
                    //针对最后一个sheet设置样式
                    var sheet = sheetList[sheetList.Count - 1];
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var cell = sheet.GetRow(0).GetCell(i);
                        cell.CellStyle = headerCellStyle;
                        if (arrColumnWidth[i] > 100) arrColumnWidth[i] = 100;
                        sheet.SetColumnWidth(i, (arrColumnWidth[i] + 5) * 256);
                    }
                }
            }

            return book;
        }
        #endregion

    }


    /// <summary>
    /// Office文件格式
    /// </summary>
    public enum OfficeType
    {
        /// <summary>
        /// 97-2003格式
        /// </summary>
        [Description("Office2003")]
        Office2003,
        /// <summary>
        /// 2007+格式
        /// </summary>
        [Description("Office2007")]
        Office2007
    }

}