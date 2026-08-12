/*
================================================================================
KEIZA_TRAINING Database Setup Script
================================================================================
Description: Creates database, tables, and inserts test data for training purposes
Created: 2026-08-03
Tables: dbo.asnaf_profile, dbo.asnaf_application
================================================================================
*/

-- ============================================================================
-- 1. CREATE DATABASE
-- ============================================================================
BEGIN TRY
    IF DB_ID(N'keiza_training') IS NULL
        CREATE DATABASE [keiza_training];
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_MESSAGE() AS ErrorMessage;
END CATCH;
GO

USE [keiza_training];
GO

-- ============================================================================
-- 2. CREATE TABLE: dbo.asnaf_profile
-- ============================================================================
-- Master table for asnaf (beneficiary) profiles
-- PK: asnaf_uid (BIGINT IDENTITY auto-increment)
-- Columns: personal info, address, contact, audit fields
-- ============================================================================
BEGIN TRY
    IF OBJECT_ID(N'dbo.asnaf_profile', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.asnaf_profile
        (
            asnaf_uid BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
            asnaf_name VARCHAR(200) NULL,
            asnaf_icno VARCHAR(50) NULL,
            res_address1 VARCHAR(200) NULL,
            res_address2 VARCHAR(200) NULL,
            res_address3 VARCHAR(200) NULL,
            postcode VARCHAR(20) NULL,
            district VARCHAR(100) NULL,
            state VARCHAR(100) NULL,
            email VARCHAR(200) NULL,
            contactno VARCHAR(50) NULL,
            asnaf_status VARCHAR(50) NULL,
            createdby VARCHAR(100) NULL,
            createddate DATETIME NULL,
            modifiedby VARCHAR(100) NULL,
            modifieddate DATETIME NULL,
            confirmedby VARCHAR(100) NULL,
            confirmeddate DATETIME NULL
        );
        PRINT 'Table dbo.asnaf_profile created successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Table dbo.asnaf_profile already exists.';
    END
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_MESSAGE() AS ErrorMessage;
END CATCH;
GO

-- ============================================================================
-- 3. CREATE TABLE: dbo.asnaf_application
-- ============================================================================
-- Application/request table linked to asnaf_profile
-- PK: app_uid (BIGINT IDENTITY auto-increment)
-- FK: asnaf_uid -> dbo.asnaf_profile(asnaf_uid)
-- Columns: application details, workflow fields, audit fields
-- ============================================================================
BEGIN TRY
    IF OBJECT_ID(N'dbo.asnaf_application', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.asnaf_application
        (
            app_uid BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
            asnaf_uid BIGINT NOT NULL,
            application_no VARCHAR(50) NULL,
            application_date DATETIME NOT NULL CONSTRAINT DF_asnaf_application_application_date DEFAULT (GETDATE()),
            submitted_date DATETIME NULL,
            application_status VARCHAR(50) NULL,
            amount DECIMAL(18,2) NULL,
            currency CHAR(3) NULL,
            remarks VARCHAR(500) NULL,
            submitted_by VARCHAR(100) NULL,
            reviewed_by VARCHAR(100) NULL,
            reviewed_date DATETIME NULL,
            confirmeddate DATETIME NULL,
            createdby VARCHAR(100) NULL,
            createddate DATETIME NOT NULL CONSTRAINT DF_asnaf_application_createddate DEFAULT (GETDATE()),
            modifiedby VARCHAR(100) NULL,
            modifieddate DATETIME NULL,
            CONSTRAINT FK_asnaf_application_profile FOREIGN KEY (asnaf_uid) REFERENCES dbo.asnaf_profile(asnaf_uid)
        );

        -- Index on FK for performance
        CREATE INDEX IX_asnaf_application_asnaf_uid ON dbo.asnaf_application(asnaf_uid);

        PRINT 'Table dbo.asnaf_application created successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Table dbo.asnaf_application already exists.';
    END
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_MESSAGE() AS ErrorMessage;
END CATCH;
GO

-- ============================================================================
-- 4. INSERT TEST DATA
-- ============================================================================
-- Inserts sample profiles and related applications for testing
-- Uses transactions for rollback safety
-- ============================================================================
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRAN;

    -- Capture new profile IDs
    DECLARE @ProfileIds TABLE (seq INT IDENTITY(1,1), asnaf_uid BIGINT);

    -- Insert test profiles
    INSERT INTO dbo.asnaf_profile
        (asnaf_name, asnaf_icno, res_address1, res_address2, res_address3, postcode, district, state, email, contactno, asnaf_status, createdby, createddate)
    VALUES
        ('Ali Bin Abu', '900101-01-1234', 'No. 1 Jalan A', NULL, NULL, '50000', 'Kuala Lumpur', 'Kuala Lumpur', 'ali@example.com', '0123456789', 'Active', 'seeder', GETDATE());
    INSERT INTO @ProfileIds(asnaf_uid) VALUES (SCOPE_IDENTITY());

    INSERT INTO dbo.asnaf_profile
        (asnaf_name, asnaf_icno, res_address1, res_address2, res_address3, postcode, district, state, email, contactno, asnaf_status, createdby, createddate)
    VALUES
        ('Siti Binti Ahmad', '880202-02-2345', 'No. 2 Jalan B', 'Taman B', NULL, '43000', 'Kajang', 'Selangor', 'siti@example.com', '0139876543', 'Active', 'seeder', GETDATE());
    INSERT INTO @ProfileIds(asnaf_uid) VALUES (SCOPE_IDENTITY());

    INSERT INTO dbo.asnaf_profile
        (asnaf_name, asnaf_icno, res_address1, res_address2, res_address3, postcode, district, state, email, contactno, asnaf_status, createdby, createddate)
    VALUES
        ('Muhammad Rahim', '750303-03-3456', 'No. 3 Jalan C', NULL, NULL, '30000', 'Ipoh', 'Perak', 'rahim@example.com', '0145556666', 'Pending', 'seeder', GETDATE());
    INSERT INTO @ProfileIds(asnaf_uid) VALUES (SCOPE_IDENTITY());

    -- Insert related applications
    DECLARE @InsertedApps TABLE (app_uid BIGINT, asnaf_uid BIGINT);

    INSERT INTO dbo.asnaf_application
        (asnaf_uid, application_no, application_date, submitted_date, application_status, amount, currency, remarks, submitted_by, reviewed_by, createdby, createddate)
    OUTPUT inserted.app_uid, inserted.asnaf_uid INTO @InsertedApps(app_uid, asnaf_uid)
    SELECT
        p.asnaf_uid,
        'APP-' + RIGHT('000' + CAST(p.seq AS VARCHAR(3)), 3),
        GETDATE(),
        DATEADD(day, -1, GETDATE()),
        CASE WHEN p.seq = 3 THEN 'Draft' ELSE 'Submitted' END,
        CASE WHEN p.seq = 1 THEN 150.00 WHEN p.seq = 2 THEN 250.00 ELSE 100.00 END,
        'MYR',
        'Test application for ' + CAST(p.asnaf_uid AS VARCHAR(20)),
        'seeder',
        NULL,
        'seeder',
        GETDATE()
    FROM @ProfileIds p;

    -- Display inserted records
    SELECT 'Inserted profiles' AS [Info], * FROM @ProfileIds;
    SELECT 'Inserted applications' AS [Info], * FROM @InsertedApps;

    COMMIT;
    PRINT 'Test data inserted successfully.';
END TRY
BEGIN CATCH
    ROLLBACK;
    SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_MESSAGE() AS ErrorMessage;
END CATCH;
GO

-- ============================================================================
-- 5. VERIFICATION QUERIES
-- ============================================================================
-- Run these to verify the setup
-- ============================================================================

-- Check database exists
SELECT name FROM sys.databases WHERE name = 'keiza_training';

-- Check tables exist
SELECT TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN ('asnaf_profile', 'asnaf_application');

-- View all profiles
SELECT * FROM dbo.asnaf_profile;

-- View all applications with profile names
SELECT 
    app.app_uid,
    app.application_no,
    prof.asnaf_name,
    prof.asnaf_icno,
    app.application_status,
    app.amount,
    app.currency,
    app.submitted_date
FROM dbo.asnaf_application app
INNER JOIN dbo.asnaf_profile prof ON app.asnaf_uid = prof.asnaf_uid;

-- ============================================================================
-- END OF SCRIPT
-- ============================================================================