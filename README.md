# Introduction

This project was written in .NET6 using Microsoft SQL Server as database and redis for caching. EF core is the ORM being used. Docker was also used to containerized the application with the database and redis cache.
FixerIo is being used as 3rd party to get the latest exchange rates.
Apart from the following brief overview there is also a swagger documentation attached.

# Design Patterns

The CurrencyExchange microservice pattern is based on MVC and the repository pattern. Task-based asynchronous pattern (TAP) was also used. Configurations were done in the appsettings for scalable options. Generics were also used to provide less reduntant code.

Unit testing was done aswell for regression testing.
