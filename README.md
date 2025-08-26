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
* Measurement

## TODOs
* Create DB trigger type of notification when an element changes so that policies can be checked and caches can be updated with in a transaction.
For example, if a substation name changes, a notification can be issued to update all related elements names with in a trasaction so that atomicity can be implemented.
* restrict substations changes in lines when there are connected elements like line reactors
* Define control area and attach it to element
* Model MSC, MSR, TCSC, STATCOM instead of compensator
* Model SVC, FSC
* whitelisted characters in names implementation during create and update commands
* lat long value object implementation
* lat long to be made optional

## How to handle domain events
* All base entities have domain events (`List<BaseEvent> _domainEvents`) that can be dispatched while saving the entity changes
* Multiple Events are defined that are derived from BaseEvent ([example](https://github.com/jasontaylordev/CleanArchitecture/blob/b46a41b20059316e897b9a77aa277be7d42cb974/src/Domain/Events/TodoItemCreatedEvent.cs#L4)). These events can be dispatched from the mediatr command handlers ([example](https://github.com/jasontaylordev/CleanArchitecture/blob/b46a41b20059316e897b9a77aa277be7d42cb974/src/Application/TodoItems/Commands/CreateTodoItem/CreateTodoItem.cs#L32))
* Event handlers are defined as INotificationHandler<EventType> ([example](https://github.com/jasontaylordev/CleanArchitecture/blob/b46a41b20059316e897b9a77aa277be7d42cb974/src/Application/TodoItems/EventHandlers/TodoItemCreatedEventHandler.cs#L6))
* Before persisting the entity changes into the database, domain events are dispatched (using mediatr.Publish) first and then entity changes are committed ([example](https://github.com/jasontaylordev/CleanArchitecture/blob/b46a41b20059316e897b9a77aa277be7d42cb974/src/Infrastructure/Data/Interceptors/DispatchDomainEventsInterceptor.cs#L8)). But for scenarios like updating cache columns, event handlers should run after entity update. Both entity update and domain events should run atomically in a transaction ([example](https://github.com/nagasudhirpulla/wrldc_codes_mgmt/blob/2f44fd53e9863d808949e9023820fcec89574e03/src/Infra/Persistence/AppDbContext.cs#L60))

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