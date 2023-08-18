namespace HL7parser.v2;
public class SubComponent
{
    private string _Value;

    public SubComponent()
    {
    }

    public SubComponent(string pValue)
    {
        _Value = pValue;
    }

    public string Value
    {
        get
        {
            if (_Value == null)
                return string.Empty;
            else
                return _Value;
        }
        set
        {
            _Value = value;

        }
    }

}