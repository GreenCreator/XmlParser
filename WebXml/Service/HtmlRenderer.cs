using System.Xml;
using System.Xml.Xsl;

namespace Xml.io.controller;

public class HtmlRenderer
{
    private readonly string _xsltPath;

    public HtmlRenderer(string xsltPath)
    {
        _xsltPath = xsltPath;
    }

    public string RenderHtml(Dictionary<string, string> attributes)
    {
        var xml = new XmlDocument();
        var root = xml.CreateElement("Root");
        xml.AppendChild(root);

        foreach (var attr in attributes)
        {
            var elem = xml.CreateElement(attr.Key);
            elem.InnerText = attr.Value;
            root.AppendChild(elem);
        }

        var xslt = new XslCompiledTransform();
        xslt.Load(_xsltPath);

        using (var stringWriter = new StringWriter())
        using (var xmlWriter = XmlWriter.Create(stringWriter, xslt.OutputSettings))
        {
            xslt.Transform(xml, xmlWriter);
            return stringWriter.ToString();
        }
    }
}