# ETL Speed Tests

# Goals

Find fastest way to load data into a database, while maintaining flexibility
to transform & verify loaded data.

# Requirements

Input data:
- multiple sources with related fields (eg. foreign keys)

Must be able to:
- accept/reject individual rows of data based on verification criteria
- track rejected rows (log file, table etc.)
- transform individual rows based on business rules
- continue despite row errors
- track row errors (log file, table etc.)
- resume after application error / unexpected halt
- there's probably other nice things that commercial ETL tools can do. Do some of them!


--------------------------------------------------------------------------------
# Data & business rules

## Input files, mapping to destination tables & rules

Person
- Id (int)                  --> Individual.Id
- FirstName (nvarchar(20))  --> Individual.Name (concat with lastname)
- LastName (nvarchar(20))   --> Individual.Name (concat with firstname)
- Gender (nvarchar(6))      --> Individual.Sex
    + Accepted values: M, F, Male, Female
    + Import if invalid
- Address (nvarchar(50))    --> Individual property
- Ph. (nvarchar(10))        --> Individual property, must be numeric
- Hobby Id (int)            --> Create IndividualActivity link, also add hobby name to Individual property

Hobby
- Id (int, PK)              --> Activity.HobbyId, also Activity property
- Name (nvarchar(20))       --> Activity.Name
- Type (nvarchar(20))       --> Activity property

Rules
- Person must have at least one name field populated. Don't import if invalid.
- Person can be added to multiple hobbies by multiple lines in the person file,
  containing only person id and hobby id
- Hobby Id must link to a hobby in the hobby input file. Import if invalid.

## Destination tables

Individual
- Id     int, PK
- Name   nvarchar(35) not null
- Sex    nvarchar(1)

Activity
- Id       int, PK
- HobbyId
- Name     nvarchar(20)

IndividualActivity
- Id (int, PK)
- IndividualId
- ActivityId

PropertyType
- Id
- Value

Property
- Id             int, PK
- PropertyTypeId
- IndividualId
- ActivityId
- Value          nvarchar(20)