using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.基础工具.枚举
{
    /// <summary>
    /// 
    /// 通过 [Description("描述内容！")] 向外公开枚举成员的描述
    /// 
    /// 路径规则：命名空间_类_方法_状态（成功、失败、其他）
    /// 
    /// 状态值规则：二位（成功10、失败20）+三位（所在类序号）+三位（内部序号）
    /// 
    /// </summary>
    //[Flags]
    public enum 结果状态
    {
        #region 100 系统内部

        [Description("系统内部错误！")]
        系统内部错误 = 20100001,
        [Description("参数不能为空！")]
        参数不能为空 = 20100002,
        [Description("超时测试！")]
        超时测试 = 20100003,
        [Description("内部测试！")]
        内部测试 = 20100004,
        [Description("该功能暂未开放！")]
        该功能暂未开放 = 20100005,

        [Description("获取测试域名成功！")]
        获取测试域名成功 = 10100001,
        [Description("获取微信服务账号成功！")]
        获取微信服务账号成功 = 10100002,
        #endregion
    }
}
