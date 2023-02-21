USE SuperheroesDb

CREATE TABLE Superhero (
	ID int PRIMARY KEY IDENTITY(1,1),
	HeroName nvarchar(50) null,
	Alias nvarchar(50) null,
	Origin nvarchar(50) null
);

CREATE TABLE Assistant (
	ID int PRIMARY KEY IDENTITY(1,1),
	AssistantName nvarchar(50) null UNIQUE,
);

CREATE TABLE Power (
	ID int PRIMARY KEY IDENTITY(1,1),
	PowerName nvarchar(50) null,
	Description nvarchar(100) null,
)

