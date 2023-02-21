USE SuperheroesDb

ALTER TABLE Assistant
ADD SuperheroID int CONSTRAINT FK_Assistant_Superhero  FOREIGN KEY REFERENCES Superhero(ID); 