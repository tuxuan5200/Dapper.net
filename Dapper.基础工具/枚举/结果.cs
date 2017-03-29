using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.基础工具.枚举
{
    /// <summary>
    /// 结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, Id = "结果")]
    public class 结果<T>
    {
        #region 私有字段
        /// <summary>
        /// _状态
        /// </summary>
        private 结果状态 _结果状态;
        #endregion

        #region 属性 结果状态
        /// <summary>
        /// 结果状态
        /// </summary>
        [JsonIgnore]
        public 结果状态 结果状态
        {
            get { return _结果状态; }
            set
            {
                是否成功 = (int)value > 10000000 && (int)value < 20000000;
                _结果状态 = value;
            }
        }
        #endregion

        #region 属性 是否成功
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonProperty("IsSuccess")]
        public bool 是否成功 { get; private set; }
        #endregion

        #region 属性 结果状态枚举解析器
        /// <summary>
        /// 结果状态枚举解析器
        /// </summary>
        [JsonProperty("States")]
        public 枚举解析器<结果状态> 结果状态枚举解析器 => new 枚举解析器<结果状态>(_结果状态);
        #endregion

        #region 属性 结果数据

        /// <summary>
        /// 结果数据
        /// </summary>
        [JsonProperty("Data")]
        public T 结果数据 { get; set; }

        #endregion
    }
}
