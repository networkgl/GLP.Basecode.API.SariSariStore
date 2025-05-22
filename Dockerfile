# Use .NET SDK to build the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution and project files
COPY . ./

# Restore dependencies
RUN dotnet restore GLP.Basecode.API.SariSariStore.sln

# Build and publish the project
RUN dotnet publish GLP.Basecode.API.SariSariStore/GLP.Basecode.API.SariSariStore.csproj -c Release -o out

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "GLP.Basecode.API.SariSariStore.dll"]
