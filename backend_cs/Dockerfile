FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY backend_cs.csproj ./
RUN dotnet restore

COPY . .

RUN dotnet publish backend_cs.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish /app

ENV ASPNETCORE_URLS=http://+:8000

EXPOSE 8000

CMD ["dotnet", "backend_cs.dll"]
