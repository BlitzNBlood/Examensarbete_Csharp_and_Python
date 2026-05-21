FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet restore

WORKDIR /app

COPY . .

RUN dotnet publish MyApi.csproj -c Release -o /app/publish

COPY --from=build /app/publish .

EXPOSE 8000

CMD ["dotnet", "MyApi.dll"]
