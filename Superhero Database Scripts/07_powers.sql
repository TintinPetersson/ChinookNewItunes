USE SuperheroesDb

INSERT INTO Power (PowerName, Description)
VALUES 
    ('Super Strength', 'Ability to lift heavy objects and cause damage with physical force.'),
    ('Flight', 'Ability to fly or soar through the air.'),
	('Indestructible', 'Ability to withstand almost anything.'),
    ('Wealth', 'Ability to buy everything and everyone.');

INSERT INTO Superhero_Power (Superhero_ID, Power_ID)
VALUES
    (1, 1), 
    (1, 2), 
	(1, 3),
	(2, 4),
	(2, 2),
	(3, 1),
	(3, 3);