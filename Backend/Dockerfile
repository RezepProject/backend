# Use the ASP.NET runtime image for the base layer
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY Backend/Backend.csproj ./Backend/  # Adjusted to copy the .csproj to the right folder
WORKDIR "/src/Backend"
RUN dotnet restore "Backend.csproj"

# Copy the rest of the application code
COPY Backend/. ./  # This ensures that all files from the Backend directory are copied

# Build the application
RUN dotnet build "Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Set up the final base image with the published application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]
