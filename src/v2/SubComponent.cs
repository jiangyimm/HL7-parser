namespace HL7parser.v2
{
    public class SubComponent
    {
        private string _value;
        public SubComponent(string pValue)
        {
            _value = pValue ?? string.Empty;
        }

        public void SetValue(string pValue)
        {
            _value = pValue ?? string.Empty;
        }

        public string Value => _value;
    }
}