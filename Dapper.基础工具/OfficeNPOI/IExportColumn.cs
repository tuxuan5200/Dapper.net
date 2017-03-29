using System;

namespace Dapper.基础工具.OfficeNPOI
{
    /// <summary>
    /// 导出列接口
    /// </summary>
    /// <typeparam name="T">数据行类型</typeparam>
    public interface IExportColumn<in T>
    {
        /// <summary>
        /// 列标题
        /// </summary>
        String Title { get; }
        /// <summary>
        /// 获取该列的值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        Object GetValue(T row, Int32 index);
    }
}
