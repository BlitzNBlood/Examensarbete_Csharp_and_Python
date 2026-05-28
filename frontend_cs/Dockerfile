FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY frontend_cs.csproj ./
RUN dotnet restore

COPY . .

RUN dotnet publish frontend_cs.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8501

EXPOSE 8501

CMD ["dotnet", "frontend_cs.dll"]
