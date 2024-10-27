# LibraryAPI
Test creating a api from openapi spec

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
