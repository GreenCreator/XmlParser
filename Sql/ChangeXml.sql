PREPARE update_xml AS
    WITH updated AS (
    SELECT
    Id,
    xmlparse(document REPLACE(
    xpath($1, FileContent)::text,
    CONCAT($2, '="', $3, '"')
    )) AS new_xml
    FROM Files
    WHERE Id = $4
    )
UPDATE Files
SET FileContent = (SELECT new_xml FROM updated WHERE Files.Id = updated.Id)
WHERE Id = $4;
