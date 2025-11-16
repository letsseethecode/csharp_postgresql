# CheeseShop


# Project Structure

## LSTC.CheeseShop.Api

The WebAPI that this project is delivering.

## LSTC.Shared

Shared library that defines the core `CQS` and `Data` types used by the project.

* `./Data`
  * `IRepository`
  * `PostgresqlRepository`
* `./CQS`
  * `./Commands`
    * `ICommand` - an empty interface that is used by the generic logic to identify commands.
    * `ICommandProcessor` - An interface capable of processing a command. There must be a single command processor registered.
    * `ICommandResolver` - An interface capable of locating an `ICommandProcessor` for a given `ICommand`
  * `./Queries`
    * `IQuery` - an empty interface that is used by the generic logic to identify queries.
    * `IQueryProcessor` - An interface capable of processing a query. There must be a single query processor registered.
    * `IQueryResolver` - An interface capable of locating an `IQueryProcessor` for a given `IQuery`
  * `./Events`
    * `IEvent` - an empty interface that is used by the generic logic to identify queries.
    * `IEventHandler` - An interface capable of processing a query.  There can be multiple event handlers registered.
    * `IEventResolver` - An interface capable of locating an `IEventHandler` for a given `IEvent`

## LSTC.CheeseShop.Messages

The library that contains the definitions for all Commands, Queries and Events for the project.  These have been separated so that CQEs can be shared between projects without having to share behaviours.

* Commands
  * `CreateProductCommand`
* Queries
  * `ListAllProductsQuery`
* Events
  * `ProductCreatedEvent`

## LSTC.CheeseShop.Domain

The library that contains all the domain logic for the project.

* Entities
  * `Product`
  * `Location`


## LSTC.CheeseShop.CQS

The Command and Query Processors and Event Handlers

* Command Processors
  * `CreateProductCommandProcessor`
* Query Processors
  * `ListAllProductsQueryProcessor`

## LSTC.CheeseShop.Domain.Tests

A library that contains all the tests for the project.
