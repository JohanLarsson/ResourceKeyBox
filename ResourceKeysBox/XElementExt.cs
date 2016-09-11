namespace ResourceKeysBox
{
    using System.Linq;
    using System.Xml.Linq;

    internal static class XElementExt
    {
        internal static string AttributeValue(this XElement e, string attributeName)
        {
            return e.Attributes().SingleOrDefault(x => x.Name.LocalName == attributeName)?.Value;
        }
    }
}