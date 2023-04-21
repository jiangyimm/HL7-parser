using System.Text;
using System.Xml;

namespace HL7parser.v3
{
    public static class XmlUtil
    {
        public static XmlNode SelectSingleNodeExt(this XmlElement xmlElement, string xpath, string ns, XmlNamespaceManager nsmgr)
        {
            var path = GetXmlPath(xpath, ns);
            return xmlElement.SelectSingleNode(path, nsmgr);
        }

        public static XmlNodeList SelectNodesExt(this XmlElement xmlElement, string xpath, string ns, XmlNamespaceManager nsmgr)
        {
            var path = GetXmlPath(xpath, ns);
            return xmlElement.SelectNodes(path, nsmgr);
        }

        private static string GetXmlPath(string xmlPath, string ns)
        {
            var arr = xmlPath.Split('/');
            var sb = new StringBuilder();
            sb.Append("/");
            foreach (var s in arr)
            {
                if (string.IsNullOrWhiteSpace(s))
                    continue;
                sb.Append("/");
                if (!s.StartsWith("@"))
                    sb.Append(ns + ":");
                sb.Append(s);
            }
            return sb.ToString();
        }
    }
}