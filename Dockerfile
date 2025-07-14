# Use .NET 9 Preview SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything
COPY . ./

# Restore dependencies
RUN dotnet restore

# Build the app
RUN dotnet publish -c Release -o /app/publish

# Use ASP.NET runtime to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (default for Kestrel)
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "Sufra.dll"]
