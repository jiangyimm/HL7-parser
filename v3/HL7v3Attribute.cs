namespace HL7parser.v3
{
    public class HL7v3Attribute : Attribute
    {
        public string XPath { get; }
        public bool IsRequired { get; }
        public MapType DestType { get; }
        public HL7v3Attribute(string xpath, bool isRequired, MapType destType)
        {
            XPath = xpath;
            IsRequired = isRequired;
            DestType = destType;
        }
        public HL7v3Attribute(string xpath, MapType destType)
        {
            XPath = xpath;
            IsRequired = true;
            DestType = MapType.TString;
        }
        public HL7v3Attribute(string xpath, bool isRequired)
        {
            XPath = xpath;
            IsRequired = isRequired;
            DestType = MapType.TString;
        }
        public HL7v3Attribute(string xpath)
        {
            XPath = xpath;
            IsRequired = true;
            DestType = MapType.TString;
        }
    }
}