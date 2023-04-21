namespace HL7parser.v3
{
    public class HL7v3Mapper
    {
        private IList<MapModel> _mappers;

        public IList<MapModel> Mappers => _mappers;

        public HL7v3Mapper()
        {
            _mappers = new List<MapModel>();
        }

        public HL7v3Mapper Add(string fieldName, string xmlPath, bool isRequired, MapType destType)
        {
            _mappers.Add(new MapModel(fieldName, xmlPath, isRequired, destType));
            return this;
        }
        /// <summary>
        /// 默认类型为string
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="xmlPath"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public HL7v3Mapper Add(string fieldName, string xmlPath, bool isRequired)
        {
            this.Add(fieldName, xmlPath, isRequired, destType: MapType.TString);
            return this;
        }

        /// <summary>
        /// 默认必须
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="xmlPath"></param>
        /// <param name="destType"></param>
        /// <returns></returns>
        public HL7v3Mapper Add(string fieldName, string xmlPath, MapType destType)
        {
            this.Add(fieldName, xmlPath, isRequired: true, destType);
            return this;
        }
        /// <summary>
        /// 默认必须，默认类型为string
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public HL7v3Mapper Add(string fieldName, string xmlPath)
        {
            this.Add(fieldName, xmlPath, isRequired: true, destType: MapType.TString);
            return this;
        }
    }

    public struct MapModel
    {
        public MapModel(string fieldName, string xmlPath, bool isRequired, MapType destType)
        {
            FieldName = fieldName;
            XmlPath = xmlPath;
            IsRequired = isRequired;
            DestType = destType;
        }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; }
        /// <summary>
        /// 在XML中的路径
        /// </summary>
        public string XmlPath { get; }
        /// <summary>
        /// 是否必须（节点必要且值不可空）
        /// </summary>
        public bool IsRequired { get; }
        /// <summary>
        /// 目标数据类型
        /// </summary>
        public MapType DestType { get; }
    }

    public enum MapType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        TString,
        /// <summary>
        /// 数值
        /// </summary>
        TDecimal,
        /// <summary>
        /// 时间格式 20181213080000
        /// </summary>
        TTime,
        /// <summary>
        /// 日期格式 20181213
        /// </summary>
        TDate
    }
}