# Use the .NET 8 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy all files into the container
COPY . ./

# Publish the correct project
RUN dotnet publish ./GLP.Basecode.API.SariSariStoreProduct/GLP.Basecode.API.SariSariStoreProduct/GLP.Basecode.API.SariSariStoreProduct.csproj -c Release -o out

# Use the ASP.NET Core 8.0 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out .

# Run the app
ENTRYPOINT ["dotnet", "GLP.Basecode.API.SariSariStoreProduct.dll"]
