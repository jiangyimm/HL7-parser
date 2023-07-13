using System.Xml;

namespace HL7parser.v3
{
    /// <summary>
    /// HL7消息解析基类
    /// Version：v3
    /// Content-Type：XML
    /// </summary>
    public class HL7v3Parser
    {
        /// <summary>
        /// xml对象
        /// </summary>
        private XmlDocument _xmlDocument;

        /// <summary>
        /// xml命名空间
        /// </summary>
        private XmlNamespaceManager _xmlNamespaceManager;

        /// <summary>
        /// 初始化并返回请求消息ID
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <returns></returns>
        public virtual string InitXmlDocument(string xml)
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.LoadXml(xml);
            var documentElement = _xmlDocument.DocumentElement;
            _xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            _xmlNamespaceManager.AddNamespace("x", _xmlDocument.DocumentElement.NamespaceURI);

            return this.GetSingleValue("/id/@extension");
        }

        /// <summary>
        /// 根据mappers将节点值赋值给obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="mappers"></param>
        protected virtual void DoMapper<T>(T obj, Dictionary<string, string> mappers) where T : class
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                foreach (var mapper in mappers)
                {
                    if (property.Name != mapper.Key)
                        continue;
                    var node = _xmlDocument.DocumentElement.SelectSingleNodeExt(mapper.Value, "x", _xmlNamespaceManager);
                    if (node == null)
                    {
                        throw new KeyNotFoundException($"xml节点不存在：{mapper.Key}");
                    }
                    var value = node.InnerText;
                    property.SetValue(obj, value);
                }
            }
        }

        /// <summary>
        /// 根据mappers将节点值赋值给obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="hl7v3mapper"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public virtual void DoMapper<T>(T obj, HL7v3Mapper hl7v3mapper) where T : class
        {
            var properties = obj.GetType().GetProperties();
            var mappers = hl7v3mapper.Mappers;
            foreach (var property in properties)
            {
                foreach (var mapper in mappers)
                {
                    if (property.Name != mapper.FieldName)
                        continue;
                    var node = _xmlDocument.DocumentElement.SelectSingleNodeExt(mapper.XmlPath, "x", _xmlNamespaceManager);
                    if (mapper.IsRequired && node == null)
                    {
                        throw new KeyNotFoundException($"xml节点不存在：{mapper.FieldName}");
                    }
                    var value = node?.InnerText;
                    if (mapper.IsRequired && string.IsNullOrWhiteSpace(value))
                    {
                        throw new KeyNotFoundException($"xml节点内容为空：{mapper.FieldName}");
                    }
                    if (mapper.Handler != null)
                    {
                        value = mapper.Handler.Invoke(value);
                    }
                    switch (mapper.DestType)
                    {
                        case MapType.TString:
                            property.SetValue(obj, value);
                            break;
                        case MapType.TDecimal:
                            decimal? dv = string.IsNullOrWhiteSpace(value) ? null : decimal.Parse(value);
                            property.SetValue(obj, dv);
                            break;
                        case MapType.TTime:
                            DateTime? tv = string.IsNullOrWhiteSpace(value) ? null : GetTime(value);
                            property.SetValue(obj, tv);
                            break;
                        case MapType.TDate:
                            DateTime? dtv = string.IsNullOrWhiteSpace(value) ? null : GetDate(value);
                            property.SetValue(obj, dtv);
                            break;
                        default:
                            property.SetValue(obj, value);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 根据mappers将节点值赋值给obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public virtual void DoMapper<T>(T obj) where T : class
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var fieldName = property.Name;
                var mapper = Attribute.GetCustomAttribute(property, typeof(HL7v3Attribute)) as HL7v3Attribute;
                if (mapper == null)
                {
                    continue;
                }
                var node = _xmlDocument.DocumentElement.SelectSingleNodeExt(mapper.XPath, "x", _xmlNamespaceManager);
                if (mapper.IsRequired && node == null)
                {
                    throw new KeyNotFoundException($"xml节点不存在：{fieldName}");
                }
                var value = node?.InnerText;
                if (mapper.IsRequired && string.IsNullOrWhiteSpace(value))
                {
                    throw new KeyNotFoundException($"xml节点内容为空：{fieldName}");
                }
                switch (mapper.DestType)
                {
                    case MapType.TString:
                        property.SetValue(obj, value);
                        break;
                    case MapType.TDecimal:
                        decimal? dv = string.IsNullOrWhiteSpace(value) ? null : decimal.Parse(value);
                        property.SetValue(obj, dv);
                        break;
                    case MapType.TTime:
                        DateTime? tv = string.IsNullOrWhiteSpace(value) ? null : GetTime(value);
                        property.SetValue(obj, tv);
                        break;
                    case MapType.TDate:
                        DateTime? dtv = string.IsNullOrWhiteSpace(value) ? null : GetDate(value);
                        property.SetValue(obj, dtv);
                        break;
                    default:
                        property.SetValue(obj, value);
                        break;
                }
            }
        }

        /// <summary>
        /// 获取单个值-字符串
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="isEnableNull"></param>
        /// <returns></returns>
        public virtual string GetSingleValue(string xpath, bool isEnableNull = false)
        {
            var xmlNode = _xmlDocument.DocumentElement.SelectSingleNodeExt(xpath, "x", _xmlNamespaceManager);
            if (!isEnableNull && xmlNode == null)
            {
                throw new KeyNotFoundException($"xml节点不存在：{xpath}");
            }
            return isEnableNull ? xmlNode?.InnerText : xmlNode.InnerText;
        }

        /// <summary>
        /// 获取单个值-数字格式
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="isEnableNull"></param>
        /// <returns></returns>
        public virtual decimal? GetSingleDecimalValue(string xpath, bool isEnableNull = false)
        {
            var value = GetSingleValue(xpath, isEnableNull);
            return string.IsNullOrWhiteSpace(value) ? null : decimal.Parse(value);
        }

        /// <summary>
        /// 获取单个值-时间格式
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="isEnableNull"></param>
        /// <returns></returns>
        public virtual DateTime? GetSingleTimeValue(string xpath, bool isEnableNull = false)
        {
            var value = GetSingleValue(xpath, isEnableNull);
            return string.IsNullOrWhiteSpace(value) ? null : GetTime(value);
        }

        /// <summary>
        /// 获取单个值-日期格式
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="isEnableNull"></param>
        /// <returns></returns>
        public virtual DateTime? GetSingleDateValue(string xpath, bool isEnableNull = false)
        {
            var value = GetSingleValue(xpath, isEnableNull);
            return string.IsNullOrWhiteSpace(value) ? null : GetDate(value);
        }

        /// <summary>
        /// 获取多个Node
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public virtual XmlNodeList GetNodeList(string xpath)
        {
            return _xmlDocument.DocumentElement.SelectNodesExt(xpath, "x", _xmlNamespaceManager);
        }

        /// <summary>
        /// 字符串转时间
        /// </summary>
        /// <param name="time">20181213080000</param>
        /// <returns></returns>
        public DateTime GetTime(string time)
        {
            time = $"{time.Substring(0, 4)}-{time.Substring(4, 2)}-{time.Substring(6, 2)} {time.Substring(8, 2)}:{time.Substring(10, 2)}:{time.Substring(12, 2)}";
            return DateTime.Parse(time);
        }

        /// <summary>
        /// 字符串转日期
        /// </summary>
        /// <param name="time">20181213</param>
        /// <returns></returns>
        public DateTime GetDate(string time)
        {
            time = $"{time.Substring(0, 4)}-{time.Substring(4, 2)}-{time.Substring(6, 2)}";
            return DateTime.Parse(time);
        }
    }
}