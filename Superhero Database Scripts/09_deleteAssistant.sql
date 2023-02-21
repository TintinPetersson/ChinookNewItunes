USE SuperheroesDb

DECLARE @AssistantName VARCHAR(50) = 'Robin';

DELETE FROM Assistant
WHERE AssistantName = @AssistantName;