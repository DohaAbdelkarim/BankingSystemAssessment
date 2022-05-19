# BankingSystemAssessment

BankingSystemAssessment is an open-source web project where users can create bank accounts to existed customer, view Customer information, accounts and transactions.

# Documentation
# Getting started

BankingSystemAssessment solution consists of 5 projects
- BankingSystemAssessment.API : API project that contains the endpoints.
- BankingSystemAssessment.IntegrationTest: Integration test project for the API endpoints using xunit.
- BankingSystemAssessment.UnitTest: unit test project for the API services using xunit.
- BankingSystemAssessment.Web: A simple Razor web Project to show basic UI -It doesn't show all the API capbailities like searching, sorting, pagination-
- BankingSystemAssessment.Core: A Library Standard projet that contains the ErrorHandling module, Polly to make sure of the API resiliance, common Models that serve the solution. This project is published as a private Nuget package and added to BankingSystemAssessment.API, BankingSystemAssessment.Web project

# To run the project:
There is a Database script file in BankingSystemAssessment.API/Infrastructure/Data folder that contains both schema and data. You can run it locally and change the BankingSystem connenction string in appsettings.json

Once the database is ready you can start the project.
The solution enables multiple startup projects First BankingSystemAssessment.API then BankingSystemAssessment.Web
- BankingSystemAssessment.API: http://localhost:5000/swagger/index.html
- BankingSystemAssessment.Web: https://localhost:10000/

BankingSystemAssessment.IntegrationTest and BankingSystemAssessment.UnitTest uses InMemeory database.

# Further enhancemnet to be done:
- Implement Authentication and authorization
- The API design is simple without any unneeded features. It will differ once we start implementing Account types, Transaction types, etc. 

UI enhancemnets:
- Enhance the styling
- add validation messages
