IF OBJECT_ID('dbo.EfEtl_Person', 'U') IS NOT NULL DROP TABLE dbo.EfEtl_Person; 
create table EfEtl_Person (
    Id int primary key clustered,
    FirstName nvarchar(20),
    LastName nvarchar(20),
    Gender nvarchar(6),
    Address nvarchar(50),
    Ph nvarchar(10),
    HobbyId int
);

IF OBJECT_ID('dbo.EfEtl_Hobby', 'U') IS NOT NULL DROP TABLE dbo.EfEtl_Hobby; 
create table EfEtl_Hobby (
    Id int primary key clustered,
    Name nvarchar(20),
    Type nvarchar(20)
);