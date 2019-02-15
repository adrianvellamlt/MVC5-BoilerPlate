# MVC5-BoilerPlate
A three-tier architecture applied to the default Visual Studio MVC App.

- The DataLayer handles all database connections. EntityFramework and SQL Server are used as persistence in this example. A database connection cannot be opened outside this layer.

- The Business Layer handles all logic. This segregation allows for easier and better testability.  All BLs should extend the ContextHandler (in DataLayer) to access a set of CRUD methods that are typed dynamically and work seamlessly with EntityFramework. The advantage of this implementation is that all connections and transactions are handled in the ContextHandler and are common across all BLs.

- The Presentation Layer is the MVC app serves all web pages or endpoints in case of an API implementation.
