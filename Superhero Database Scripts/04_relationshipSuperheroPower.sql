USE SuperheroesDb

CREATE TABLE Superhero_Power (
	Superhero_ID int,
	Power_ID int,
	PRIMARY KEY (Superhero_ID, Power_ID),
	FOREIGN KEY	(Superhero_ID) REFERENCES Superhero(ID),
	FOREIGN KEY	(Power_ID) REFERENCES Power(ID),
)