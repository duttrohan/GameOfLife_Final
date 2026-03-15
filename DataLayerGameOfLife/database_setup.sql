-- 1. Create the Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'GameOfLifeDB')
BEGIN
    CREATE DATABASE GameOfLifeDB;
END
GO

USE GameOfLifeDB;
GO

-- 2. Create the table that stores our cell coordinates
-- We added 'PatternName' so you can save different designs!
IF OBJECT_ID('Cells', 'U') IS NOT NULL
    DROP TABLE Cells;
GO

CREATE TABLE Cells (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    X INT NOT NULL,
    Y INT NOT NULL,
    PatternName NVARCHAR(50) NOT NULL DEFAULT 'Default'
);
GO