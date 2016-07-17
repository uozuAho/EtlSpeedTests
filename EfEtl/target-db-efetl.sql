create table EfEtl_Person (
    Id int,
    FirstName nvarchar(20),
    LastName nvarchar(20),
    Gender nvarchar(6),
    Address nvarchar(50),
    Ph. nvarchar(10),
    Hobby Id int
);
go

create table EfEtl_Hobby (
    Id int,
    Name nvarchar(20),
    Type nvarchar(20)
);
go