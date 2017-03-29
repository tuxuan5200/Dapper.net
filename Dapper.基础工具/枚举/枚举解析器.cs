using Dapper.基础工具.JSON转换器;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.基础工具.枚举
{/// <summary>
 /// 枚举解析器<T/>
 /// </summary>
 /// <typeparam name="T"></typeparam>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, Id = "枚举解析器")]
    public class 枚举解析器<T> : 枚举描述转换器<T> where T : struct, IConvertible
    {
        #region 私有字段
        /// <summary>
        /// 枚举
        /// </summary>
        private T _枚举;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public 枚举解析器(T 枚举)
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T 必须是枚举类型");
            this._枚举 = 枚举;
        }
        #endregion

        #region 属性 代码
        /// <summary>
        /// 代码
        /// </summary>
        [JsonProperty("Code")]
        public int 代码
        {
            get { return _枚举.ToInt32(CultureInfo.InvariantCulture); }
            set { _枚举 = (T)Enum.ToObject(typeof(T), value); }
        }
        #endregion

        #region 属性 名称
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("Name")]
        public string 名称
        {
            get { return Enum.GetName(typeof(T), _枚举); }
            set { Enum.TryParse(value, out _枚举); }
        }
        #endregion

        #region 属性 描述
        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("Description")]
        public string 描述
        {
            get
            {
                //int isNo;
                //return int.TryParse(_枚举.ToString(CultureInfo.InvariantCulture), out isNo)
                //    ? 获取枚举描述(typeof(T), isNo)
                //    : 获取枚举描述(typeof(T), _枚举.ToString(CultureInfo.InvariantCulture));

                return 获取枚举描述(_枚举);
            }
            set
            {
                // 忽略设置
                var v = value;
            }
        }
        #endregion

        #region 静态方法 遍历枚举
        /// <summary>
        /// 遍历枚举
        /// </summary>
        /// <returns></returns>
        public static List<T> 遍历枚举()
        {
            return Enum.GetNames(typeof(T)).Select(name => (T)Enum.Parse(typeof(T), name)).ToList();
        }
        #endregion
    }
}
