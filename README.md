# MovieRating

### Structure
* MovieRating - main asp .net coe mvc project
* MovieRating.Dto - project with dtos
* MovieRating.Dal - project with migratons, dbcontext, models. etc..
* MoveRating.Test - Testing project

### Start
Install MS SqlServer Express and change connection string in appsettings.json

`dotnet run --project MovieRating`

You can use `--initfake` argument to add fake data into the db for testing purposes
