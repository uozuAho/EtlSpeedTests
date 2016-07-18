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