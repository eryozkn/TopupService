IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'TopupDb')
BEGIN
    CREATE DATABASE TopupDb
END

USE TopupDb
GO

IF NOT EXISTS (SELECT 1 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Beneficiaries')
BEGIN
    CREATE TABLE dbo.Beneficiaries (
            [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
            [UserId] BIGINT NOT NULL,
            [NickName] NVARCHAR(20) NOT NULL,
            [PhoneNumber] NVARCHAR(15) NOT NULL,
            [IsActive] BIT NOT NULL,
            [CreatedAt] DATETIMEOFFSET NULL,
            [UpdatedAt] DATETIMEOFFSET NULL
        );

        -- Seed data 

        INSERT INTO dbo.Beneficiaries (UserId, NickName, PhoneNumber, IsActive, CreatedAt, UpdatedAt) 
        VALUES (1, 'Eray Dubai Number', '+971521503931', 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());

        INSERT INTO dbo.Beneficiaries (UserId, NickName, PhoneNumber, IsActive, CreatedAt, UpdatedAt) 
        VALUES (1, 'Eray Etisalat', '+971521503932', 1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());
END 

IF NOT EXISTS (SELECT 1 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'TopupHistory')
BEGIN
    CREATE TABLE dbo.TopupHistory (
            [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
            [UserId] BIGINT NOT NULL,
            [BeneficiaryId] BIGINT NOT NULL,
            [TopupAmount] DECIMAL(18,2) NOT NULL,
            [CreatedAt] DATETIMEOFFSET NULL
        );

    ALTER TABLE dbo.TopupHistory
    ADD CONSTRAINT FK_TopupHistory_BeneficiaryId
    FOREIGN KEY (BeneficiaryId)
    REFERENCES dbo.Beneficiaries (Id);

END