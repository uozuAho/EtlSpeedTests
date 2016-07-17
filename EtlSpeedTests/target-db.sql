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

create table Individual (
    Id [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    Name nvarchar(50) not null,
    Sex nvarchar(1)
);
go

create table Activity(
    Id [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    Name nvarchar(20),
    HobbyId int
);
go

create table IndividualActivity(
    Id [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    IndividualId int foreign key references Individual(Id),
    ActivityId int foreign key references Activity(Id)
);
go

create table PropertyType(
    Id [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    Name nvarchar(20)
);
go

create table Property(
    Id [int] IDENTITY(1,1) PRIMARY KEY CLUSTERED NOT NULL,
    PropertyTypeId int foreign key references PropertyType(Id),
    Value nvarchar(20)
);
go

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