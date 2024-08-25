#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BudgetBuddyAPI/BudgetBuddyAPI.csproj", "BudgetBuddyAPI/"]
COPY ["BudgetBuddy.Infrastructure/BudgetBuddy.Infrastructure.csproj", "BudgetBuddy.Infrastructure/"]
COPY ["BudgetBuddy.Core/BudgetBuddy.Core.csproj", "BudgetBuddy.Core/"]
COPY ["BudgetBuddy.Domain/BudgetBuddy.Domain.csproj", "BudgetBuddy.Domain/"]
RUN dotnet restore "BudgetBuddyAPI/BudgetBuddyAPI.csproj"
COPY . .
COPY BudgetBuddyAPI/StaticFiles /app/StaticFiles

WORKDIR "/src/BudgetBuddyAPI"
RUN dotnet build "BudgetBuddyAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetBuddyAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY BudgetBuddyAPI/StaticFiles /app/StaticFiles
ENTRYPOINT ["dotnet", "BudgetBuddyAPI.dll"]