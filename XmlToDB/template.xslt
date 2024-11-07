<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:template match="/">
        <html>
            <body>
                <h2>Сведения о сотруднике</h2>
                <table border="1">
                    <tr><th>Имя</th><td><xsl:value-of select="/Root/FirstName"/></td></tr>
                    <tr><th>Должность</th><td><xsl:value-of select="/Root/PerformerPosition"/></td></tr>
                    <tr><th>Дата регистрации</th><td><xsl:value-of select="/Root/RegDate"/></td></tr>
                    <tr><th>Тема</th><td><xsl:value-of select="/Root/Content"/></td></tr>
                    <tr><th>Вид</th><td><xsl:value-of select="/Root/Kind_Name"/></td></tr>
                    <tr><th>Ссылка</th><td><xsl:value-of select="/Root/ReferenceList"/></td></tr>
                    <tr><th>ИД Автора</th><td><xsl:value-of select="/Root/Author"/></td></tr>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>
