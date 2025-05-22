# Use the .NET 9 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy all files into the container
COPY . ./

# Restore using the solution path
RUN dotnet restore ./GLP.Basecode.API.SariSariStoreProduct/GLP.Basecode.API.SariSariStore.sln

# Publish the main project
RUN dotnet publish ./GLP.Basecode.API.SariSariStoreProduct/GLP.Basecode.API.SariSariStore/GLP.Basecode.API.SariSariStore.csproj -c Release -o out

# Use the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "GLP.Basecode.API.SariSariStore.dll"]
