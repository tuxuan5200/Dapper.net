

using System;
using System.Data;

namespace Dapper.基础工具.OfficeNPOI
{
    /// <summary>
    ///DataTable 实现导出列接口
    /// </summary>
    public class DataRowExportColumn : IExportColumn<DataRow>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DataRowExportColumn(String name)
            : this(name, String.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="title"></param>
        public DataRowExportColumn(String name, String title)
            : this(name, title, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="funcFormatValue"></param>
        public DataRowExportColumn(String name, String title, Func<Object, Int32, Object> funcFormatValue)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            this.Name = name;
            this._title = title;
            this._funcFormatValue = funcFormatValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public String Name { get; private set; }
        private readonly String _title;
        private readonly Func<Object, Int32, Object> _funcFormatValue;

        public string Title
        {
            get { return String.IsNullOrEmpty(this._title) ? this.Name : this._title; }
        }

        public object GetValue(DataRow row, int index)
        {
            var val = row[this.Name];
            return this._funcFormatValue != null ? _funcFormatValue(val, index) : val;
        }
    }
}
