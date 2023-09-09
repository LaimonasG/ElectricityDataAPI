    FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
    WORKDIR /app

    COPY ["Girteka_task.csproj", "."]
    RUN dotnet restore
    COPY . .
    RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

    FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
    WORKDIR /app
    COPY --from=build /app/publish .

    EXPOSE 80
    EXPOSE 443
    ENTRYPOINT ["dotnet", "Girteka_task.dll"]