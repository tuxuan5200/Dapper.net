using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.基础工具.枚举
{
    public enum 枚举示例
    {
        [Description("未知！")]
        未知,

        [Description("未处理！")]
        未处理,

        [Description("已处理！")]
        已处理
    }
}
