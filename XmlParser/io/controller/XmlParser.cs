using System;
using System.Collections.Generic;
using System.Xml;

namespace Xml.io.controller;

public class XmlParser
{
    public Dictionary<string, string> ExtractAttributes(string xmlContent)
    {
        var attributes = new Dictionary<string, string>();
        XmlDocument doc = new XmlDocument();

        try
        {
            doc.LoadXml(xmlContent);
            attributes["FirstName"] = doc.SelectSingleNode("Data/CardDocument/MainInfo/@FirstName")?.Value;
            attributes["Performer"] =
                doc.SelectSingleNode("Data/CardDocument/Performers/PerformersRow/@Performer")?.Value;
            attributes["RegDate"] = doc.SelectSingleNode("Data/CardDocument/MainInfo/@RegDate")?.Value;
            attributes["Content"] = doc.SelectSingleNode("Data/CardDocument/MainInfo/@Content")?.Value;
            attributes["Kind_Name"] = doc.SelectSingleNode("Data/CardDocument/System/@Kind_Name")?.Value;
            attributes["ReferenceList"] = doc.SelectSingleNode("Data/CardDocument/MainInfo/@ReferenceList")?.Value;
            attributes["Author"] = doc.SelectSingleNode("Data/CardDocument/MainInfo/@Author")?.Value;

            var performerId = attributes["Performer"];
            if (!string.IsNullOrEmpty(performerId))
            {
                var positionNode =
                    doc.SelectSingleNode(
                        $"Data/RefStaff/Units/UnitsRow/UnitsRow/UnitsRow/Employees/EmployeesRow[@RowID='{performerId}']");
                if (positionNode != null)
                {
                    attributes["PerformerPosition"] = positionNode.Attributes["PositionName"]?.Value;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при обработке XML: " + ex.Message);
        }

        return attributes;
    }
}