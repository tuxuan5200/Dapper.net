using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Dapper.基础工具.JSON转换器
{
    /// <summary>
    /// 枚举描述转换器<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    // ReSharper disable once InconsistentNaming
    public class 枚举描述转换器<T> : JsonConverter where T : struct, IConvertible
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public 枚举描述转换器()
        {
            if (!typeof(T).IsEnum) { throw new ArgumentException("T 必须是枚举类型"); }
        }
        #endregion

        #region 重写JsonConverter成员

        #region 重写方法 CanConvert

        /// <summary>
        /// 确定此实例是否可以转换成指定的对象类型。
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        #endregion

        #region 重写方法 ReadJson

        /// <summary>
        /// ReadJson
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            try
            {
                return reader.Value;
            }
            catch (Exception)
            {
                throw new Exception($"不能将枚举{objectType}的值{reader.Value}转换为Json格式.");
            }
        }

        #endregion

        #region 重写方法 WriteJson

        /// <summary>
        /// WriteJson
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            var bValue = value.ToString();
            int isNo;
            if (int.TryParse(bValue, out isNo))
            {
                bValue = 获取枚举描述(typeof(T), isNo);
            }
            else
            {
                bValue = 获取枚举描述(typeof(T), value.ToString());
            }
            writer.WriteValue(bValue);
        }

        #endregion

        #endregion

        #region 私有静态方法 获取枚举描述
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="value">枚举名称</param>
        /// <returns></returns>
        private static string 获取枚举描述(Type type, string value)
        {
            try
            {
                var 字段 = type.GetField(value);
                if (字段 == null)
                {
                    return "";
                }
                var 自定义属性 = Attribute.GetCustomAttribute(字段, typeof(DescriptionAttribute)) as DescriptionAttribute;
                return 自定义属性 != null ? 自定义属性.Description : "";
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 私有静态方法 获取枚举描述
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="value">枚举hasecode</param>
        /// <returns></returns>
        private static string 获取枚举描述(Type type, int value)
        {
            try
            {
                var 字段 = type.GetField(Enum.GetName(type, value));
                if (字段 == null) return string.Empty;
                var 自定义属性 = Attribute.GetCustomAttribute(字段, typeof(DescriptionAttribute)) as DescriptionAttribute;
                return 自定义属性 != null ? 自定义属性.Description : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region 静态方法 获取枚举描述
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="枚举值列表">枚举值列表</param>
        /// <returns>枚举值对应的描述集合</returns>
        public static List<string> 获取枚举描述(List<int> 枚举值列表)
        {
            var type = typeof(T);
            return (from fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Static) select (T)fieldInfo.GetValue(null) into item where 枚举值列表.Find(e => e == Convert.ToInt32(item)) > 0 select 获取枚举描述(item)).ToList();
        }
        #endregion

        #region 静态方法 获取枚举描述
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="枚举值"></param>
        /// <returns></returns>
        public static string 获取枚举描述(int 枚举值)
        {
            return 获取枚举描述((T)Enum.ToObject(typeof(T), 枚举值));
        }
        #endregion

        #region 静态方法 获取枚举描述
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="枚举"></param>
        /// <returns>枚举值对应的描述</returns>
        // ReSharper disable once MemberCanBeProtected.Global
        public static string 获取枚举描述(T 枚举)
        {
            var type = 枚举.GetType();
            var memInfo = type.GetMember(枚举.ToString(CultureInfo.InvariantCulture));
            if (memInfo.Length <= 0) return 枚举.ToString(CultureInfo.InvariantCulture);
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : 枚举.ToString(CultureInfo.InvariantCulture);
        }
        #endregion
    }
}