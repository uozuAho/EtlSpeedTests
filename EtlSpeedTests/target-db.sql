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