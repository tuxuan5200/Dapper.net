using System;

namespace Dapper.基础工具.OfficeNPOI
{
    /// <summary>
    /// 对象数组 实现导出列接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExportColumn<T> : IExportColumn<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="funcGetValue"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExportColumn(String title, Func<T, Int32, Object> funcGetValue)
        {
            if (String.IsNullOrEmpty(title)) throw new ArgumentNullException("title");
            if (funcGetValue == null) throw new ArgumentNullException("funcGetValue");
            this.Title = title;
            this._funcGetValue = funcGetValue;
        }

        private readonly Func<T, Int32, Object> _funcGetValue;
        public string Title { get; private set; }

        public object GetValue(T row, int index)
        {
            return this._funcGetValue(row, index);
        }
    }
}
