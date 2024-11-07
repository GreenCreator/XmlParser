using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

namespace XmlToDB.io.controller;

public class HtmlRenderer
{
    private readonly string _xsltPath;

    public HtmlRenderer(string xsltPath)
    {
        _xsltPath = xsltPath;
    }

    public void RenderHtml(Dictionary<string, string> attributes, WebBrowser webBrowser)
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

        string tempHtmlPath = Path.Combine(Path.GetTempPath(), "transformed.html");
        using (var writer = XmlWriter.Create(tempHtmlPath, xslt.OutputSettings))
        {
            xslt.Transform(xml, writer);
        }

        webBrowser.Navigate(tempHtmlPath);
    }
}