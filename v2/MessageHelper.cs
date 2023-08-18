namespace HL7parser.v2;
public static class MessageHelper
{
    public static List<string> SplitString(string strStringToSplit, string[] strSplitBy, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        return strStringToSplit.Split(strSplitBy, splitOptions).ToList();
    }

    public static List<string> SplitString(string strStringToSplit, char[] chSplitBy, StringSplitOptions splitOptions = StringSplitOptions.None)
    {
        return strStringToSplit.Split(chSplitBy, splitOptions).ToList();
    }
}