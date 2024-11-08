# LibraryAPI
Test creating a api from openapi spec

Original spec https://libapi.1breadcrumb.com/

## Test

### http://localhost:5125/swagger


## Stub generation using NSwag

NSwag chosen as it generates less files and *might* have better date handling

# Generate Controller stubs manually

## Initial setup -  Install NSwag
```
dotnet tool install --global NSwag.ConsoleCore

# or just locally for your Project

dotnet add package NSwag.ApiDescription.Client

# Server stubs
nswag openapi2cscontroller /input:https://libapi.1breadcrumb.com/v1/swagger.json /output:Controllers.cs /namespace:LibraryAPI

# Optional CS Client stubs
nswag openapi2csclient /input:https://libapi.1breadcrumb.com/v1/swagger.json /namespace:LibraryClient /output:LibraryClient.cs
```

# TODO script from CI/CD
github actions?

# TODO openapi.json generation
separate project probably, has a  quick look but did not work
can then generate client stubs on demand

# TODO revisit repository pattern 

# TODO add paging

# Authentication/Authorisation
Followed this as a guide https://blog.devgenius.io/applying-jwt-access-tokens-and-refresh-tokens-in-asp-net-core-web-api-fc757c9191b9
as there are many ways to skin the authcat...:lolz-cat: 