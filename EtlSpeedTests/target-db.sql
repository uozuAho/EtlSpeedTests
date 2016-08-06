/*
-- Uncomment this section to drop & recreate the target database

use master;
go

if exists (select * from sys.databases where name='EtlSpeedTests') drop database EtlSpeedTest;
create database EtlSpeedTests on
primary (
    name = EtlSpeedTests,
    filename = 'C:\temp\EtlSpeedTests.mdf',
    size = 100MB,
    filegrowth = 10%
)
log on (
    name = EtlSpeedTestsLog,
    filename = 'C:\temp\EtlSpeedTestsLog.ldf',
    size = 100MB,
    filegrowth = 10%
);
go

use EtlSpeedTests;
go
*/

IF OBJECT_ID('dbo.Individual', 'U') IS NOT NULL DROP TABLE dbo.Individual; 
create table Individual (
    Id [int] PRIMARY KEY CLUSTERED NOT NULL,
    Name nvarchar(35) not null,
    Sex nvarchar(1)
);
go

IF OBJECT_ID('dbo.Activity', 'U') IS NOT NULL DROP TABLE dbo.Activity; 
create table Activity(
    Id [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    Name nvarchar(20),
    HobbyId int
);
go

IF OBJECT_ID('dbo.IndividualActivity', 'U') IS NOT NULL DROP TABLE dbo.IndividualActivity; 
create table IndividualActivity(
    Id [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    IndividualId int foreign key references Individual(Id) NOT NULL,
    ActivityId int foreign key references Activity(Id) NOT NULL,
    constraint IndividualActivity_C_Unique unique (IndividualId, ActivityId)
);
go

IF OBJECT_ID('dbo.PropertyType', 'U') IS NOT NULL DROP TABLE dbo.PropertyType; 
create table PropertyType(
    Id [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    Value nvarchar(20)
);
go

IF OBJECT_ID('dbo.Property', 'U') IS NOT NULL DROP TABLE dbo.Property; 
create table Property(
    Id [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    PropertyTypeId [int] foreign key references PropertyType(Id) NOT NULL,
    IndividualId [int] foreign key references Individual(id),
    ActivityId int foreign key references Activity(Id),
    Value nvarchar(20)
);
go


/*****************************************************************************/
-- Initial data
insert into PropertyType ([Value]) values ('Address'), ('Ph.'), ('Hobby Name'), ('Hobby Id'), ('Hobby Type');



/*****************************************************************************/
-- EF ETL TABLES

IF OBJECT_ID('dbo.EfEtl_Person', 'U') IS NOT NULL DROP TABLE dbo.EfEtl_Person; 
create table EfEtl_Person (
    -- unique id
    RowId int identity(1,1) primary key clustered,

    -- data fields
    Id int,
    FirstName nvarchar(20),
    LastName nvarchar(20),
    Gender nvarchar(6),
    Address nvarchar(50),
    Ph nvarchar(10),
    HobbyId int,

    -- processing-related fields
    ProcessingState int not null default 0
);

IF OBJECT_ID('dbo.EfEtl_Hobby', 'U') IS NOT NULL DROP TABLE dbo.EfEtl_Hobby; 
create table EfEtl_Hobby (
    -- unique id
    RowId int identity(1,1) primary key clustered,

    -- data fields
    Id int,
    Name nvarchar(20),
    Type nvarchar(20),

    -- processing-related fields
    ProcessingState int not null default 0
);


/*
These tables aren't needed, but are probably going to be used by many solutions
as 'input' tables. Modify them to suit your ETL solution's needs.

create table Person (
    Id int,
    FirstName nvarchar(20),
    LastName nvarchar(20),
    Gender nvarchar(6),
    Address nvarchar(50),
    Ph nvarchar(10),
    HobbyId int
);
go

create table Hobby (
    Id int,
    Name nvarchar(20),
    Type nvarchar(20)
);
go

*/