using System;
using Newtonsoft.Json;

namespace Dapper.基础工具.JSON转换器
{
    /// <summary>
    /// 布尔转换器
    /// 可以将bool转换成"是"或"否"
    /// </summary>
    public class 布尔转换器 : JsonConverter
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public 布尔转换器()
        {
            布尔字符串数组 = "是,否".Split(',');
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="布尔字符串">将bool值转换成的字符串值</param>
        public 布尔转换器(string 布尔字符串)
        {
            if (string.IsNullOrEmpty(布尔字符串))
            {
                throw new ArgumentNullException();
            }
            布尔字符串数组 = 布尔字符串.Split(',');
            if (布尔字符串数组.Length != 2)
            {
                throw new ArgumentException("布尔字符串格式不符合规定");
            }
        }
        #endregion

        #region 属性 布尔字符串数组
        /// <summary>
        /// 布尔字符串数组
        /// </summary>
        private string[] 布尔字符串数组 { get; set; }
        #endregion

        #region 方法 是否可空类型
        /// <summary>
        /// 是否可空类型
        /// </summary>
        /// <param name="类型"></param>
        /// <returns></returns>
        private static bool 是否可空类型(Type 类型)
        {
            if (类型 == null) throw new ArgumentNullException(nameof(类型));
            return 类型.BaseType != null && (类型.BaseType.FullName == "System.ValueType" && 类型.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        #endregion

        #region 重写方法 CanConvert
        /// <summary>
        /// CanConvert
        /// </summary>
        /// <param name="objectType">类型</param>
        /// <returns>为bool类型则可以进行转换</returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
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
            var isNullable = 是否可空类型(objectType);
            var t = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;
            if (reader.TokenType == JsonToken.Null)
            {
                if (!是否可空类型(objectType))
                {
                    throw new Exception($"不能转换空值到 {objectType} ！");
                }

                return null;
            }
            try
            {
                if (reader.TokenType == JsonToken.String)
                {
                    var boolText = reader.Value.ToString();
                    if (boolText.Equals(布尔字符串数组[0], StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    if (boolText.Equals(布尔字符串数组[1], StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

                if (reader.TokenType == JsonToken.Integer)
                {
                    //数值
                    return Convert.ToInt32(reader.Value) == 1;
                }
            }
            catch (Exception)
            {
                throw new Exception($"Error converting value {reader.Value} to type '{objectType}'");
            }
            throw new Exception($"Unexpected token {reader.TokenType} when parsing enum");
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
            var bValue = (bool)value;
            writer.WriteValue(bValue ? 布尔字符串数组[0] : 布尔字符串数组[1]);
        }
        #endregion
    }
}
