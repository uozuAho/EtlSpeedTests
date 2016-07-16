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
- Id (int, PK)     --> Individual property
- FirstName        --> Individual.Name (concat with lastname)
- LastName         --> Individual.Name (concat with firstname)
- Gender (null)    --> Individual.Sex
    + Accepted values: M, F, Male, Female
    + Import if invalid
- Address (null)   --> Individual property
- Ph. (null)       --> Individual property
- Hobby Id (null)  --> Create IndividualActivity link, also add hobby name to Individual property

Hobby
- Id (int, PK)     --> Activity.HobbyId, also Activity property
- Name             --> Activity.Name
- Type (null)      --> Activity property

Rules
- Person must have at least one name field populated. Don't import if invalid.
- Hobby Id must link to a hobby in the hobby input file. Import if invalid.

## Destination tables

Individual
- Id (int, PK)
- Name
- Sex

Activity
- Id (int, PK)
- HobbyId
- Name

IndividualActivity
- Id (int, PK)
- IndividualId
- ActivityId

Property
- Id (int, PK)
- PropertyTypeId
- Value

PropertyType
- Id (int, PK)
- Name