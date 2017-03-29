using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dapper.基础工具.JSON转换器
{
    /// <summary>
    /// 键值转换器
    /// </summary>
    public class 键值转换器 : JsonConverter
    {
        private readonly Type[] _types;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="types"></param>
        public 键值转换器(params Type[] types)
        {
            _types = types;
        }
        #endregion

        #region 重写 JsonConverter成员

        #region 重写方法 WriteJson
        /// <summary>
        /// WriteJson
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var JsonToken = JToken.FromObject(value);
            if (JsonToken.Type != JTokenType.Object)
            {
                JsonToken.WriteTo(writer);
            }
            else
            {
                var JsonObject = (JObject)JsonToken;
                IList<string> 属性名称列表 = JsonObject.Properties().Select(p => p.Name).ToList();
                JsonObject.AddFirst(new JProperty("Keys", new JArray(属性名称列表)));
                JsonObject.WriteTo(writer);
            }
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
            throw new NotImplementedException("不必要，因为CanRead是false。 类型将跳过转换器。");
        }
        #endregion

        #region 重写属性 CanRead
        /// <summary>
        /// CanRead
        /// </summary>
        public override bool CanRead
        {
            get { return false; }
        }
        #endregion

        #region 重写方法 CanConvert
        /// <summary>
        /// CanConvert
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }
        #endregion

        #endregion
    }
}