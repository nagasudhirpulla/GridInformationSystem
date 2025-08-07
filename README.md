Grid Elements database service for managing and querying Grid Elements database

## Status
### Defined Element Entities
* Element
* Bay
* Bus 
* BusReactor
* FilterBank 
* GeneratingUnit 
* HvdcLine 
* HvdcPole 
* Line 
* LineReactor 
* SubFilterBank 
* Transformer


### Pending Element Entities to be defined
* SVC
* FSC
* Compensator

### Implemented General Entities
* ElementOwner
* Fuel
* GeneratingStation
* GeneratingStationClassification
* GeneratingStationType
* Location
* Owner
* Region
* State
* Substation
* SubstationOwner
* VoltageLevel

### Implemented Entity Management UIs
* Buses
* Lines
* Locations
* Owners
* Regions
* States
* Substations
* VoltageLevels
* Bay
* Transformer
* Generating Unit
* GeneratingStationType
* Generating Station Classification
* Fuel
* Generating Station
* LineReactor
* BusReactor
* HvdcLine
* HvdcPole
* Filter Bank
* SubFilterBank
* Datasource
* Metric

## TODOs
* Measurements UI layer pending
* restrict substations changes in lines when there are connected elements like line reactors
* Define control area and attach it to element
* Model MSC, MSR, TCSC, STATCOM instead of compensator
* Model SVC, FSC
* whitelisted characters in names implementation during create and update commands
* lat long value object implementation
* lat long to be made optional

## References
* Clean architecture - https://github.com/jasontaylordev/CleanArchitecture/blob/a713468e27deb655eeb96b340318274eeccc5c3f/src/Infrastructure/Data/ApplicationDbContext.cs
* Many-to-Many in EF Core - https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#direct-use-of-join-table
* Transactions in EF Core - https://learn.microsoft.com/en-us/ef/core/saving/transactions
* multi select reference - https://github.com/nagasudhirpulla/wrldc_scada_issues_portal/blob/master/src/WrldcScadaIssuesPortal/ScadaIssuesPortal.Web/Views/ReportingCases/Create.cshtml
* Using database transaction in mediatr command pipeline - https://github.com/Jefferycheng/FlexibleTransactionHandler/blob/master/FlexibleTransactionHandler.WebApi/Application/Behaviors/TransactionBehavior.cs
* Smart Enum model binding - https://github.com/nagasudhirpulla/SmartEnumModelBinding/tree/main
* Query items from DB with pagination - https://github.com/jasontaylordev/CleanArchitecture/blob/main/src/Application/TodoItems/Queries/GetTodoItemsWithPagination/GetTodoItemsWithPagination.cs
* Use @functions to call dotnet function in razor page javascript callback - https://stackoverflow.com/questions/61846815/how-to-add-a-event-to-a-button-using-razor/61864703 , https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-9.0
* Dotnet QuickGrid for displaying tabular data