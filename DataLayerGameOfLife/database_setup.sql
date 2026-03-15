-- 1. Create the Database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'GameOfLifeDB')
BEGIN
    CREATE DATABASE GameOfLifeDB;
END
GO

USE GameOfLifeDB;
GO

-- 2. Clean up old tables
IF OBJECT_ID('Cells', 'U') IS NOT NULL DROP TABLE Cells;
IF OBJECT_ID('InitialStates', 'U') IS NOT NULL DROP TABLE InitialStates;
GO

-- 3. Create the table exactly as requested in the Exam PDF
CREATE TABLE InitialStates (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL, -- This is your Pattern Name
    X INT NOT NULL,
    Y INT NOT NULL
);
GO